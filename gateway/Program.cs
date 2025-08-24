using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 明確設置監聽端口為80
builder.WebHost.UseUrls("http://0.0.0.0:80");

// Load Ocelot routes
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

// 添加 Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Ecommerce Microservices Gateway API", 
        Version = "v1",
        Description = "API Gateway for Ecommerce Microservices using Ocelot"
    });
});

var app = builder.Build();

// 配置靜態文件 - 必須在 Ocelot 之前
app.UseStaticFiles();

// 配置 Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce Gateway API V1");
        c.RoutePrefix = "swagger";
    });
}

// 設置根路徑和默認文檔 - 必須在 Ocelot 之前
app.MapGet("/", () => Results.File("wwwroot/index.html", "text/html"));
app.MapGet("/index.html", () => Results.File("wwwroot/index.html", "text/html"));

// 添加一個簡單的健康檢查端點
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Service = "Gateway", Timestamp = DateTime.UtcNow }));

// 最後配置 Ocelot
await app.UseOcelot();
app.Run();
