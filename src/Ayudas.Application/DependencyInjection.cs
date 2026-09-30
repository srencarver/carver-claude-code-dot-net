using Ayudas.Application.Servicios;
using Microsoft.Extensions.DependencyInjection;

namespace Ayudas.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAplicacion(this IServiceCollection services)
    {
        services.AddScoped<IEntidadService, EntidadService>();
        services.AddScoped<IRemesaService, RemesaService>();
        services.AddScoped<IAyudaService, AyudaService>();
        services.AddScoped<IInformeService, InformeService>();
        services.AddScoped<ICatalogoService, CatalogoService>();
        services.AddScoped<IConvocatoriaService, ConvocatoriaService>();
        return services;
    }
}
