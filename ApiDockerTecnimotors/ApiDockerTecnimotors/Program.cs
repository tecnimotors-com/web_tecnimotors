using ApiDockerTecnimotors.Context;
using ApiDockerTecnimotors.Repositories.Auth.Interface;
using ApiDockerTecnimotors.Repositories.Auth.Repo;
using ApiDockerTecnimotors.Repositories.CarritoList.Interface;
using ApiDockerTecnimotors.Repositories.CarritoList.Repository;
using ApiDockerTecnimotors.Repositories.Distribuidores.Interface;
using ApiDockerTecnimotors.Repositories.Distribuidores.Repo;
using ApiDockerTecnimotors.Repositories.MaestroArticulo.Interface;
using ApiDockerTecnimotors.Repositories.MaestroArticulo.Repo;
using ApiDockerTecnimotors.Repositories.MaestroClasificado.Interface;
using ApiDockerTecnimotors.Repositories.MaestroClasificado.Repo;
using ApiDockerTecnimotors.Repositories.WishList.Interface;
using ApiDockerTecnimotors.Repositories.WishList.Repository;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Configurar el límite de tamaño de la solicitud
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});

// Configurar Kestrel para aumentar el límite de tamaño de la solicitud
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 104857600; // 100 MB
});

var Configuration = builder.Configuration;

builder.Services.AddControllers();
var postgreSQLConnectionConfiguration = new PostgreSQLConfiguration(Configuration.GetConnectionString("PostgreSQLConnection")!);
builder.Services.AddSingleton(postgreSQLConnectionConfiguration);

builder.Services.AddScoped<IMaestroArticuloRepository, MaestroArticuloRepository>();
builder.Services.AddScoped<IMaestroClasificado, MaestroClasificado>();
builder.Services.AddScoped<IDistribuidoresRepository, DistribuidoresRepository>();
builder.Services.AddScoped<IAuthInterface, AuthRepository>();
builder.Services.AddScoped<IWishList, WIshList>();
builder.Services.AddScoped<ICarritoList, CarritoList>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Maestro Articulo v1"));
}
else
{
    app.UseExceptionHandler(
        options =>
        {
            options.Run(
                async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                    if (ex != null)
                    {
                        await context.Response.WriteAsync(ex.Error.Message);
                    }
                }
            );
        }
    );
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(m => m.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();