using ApiAlmacen.Context;
using ApiDockerTecnimotors.Repositories.MaestroArticulo.Interface;
using ApiDockerTecnimotors.Repositories.MaestroArticulo.Repo;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromHours(2);
    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromHours(2); //horas 2 horas se probara mañana
});

var Configuration = builder.Configuration;

builder.Services.AddControllers();
var postgreSQLConnectionConfiguration = new PostgreSQLConfiguration(Configuration.GetConnectionString("PostgreSQLConnection")!);
builder.Services.AddSingleton(postgreSQLConnectionConfiguration);

builder.Services.AddScoped<IMaestroArticuloRepository, MaestroArticuloRepository>();

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