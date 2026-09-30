using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;

namespace Ayudas.Infrastructure.Externos;

public static class RegistroAyudasExtensions
{
    /// <summary>
    /// Registra el cliente del Registro de ayudas con el handler estándar de resiliencia:
    /// reintentos solo en fallos transitorios (408, 429, 5xx y errores de red), tiempo máximo
    /// por intento y total, y circuit breaker. El POST de registro no se reintenta porque no es idempotente.
    /// </summary>
    public static IHttpClientBuilder AddRegistroAyudas(
        this IServiceCollection services,
        OpcionesRegistroAyudas opciones,
        Action<HttpStandardResilienceOptions>? ajustar = null)
    {
        var builder = services.AddHttpClient<AyudasApiClient>(cliente => cliente.BaseAddress = new Uri(opciones.UrlBase));

        builder.AddStandardResilienceHandler(resiliencia =>
        {
            resiliencia.Retry.MaxRetryAttempts = opciones.Reintentos;
            resiliencia.Retry.DisableForUnsafeHttpMethods();
            resiliencia.AttemptTimeout.Timeout = TimeSpan.FromSeconds(opciones.SegundosPorIntento);
            resiliencia.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(opciones.SegundosTotales);
            ajustar?.Invoke(resiliencia);
        });

        return builder;
    }
}

/// <summary>Sección RegistroAyudas de appsettings.json.</summary>
public class OpcionesRegistroAyudas
{
    public string UrlBase { get; set; } = string.Empty;

    public int Reintentos { get; set; } = 3;

    public int SegundosPorIntento { get; set; } = 10;

    public int SegundosTotales { get; set; } = 30;
}
