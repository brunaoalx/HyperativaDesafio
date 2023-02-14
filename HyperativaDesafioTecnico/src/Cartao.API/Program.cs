using AutoMapper;
using HyperativaDesafio.API.AutoMapper;
using HyperativaDesafio.Application;
using HyperativaDesafio.Application.Interfaces;
using HyperativaDesafio.Domain.Interfaces.Repositories;
using HyperativaDesafio.Domain.Interfaces.Services;
using HyperativaDesafio.Domain.Services;
using HyperativaDesafio.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ICartaoAppService, CartaoAppService>();

builder.Services.AddScoped<ICartaoService, CartaoService>();

builder.Services.AddScoped<ICartaoRepository, CartaoRepository>();


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
    .WriteTo.File("logHyperativaDesafioApi.txt",
        LogEventLevel.Warning,
        rollingInterval: RollingInterval.Day));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
