using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 明確設置監聽端口為80
builder.WebHost.UseUrls("http://0.0.0.0:80");

builder.Services.AddControllers();

// 添加 Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Product Service API", 
        Version = "v1",
        Description = "Product management microservice API"
    });
});

var app = builder.Build();

// 配置 Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Service API V1");
        c.RoutePrefix = "swagger";
    });
}

app.MapControllers();
app.Run();
