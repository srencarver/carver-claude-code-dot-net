using Ayudas.Application.Dtos;
using Ayudas.Application.Excepciones;
using Ayudas.Application.Servicios;
using Ayudas.Tests.Unit.Fakes;

namespace Ayudas.Tests.Unit.Servicios;

public class EntidadServiceTests
{
    private readonly EntidadRepositoryFalso _repositorio = new();
    private readonly EntidadService _servicio;

    public EntidadServiceTests()
    {
        _repositorio.Entidades.Add(Datos.Entidad(1));
        _servicio = new EntidadService(_repositorio);
    }

    [Fact]
    public async Task ActualizarAsync_NifConFormatoLibre_SeGuardaLimpio()
    {
        var datos = DatosEdicion(nif: "p-2807900-b");

        await _servicio.ActualizarAsync(datos);

        Assert.Equal("P2807900B", _repositorio.Entidades[0].Nif);
        Assert.Equal(1, _repositorio.Guardados);
    }

    [Fact]
    public async Task ActualizarAsync_NifInvalido_LanzaValidacion()
    {
        var datos = DatosEdicion(nif: "P2807900C");

        var error = await Assert.ThrowsAsync<ValidacionException>(() => _servicio.ActualizarAsync(datos));

        Assert.Equal(nameof(EntidadEdicionDto.Nif), error.Campo);
        Assert.Equal(0, _repositorio.Guardados);
    }

    [Fact]
    public async Task ActualizarAsync_EntidadInexistente_LanzaNoEncontrada()
    {
        var datos = DatosEdicion(id: 99);

        await Assert.ThrowsAsync<EntidadNoEncontradaException>(() => _servicio.ActualizarAsync(datos));
    }

    [Fact]
    public async Task ActualizarAsync_DireccionEnBlanco_SeGuardaComoNula()
    {
        var datos = DatosEdicion();
        datos.DireccionNotificacion = "   ";

        await _servicio.ActualizarAsync(datos);

        Assert.Null(_repositorio.Entidades[0].DireccionNotificacion);
    }

    private static EntidadEdicionDto DatosEdicion(int id = 1, string nif = "P2807900B") => new()
    {
        Id = id,
        Nif = nif,
        Nombre = "Ayuntamiento de prueba",
        Tipo = Domain.Entidades.TipoEntidad.Ayuntamiento,
        CodigoMunicipio = "28079",
        Provincia = "Madrid",
        DireccionNotificacion = "Plaza Mayor, 1",
        Activa = true
    };
}
