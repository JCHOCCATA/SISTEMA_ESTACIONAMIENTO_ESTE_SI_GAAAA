using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EstacionamientoModels>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("CadenaConexion"),
        ServerVersion.Parse("9.4.0-mysql")
    )
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();