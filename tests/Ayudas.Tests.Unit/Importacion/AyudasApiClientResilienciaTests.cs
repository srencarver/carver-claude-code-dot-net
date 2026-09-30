using System.Net;
using System.Net.Http.Json;
using Ayudas.Infrastructure.Externos;
using Microsoft.Extensions.DependencyInjection;

namespace Ayudas.Tests.Unit.Importacion;

/// <summary>
/// Comprueba la configuración real de resiliencia (AddRegistroAyudas) con un handler falso
/// que devuelve las respuestas que se le indiquen, en orden.
/// </summary>
public class AyudasApiClientResilienciaTests
{
    [Fact]
    public async Task ObtenerEstado_DosFallosTransitorios_ReintentaYDevuelveElEstado()
    {
        var falso = new HandlerFalso(HttpStatusCode.ServiceUnavailable, HttpStatusCode.ServiceUnavailable, HttpStatusCode.OK);
        var cliente = CrearCliente(falso);

        var estado = await cliente.ObtenerEstadoConvocatoriaAsync("CONV-2026-01");

        Assert.NotNull(estado);
        Assert.Equal(3, falso.Llamadas);
    }

    [Fact]
    public async Task ObtenerEstado_ErrorDelCliente400_NoReintenta()
    {
        var falso = new HandlerFalso(HttpStatusCode.BadRequest, HttpStatusCode.OK);
        var cliente = CrearCliente(falso);

        await Assert.ThrowsAsync<HttpRequestException>(() => cliente.ObtenerEstadoConvocatoriaAsync("CONV-2026-01"));

        Assert.Equal(1, falso.Llamadas);
    }

    [Fact]
    public async Task RegistrarRemesa_FalloTransitorio_NoReintentaPorqueElPostNoEsIdempotente()
    {
        var falso = new HandlerFalso(HttpStatusCode.ServiceUnavailable, HttpStatusCode.OK);
        var cliente = CrearCliente(falso);
        var remesa = new RemesaRegistro("REM-2026-100001", "P2839600J", "CONV-2026-01", 4, 1000m);

        await Assert.ThrowsAsync<HttpRequestException>(() => cliente.RegistrarRemesaAsync(remesa));

        Assert.Equal(1, falso.Llamadas);
    }

    private static AyudasApiClient CrearCliente(HandlerFalso falso)
    {
        var servicios = new ServiceCollection();
        servicios
            .AddRegistroAyudas(
                new OpcionesRegistroAyudas { UrlBase = "http://registro.test/" },
                resiliencia => resiliencia.Retry.Delay = TimeSpan.Zero)
            .ConfigurePrimaryHttpMessageHandler(() => falso);

        return servicios.BuildServiceProvider().GetRequiredService<AyudasApiClient>();
    }

    private sealed class HandlerFalso : HttpMessageHandler
    {
        private readonly Queue<HttpStatusCode> _respuestas;

        public HandlerFalso(params HttpStatusCode[] respuestas)
        {
            _respuestas = new Queue<HttpStatusCode>(respuestas);
        }

        public int Llamadas { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Llamadas++;
            var codigo = _respuestas.Count > 0 ? _respuestas.Dequeue() : HttpStatusCode.OK;
            var respuesta = new HttpResponseMessage(codigo);

            if (codigo == HttpStatusCode.OK)
            {
                respuesta.Content = request.Method == HttpMethod.Get
                    ? JsonContent.Create(new EstadoConvocatoriaRegistro("CONV-2026-01", true, 1000m))
                    : JsonContent.Create(new AcuseRegistro("RA-2026-000001", DateTime.Now));
            }

            return Task.FromResult(respuesta);
        }
    }
}
