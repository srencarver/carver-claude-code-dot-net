using Ayudas.Domain.Entidades;

namespace Ayudas.Tests.Unit.Fakes;

/// <summary>
/// Objetos de prueba con valores válidos por defecto.
/// </summary>
public static class Datos
{
    public static EntidadLocal Entidad(int id = 1, string nif = "P2807900B", string? direccion = "Plaza Mayor, 1") => new()
    {
        Id = id,
        Nif = nif,
        Nombre = $"Ayuntamiento de prueba {id}",
        Tipo = TipoEntidad.Ayuntamiento,
        CodigoMunicipio = "28079",
        Provincia = "Madrid",
        DireccionNotificacion = direccion,
        Activa = true
    };

    public static Convocatoria Convocatoria(decimal porcentaje = 0.80m) => new()
    {
        Id = 1,
        Codigo = "CONV-2026-01",
        Titulo = "Convocatoria de prueba",
        Ejercicio = 2026,
        PorcentajeCofinanciacion = porcentaje,
        FechaInicio = new DateOnly(2026, 1, 1),
        FechaFin = new DateOnly(2026, 12, 31),
        Abierta = true
    };

    public static Remesa Remesa(int id, EntidadLocal entidad, Convocatoria convocatoria, params decimal[] importesConcedidos)
    {
        var remesa = new Remesa
        {
            Id = id,
            Referencia = $"REM-2026-{id:D6}",
            EntidadLocal = entidad,
            EntidadLocalId = entidad.Id,
            Convocatoria = convocatoria,
            ConvocatoriaId = convocatoria.Id,
            FechaEnvio = new DateTime(2026, 3, 1, 10, 0, 0),
            Estado = EstadoRemesa.Recibida
        };

        var numero = 1;
        foreach (var importe in importesConcedidos)
        {
            remesa.Ayudas.Add(new Ayuda
            {
                Id = id * 100 + numero,
                NifBeneficiario = "12345678Z",
                NombreBeneficiario = $"Beneficiario {numero}",
                CodigoMunicipio = "28079",
                Concepto = "Concepto de prueba",
                ImporteSolicitado = importe,
                ImporteConcedido = importe,
                FechaSolicitud = new DateOnly(2026, 2, 15)
            });
            numero++;
        }

        return remesa;
    }
}
