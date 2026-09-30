using System.Net;
using System.Net.Http.Json;

namespace Ayudas.Infrastructure.Externos;

/// <summary>
/// Cliente de la API del Registro de ayudas (servicio externo).
/// La URL base se configura en appsettings (RegistroAyudas:UrlBase).
/// </summary>
public class AyudasApiClient
{
    private readonly HttpClient _http;

    public AyudasApiClient(HttpClient http)
    {
        _http = http;
    }

    /// <summary>Estado de una convocatoria en el Registro. Null si el Registro no la conoce.</summary>
    public async Task<EstadoConvocatoriaRegistro?> ObtenerEstadoConvocatoriaAsync(string codigo, CancellationToken ct = default)
    {
        using var respuesta = await _http.GetAsync($"api/registro/convocatorias/{Uri.EscapeDataString(codigo)}", ct);

        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        respuesta.EnsureSuccessStatusCode();
        return await respuesta.Content.ReadFromJsonAsync<EstadoConvocatoriaRegistro>(cancellationToken: ct);
    }

    /// <summary>Registra una remesa. No es idempotente: cada llamada crea un registro nuevo.</summary>
    public async Task<AcuseRegistro> RegistrarRemesaAsync(RemesaRegistro remesa, CancellationToken ct = default)
    {
        using var respuesta = await _http.PostAsJsonAsync("api/registro/remesas", remesa, ct);
        respuesta.EnsureSuccessStatusCode();

        return await respuesta.Content.ReadFromJsonAsync<AcuseRegistro>(cancellationToken: ct)
            ?? throw new InvalidOperationException("El Registro de ayudas ha devuelto una respuesta vacía.");
    }
}

public record EstadoConvocatoriaRegistro(string Codigo, bool Abierta, decimal ImporteDisponible);

public record RemesaRegistro(string Referencia, string NifEntidad, string CodigoConvocatoria, int NumeroSolicitudes, decimal ImporteTotal);

public record AcuseRegistro(string NumeroRegistro, DateTime FechaRegistro);
