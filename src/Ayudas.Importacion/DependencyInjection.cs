using Ayudas.Importacion.Lectores;
using Ayudas.Importacion.Validacion;
using Microsoft.Extensions.DependencyInjection;

namespace Ayudas.Importacion;

public static class DependencyInjection
{
    public static IServiceCollection AddImportacion(this IServiceCollection services)
    {
        // Un lector por formato de fichero; el importador elige el que soporta cada documento.
        services.AddSingleton<IAyudasLector, AyudasXmlReader>();
        services.AddSingleton<IAyudasLector, DiputacionXmlReader>();
        services.AddSingleton<AyudaEntradaValidator>();
        services.AddScoped<ImportadorService>();
        return services;
    }
}
