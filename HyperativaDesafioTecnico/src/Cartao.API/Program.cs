using AutoMapper;
using HyperativaDesafio.API.AutoMapper;
using HyperativaDesafio.Application;
using HyperativaDesafio.Application.Interfaces;
using HyperativaDesafio.Domain.Interfaces.Repositories;
using HyperativaDesafio.Domain.Interfaces.Services;
using HyperativaDesafio.Domain.Services;
using HyperativaDesafio.Infra.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Inject for CartaoController
builder.Services.AddScoped<ICartaoAppService, CartaoAppService>();

builder.Services.AddScoped<ICartaoService, CartaoService>();

builder.Services.AddScoped<ICartaoRepository, CartaoRepository>();

//Inject for LoginController
builder.Services.AddScoped<IUsuarioAppService, UsuarioAppService>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Log
//https://serilog.net/
//on 2023-02-14
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console(LogEventLevel.Debug)
    .WriteTo.File("log\\HyperativaDesafioApi_.txt",
        LogEventLevel.Information,
        rollingInterval: RollingInterval.Day));


//Authentication step 1 of 2

var keyApi = Encoding.ASCII.GetBytes(SecurityService.apiKey);

builder.Services.AddAuthentication(
    x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(
    x => 
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyApi),
            ValidateIssuer = false,
            ValidateAudience = false
        };

    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Authentication step 2 of 2
app.UseAuthentication();


//Default
app.UseAuthorization();

app.MapControllers();

app.Run();
