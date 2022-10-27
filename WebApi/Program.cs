using System.Data.Entity.Core;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using StoreApi.DataLayer.DataRepository.AutoMapperProfiles;
using StoreApi.DataLayer.DataRepository.Models;
using StoreApi.Shared.Services.ExceptionSecurityChecker;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddConfiguration(StoreApi.Shared.Settings.ConfigurationProvider.GetConfiguration());
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(c=>{
    var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:StoreApi");
    if(string.IsNullOrEmpty(connectionString)) throw new Exception("Unable to start application, connection string not found!");
    
    c.UseSqlServer(connectionString);
});

builder.Services.AddTransient<IExceptionSecurityChecker>(x => {
    return new ExceptionSecurityChecker(
        typeof(NullReferenceException),
        typeof(InvalidDataException),
        typeof(ObjectNotFoundException),
        typeof(DuplicateWaitObjectException)
    );
});

var dataLayerMapper = AutoMapperConfiguration.Create();
if(dataLayerMapper == null)
    throw new Exception("Application cannot create auto mapper data layer configuration");

builder.Services.AddSingleton(op=>{
    return dataLayerMapper.CreateMapper();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
