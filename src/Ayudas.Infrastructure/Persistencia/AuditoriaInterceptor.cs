using Ayudas.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ayudas.Infrastructure.Persistencia;

/// <summary>
/// Rellena los campos de auditoría de las entidades locales modificadas antes de guardar.
/// </summary>
public class AuditoriaInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        MarcarModificaciones(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        MarcarModificaciones(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void MarcarModificaciones(DbContext? contexto)
    {
        if (contexto is null)
        {
            return;
        }

        foreach (var entrada in contexto.ChangeTracker.Entries<EntidadLocal>())
        {
            if (entrada.State == EntityState.Modified)
            {
                entrada.Entity.FechaModificacion = DateTime.Now;
                entrada.Entity.UsuarioModificacion = Environment.UserName;
            }
        }
    }
}
