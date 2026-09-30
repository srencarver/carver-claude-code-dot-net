using Ayudas.Application;
using Ayudas.Infrastructure;
using Ayudas.Infrastructure.Persistencia;

var builder = WebApplication.CreateBuilder(args);

var cadenaConexion = builder.Configuration.GetConnectionString("Ayudas")
    ?? throw new InvalidOperationException("Falta la cadena de conexión 'Ayudas' en appsettings.json.");

builder.Services.AddControllers();
builder.Services.AddAplicacion();
builder.Services.AddInfraestructura(cadenaConexion);

var app = builder.Build();

await InicializadorBd.InicializarAsync(app.Services);

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
