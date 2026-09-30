using Ayudas.Domain.Comun;
using Ayudas.Domain.Entidades;

namespace Ayudas.Infrastructure.Persistencia;

/// <summary>
/// Datos de prueba sintéticos. Ningún NIF ni nombre corresponde a personas reales.
/// La generación es determinista: siempre se crean los mismos datos.
/// </summary>
public static class DatosSemilla
{
    private static readonly (string Codigo, string Nombre, string Provincia)[] MunicipiosSemilla =
    {
        ("01059", "Vitoria-Gasteiz", "Araba/Álava"),
        ("03014", "Alicante/Alacant", "Alicante"),
        ("07040", "Palma", "Illes Balears"),
        ("08019", "Barcelona", "Barcelona"),
        ("14021", "Córdoba", "Córdoba"),
        ("15030", "A Coruña", "A Coruña"),
        ("18087", "Granada", "Granada"),
        ("24089", "León", "León"),
        ("28079", "Madrid", "Madrid"),
        ("29067", "Málaga", "Málaga"),
        ("30030", "Murcia", "Murcia"),
        ("33024", "Gijón", "Asturias"),
        ("36057", "Vigo", "Pontevedra"),
        ("37274", "Salamanca", "Salamanca"),
        ("39075", "Santander", "Cantabria"),
        ("41091", "Sevilla", "Sevilla"),
        ("46250", "València", "Valencia"),
        ("47186", "Valladolid", "Valladolid"),
        ("48020", "Bilbao", "Bizkaia"),
        ("50297", "Zaragoza", "Zaragoza")
    };

    private static readonly string[] Nombres =
    {
        "Lucía", "Hugo", "Martina", "Daniel", "Sofía", "Pablo", "Julia", "Álvaro",
        "Paula", "Adrián", "Carmen", "Javier", "Elena", "Sergio", "Irene", "Marcos"
    };

    private static readonly string[] Apellidos =
    {
        "Prueba", "Ficticio", "Ejemplo", "Muestra", "Ensayo", "Simulado",
        "Demostración", "Formación", "Modelo", "Patrón", "Borrador", "Maqueta"
    };

    private static readonly string[] Conceptos =
    {
        "Rehabilitación de vivienda",
        "Eficiencia energética",
        "Accesibilidad en edificio",
        "Comercio de proximidad",
        "Digitalización de pyme",
        "Empleo juvenil"
    };

