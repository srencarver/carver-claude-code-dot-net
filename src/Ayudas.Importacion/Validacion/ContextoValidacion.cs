namespace Ayudas.Importacion.Validacion;

/// <summary>Datos de la remesa y de la convocatoria que necesitan las reglas de cada solicitud.</summary>
public record ContextoValidacion(DateOnly InicioPlazo, DateOnly FinPlazo, DateOnly FechaEnvio);
