//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.EntityFrameworkCore;
using Lab_1;
using Lab_1.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Lab_1.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<Lab1Context>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("Lab1Context") ?? throw new InvalidOperationException("Connection string 'Lab1Context' not found.")));

builder.Services.AddEntityFrameworkSqlite().AddDbContext<Lab1Context>();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // укзывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = AuthOptions.ISSUER,

            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,

            // будет ли валидироваться время существования
            ValidateLifetime = true,

            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        };
    });

builder.Services.AddControllers();
builder.Services.AddCors();

builder.Services.AddTransient<FlightManager>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.UseUrls("https://localhost:7017");

var app = builder.Build();

//don't do it in real production code!!!
app.UseCors(cpb => cpb
    .SetIsOriginAllowed(_ => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
