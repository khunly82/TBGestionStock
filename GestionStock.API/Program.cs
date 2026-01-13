using GestionStock.API.Data;
using GestionStock.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();

builder.Services.AddDbContext<StockContext>(o => 
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddCors(b => b.AddDefaultPolicy(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
