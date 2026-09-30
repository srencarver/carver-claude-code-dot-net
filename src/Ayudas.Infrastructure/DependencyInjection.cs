using Ayudas.Application.Abstracciones;
using Ayudas.Infrastructure.Persistencia;
using Ayudas.Infrastructure.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ayudas.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructura(this IServiceCollection services, string cadenaConexion)
    {
        services.AddSingleton<AuditoriaInterceptor>();

        services.AddDbContext<AyudasDbContext>((sp, opciones) =>
            opciones
                .UseSqlServer(cadenaConexion)
                .AddInterceptors(sp.GetRequiredService<AuditoriaInterceptor>()));

        services.AddScoped<IEntidadRepository, EntidadRepository>();
        services.AddScoped<IRemesaRepository, RemesaRepository>();
        services.AddScoped<IAyudaRepository, AyudaRepository>();
        services.AddScoped<IInformeRepository, InformeRepository>();
        services.AddScoped<IMunicipioRepository, MunicipioRepository>();
        services.AddScoped<IConvocatoriaRepository, ConvocatoriaRepository>();

        return services;
    }
}
