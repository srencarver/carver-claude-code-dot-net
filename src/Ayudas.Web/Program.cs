using Ayudas.Application;
using Ayudas.Importacion;
using Ayudas.Infrastructure;
using Ayudas.Infrastructure.Externos;
using Ayudas.Infrastructure.Persistencia;
using Ayudas.Web.Legacy.Justificaciones;
using Microsoft.AspNetCore.Mvc.Razor;

// --reset-bd recrea la base de datos de formación; --solo-bd la prepara y termina.
var recrearBd = args.Contains("--reset-bd");
var soloBd = args.Contains("--solo-bd");
var argumentos = args.Where(a => a is not "--reset-bd" and not "--solo-bd").ToArray();

var builder = WebApplication.CreateBuilder(argumentos);

var cadenaConexion = builder.Configuration.GetConnectionString("Ayudas")
    ?? throw new InvalidOperationException("Falta la cadena de conexión 'Ayudas' en appsettings.json.");

builder.Services.AddControllersWithViews();

// El módulo heredado de justificaciones guarda sus vistas junto a su controlador.
builder.Services.Configure<RazorViewEngineOptions>(opciones =>
    opciones.ViewLocationFormats.Add("/Legacy/{1}/Views/{0}" + RazorViewEngine.ViewExtension));

builder.Services.AddAplicacion();
builder.Services.AddInfraestructura(cadenaConexion);

builder.Services.AddImportacion();

// Cliente del Registro de ayudas (API externa; en local la simula Ayudas.Api), con resiliencia.
var opcionesRegistro = builder.Configuration.GetSection("RegistroAyudas").Get<OpcionesRegistroAyudas>()
    ?? throw new InvalidOperationException("Falta la sección RegistroAyudas en appsettings.json.");
builder.Services.AddRegistroAyudas(opcionesRegistro);

JustificacionesDAL.CadenaConexion = cadenaConexion;

var app = builder.Build();

await InicializadorBd.InicializarAsync(app.Services, recrearBd);
if (soloBd)
{
    Console.WriteLine(recrearBd ? "Base de datos recreada con los datos de prueba." : "Base de datos preparada.");
    return;
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization("es-ES");
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
