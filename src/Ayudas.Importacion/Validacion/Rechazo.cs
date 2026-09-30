namespace Ayudas.Importacion.Validacion;

/// <summary>Motivo por el que un campo de una solicitud no se acepta.</summary>
public record Rechazo(int Fila, string Campo, string? Valor, string Motivo);
