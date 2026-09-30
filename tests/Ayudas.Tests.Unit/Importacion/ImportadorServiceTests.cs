using Ayudas.Domain.Entidades;
using Ayudas.Importacion;
using Ayudas.Importacion.Lectores;
using Ayudas.Importacion.Validacion;
using Ayudas.Tests.Unit.Fakes;

namespace Ayudas.Tests.Unit.Importacion;

/// <summary>
/// Pruebas de extremo a extremo del importador con las muestras reales del repositorio
/// (copiadas a la salida de la compilación) y repositorios falsos con las entidades de las muestras.
/// </summary>
public class ImportadorServiceTests
{
    private readonly EntidadRepositoryFalso _entidades = new();
    private readonly ConvocatoriaRepositoryFalso _convocatorias = new();
    private readonly RemesaRepositoryFalso _remesas = new();
    private readonly ImportadorService _importador;

    public ImportadorServiceTests()
    {
        var nifs = new[]
        {
            "P0110000G", "P0313700G", "P0717400F", "P0821100E", "P1424800I", "P1528500J",
            "P1832200H", "P2435900B", "P2839600J", "P2943300J", "P3047000I"
        };
        for (var i = 0; i < nifs.Length; i++)
        {
            _entidades.Entidades.Add(Datos.Entidad(i + 1, nifs[i]));
        }

        _convocatorias.Convocatorias.Add(Convocatoria(1, "CONV-2026-01", new DateOnly(2026, 1, 15), new DateOnly(2026, 12, 15)));
        _convocatorias.Convocatorias.Add(Convocatoria(2, "CONV-2026-02", new DateOnly(2026, 4, 1), new DateOnly(2026, 12, 31)));

        _importador = new ImportadorService(
            new IAyudasLector[] { new AyudasXmlReader() },
            _entidades,
            _convocatorias,
            _remesas,
            new AyudaEntradaValidator());
    }

    public static IEnumerable<object[]> Muestras() =>
        Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "muestras"), "remesa-*.xml")
            .OrderBy(f => f)
            .Select(f => new object[] { Path.GetFileName(f) });

    [Theory]
    [MemberData(nameof(Muestras))]
    public async Task ImportarAsync_CadaMuestra_NoFallaYCuadraElRecuento(string muestra)
    {
        var resultado = await Importar(Path.Combine("muestras", muestra));

        Assert.False(string.IsNullOrEmpty(resultado.Referencia));
        if (!resultado.RemesaRechazada)
        {
            Assert.Equal(_remesas.Remesas.Single().Ayudas.Count, resultado.Aceptadas);
        }
    }

    [Theory]
    [InlineData("remesa-01-madrid.xml", 4, 0)]
    [InlineData("remesa-02-malaga.xml", 3, 0)]
    [InlineData("remesa-04-palma.xml", 4, 0)]
    [InlineData("remesa-07-coruna.xml", 2, 2)]
    [InlineData("remesa-10-madrid.xml", 2, 3)]
    [InlineData("remesa-15-palma.xml", 1, 2)]
    public async Task ImportarAsync_MuestrasConocidas_AceptaYRechazaLoEsperado(string muestra, int aceptadas, int rechazadas)
    {
        var resultado = await Importar(Path.Combine("muestras", muestra));

        Assert.Equal(aceptadas, resultado.Aceptadas);
        Assert.Equal(rechazadas, resultado.Rechazadas);
    }

    [Fact]
    public async Task ImportarAsync_ConvocatoriaInexistente_RechazaLaRemesaEntera()
    {
        var resultado = await Importar(Path.Combine("muestras", "remesa-19-leon.xml"));

        Assert.True(resultado.RemesaRechazada);
        Assert.Empty(_remesas.Remesas);
    }

    [Fact]
    public async Task ImportarAsync_CabeceraQueNoCuadra_AvisaSinRechazar()
    {
        var resultado = await Importar(Path.Combine("muestras", "remesa-13-malaga.xml"));

        Assert.False(resultado.RemesaRechazada);
        Assert.Equal(2, resultado.Avisos.Count);
    }

    [Fact]
    public async Task ImportarAsync_MismaRemesaDosVeces_LaSegundaSeRechaza()
    {
        await Importar(Path.Combine("muestras", "remesa-01-madrid.xml"));

        var segunda = await Importar(Path.Combine("muestras", "remesa-01-madrid.xml"));

        Assert.True(segunda.RemesaRechazada);
        Assert.Single(_remesas.Remesas);
    }

    [Fact]
    public async Task ImportarAsync_ImporteTotalConComaDecimal_NoFallaYLoInterpreta()
    {
        // Reproduce el fallo de producción de logs/error.txt (FormatException en LeerImporte).
        var resultado = await Importar(Path.Combine("muestras", "fallos", "remesa-100231-cordoba.xml"));

        Assert.Equal(3, resultado.Aceptadas);
        Assert.Empty(resultado.Avisos);
    }

    private async Task<ResultadoImportacion> Importar(string rutaRelativa)
    {
        await using var fichero = File.OpenRead(Path.Combine(AppContext.BaseDirectory, rutaRelativa));
        return await _importador.ImportarAsync(fichero);
    }

    private static Convocatoria Convocatoria(int id, string codigo, DateOnly inicio, DateOnly fin) => new()
    {
        Id = id,
        Codigo = codigo,
        Titulo = codigo,
        Ejercicio = 2026,
        PorcentajeCofinanciacion = 0.8m,
        FechaInicio = inicio,
        FechaFin = fin,
        Abierta = true
    };
}
