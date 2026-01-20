using GestionStock.API.Data;
using GestionStock.API.Hubs;
using GestionStock.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<JwtManager>();

builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuers = ["api"/*, "azure"*/],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:secret")!))
        };
    });


builder.Services.AddDbContext<StockContext>(o => 
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddCors(b => b.AddDefaultPolicy(o => 
    o.WithOrigins("http://localhost:5182", "http://localhost:5098")
    .AllowCredentials()
    .AllowAnyMethod()
    .AllowAnyHeader()
));

builder.Services.AddSignalR();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ProductHub>("ws/product", o => 
    o.Transports = HttpTransportType.WebSockets
);

app.Run();