    public static void Cargar(AyudasDbContext db)
    {
        var aleatorio = new Random(2026);

        var municipios = MunicipiosSemilla
            .Select(m => new Municipio { CodigoIne = m.Codigo, Nombre = m.Nombre, Provincia = m.Provincia })
            .ToList();
        db.Municipios.AddRange(municipios);

        var convocatorias = new List<Convocatoria>
        {
            new()
            {
                Codigo = "CONV-2025-02",
                Titulo = "Ayudas a la rehabilitación de vivienda 2025",
                Ejercicio = 2025,
                PorcentajeCofinanciacion = 0.75m,
                FechaInicio = new DateOnly(2025, 3, 1),
                FechaFin = new DateOnly(2025, 11, 30),
                Abierta = false
            },
            new()
            {
                Codigo = "CONV-2026-01",
                Titulo = "Ayudas a la eficiencia energética en municipios 2026",
                Ejercicio = 2026,
                PorcentajeCofinanciacion = 0.80m,
                FechaInicio = new DateOnly(2026, 1, 15),
                FechaFin = new DateOnly(2026, 12, 15),
                Abierta = true
            },
            new()
            {
                Codigo = "CONV-2026-02",
                Titulo = "Ayudas al comercio de proximidad 2026",
                Ejercicio = 2026,
                PorcentajeCofinanciacion = 0.70m,
                FechaInicio = new DateOnly(2026, 4, 1),
                FechaFin = new DateOnly(2026, 12, 31),
                Abierta = true
            }
        };
        db.Convocatorias.AddRange(convocatorias);

        var entidades = new List<EntidadLocal>();
        for (var i = 0; i < 12; i++)
        {
            var municipio = MunicipiosSemilla[i];
            var esDiputacion = i % 4 == 3;
            var digitos = municipio.Codigo[..2] + (100 + i * 37).ToString("D3") + "00";

            entidades.Add(new EntidadLocal
            {
                Nif = "P" + digitos + Nif.ControlOrganizacion('P', digitos),
                Nombre = esDiputacion ? $"Diputación Provincial de {municipio.Provincia}" : $"Ayuntamiento de {municipio.Nombre}",
                Tipo = esDiputacion ? TipoEntidad.Diputacion : TipoEntidad.Ayuntamiento,
                CodigoMunicipio = municipio.Codigo,
                Provincia = municipio.Provincia,
                // Dos entidades no tienen dirección de notificación informada.
                DireccionNotificacion = i is 6 or 10 ? null : $"Plaza Mayor, {i + 1}  -  {municipio.Codigo} {municipio.Nombre}",
                CorreoNotificacion = $"registro{i + 1:D2}@entidad-ficticia.example",
                Activa = i != 11,
                FechaAlta = new DateTime(2019, 1, 1).AddDays(i * 45)
            });
        }
        db.EntidadesLocales.AddRange(entidades);

        var estados = new[]
        {
            EstadoRemesa.Recibida, EstadoRemesa.EnValidacion, EstadoRemesa.Validada,
            EstadoRemesa.Validada, EstadoRemesa.Pagada, EstadoRemesa.Rechazada
        };

        var remesas = new List<Remesa>();
        for (var i = 0; i < 45; i++)
        {
            var entidad = entidades[i % entidades.Count];
            var convocatoria = convocatorias[i % convocatorias.Count];
            var fechaEnvio = convocatoria.FechaInicio.ToDateTime(new TimeOnly(9, 0)).AddDays(3 + i * 5 % 200).AddHours(i % 7);

            var remesa = new Remesa
            {
                Referencia = $"REM-{convocatoria.Ejercicio}-{i + 1:D6}",
                EntidadLocal = entidad,
                Convocatoria = convocatoria,
                FechaEnvio = fechaEnvio,
                Estado = convocatoria.Abierta ? estados[i % estados.Length] : EstadoRemesa.Pagada
            };

            var numeroAyudas = aleatorio.Next(2, 7);
            for (var j = 0; j < numeroAyudas; j++)
            {
                var solicitado = Math.Round((decimal)(aleatorio.Next(40000, 1200000) / 100.0), 2);
                var concedido = remesa.Estado == EstadoRemesa.Rechazada ? 0m : Math.Round(solicitado * 0.9m, 2);
                var municipioBeneficiario = aleatorio.Next(0, 3) == 0
                    ? MunicipiosSemilla[aleatorio.Next(MunicipiosSemilla.Length)].Codigo
                    : entidad.CodigoMunicipio;

                remesa.Ayudas.Add(new Ayuda
                {
                    NifBeneficiario = GenerarDni(aleatorio),
                    NombreBeneficiario = $"{Nombres[aleatorio.Next(Nombres.Length)]} {Apellidos[aleatorio.Next(Apellidos.Length)]} {Apellidos[aleatorio.Next(Apellidos.Length)]}",
                    CodigoMunicipio = municipioBeneficiario,
                    Concepto = Conceptos[aleatorio.Next(Conceptos.Length)],
                    ImporteSolicitado = solicitado,
                    ImporteConcedido = concedido,
                    FechaSolicitud = DateOnly.FromDateTime(fechaEnvio.AddDays(-aleatorio.Next(5, 40)))
                });
            }

            remesas.Add(remesa);
        }
        db.Remesas.AddRange(remesas);

        foreach (var remesa in remesas.Where(r => r.Estado is EstadoRemesa.Pagada or EstadoRemesa.Validada))
        {
            foreach (var ayuda in remesa.Ayudas.Where((_, indice) => indice % 2 == 0))
            {
                var estado = aleatorio.Next(0, 4) switch { 0 => "R", 1 => "A", _ => "P" };
                ayuda.Justificaciones.Add(new Justificacion
                {
                    FechaPresentacion = remesa.FechaEnvio.AddDays(aleatorio.Next(30, 120)),
                    ImporteJustificado = Math.Round(ayuda.ImporteConcedido * (decimal)(0.8 + aleatorio.NextDouble() * 0.3), 2),
                    CodigoEstado = estado,
                    Observaciones = estado == "R" ? "Facturas ilegibles" : null
                });
            }
        }
    }

    private static string GenerarDni(Random aleatorio)
    {
        var numero = aleatorio.Next(10000000, 99999999);
        return numero.ToString("D8") + Nif.LetraDni(numero);
    }
}
