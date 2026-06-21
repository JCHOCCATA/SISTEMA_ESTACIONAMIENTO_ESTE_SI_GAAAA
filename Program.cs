using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;
using SistemaEstacionamiento.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EstacionamientoModels>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("CadenaConexion"),
        ServerVersion.Parse("9.4.0-mysql")
    )
);

// eeeeuuu maniño aca se registra los servicios asi que cuando crees uno nuevo lo registras aca para que se pueda usar en los controladores gaaaaa
builder.Services.AddScoped<UsuariosService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<EntidadesService>();
builder.Services.AddScoped<TipoDocumentoService>();
builder.Services.AddScoped<SedesService>();
builder.Services.AddScoped<SitiosService>();
builder.Services.AddScoped<RegistrosEstacionamientoService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var usuarioService = services.GetRequiredService<UsuariosService>();

        bool creado = await usuarioService.CrearUsuarioXDefecto();

        if (creado)
        {
            Console.WriteLine("--> [Inicio] Usuario por defecto verificado/creado con éxito.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> [Inicio] Error al ejecutar el servicio de inicio: {ex.Message}");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();