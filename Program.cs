using API_CRUDTareas.Application;
using API_CRUDTareas.Interfaces;
using API_CRUDTareas.Models.Context;
using API_CRUDTareas.Models.DTOs;
using API_CRUDTareas.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Configuracion de Servicios

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddServices();

builder.Services.ConfigureSwagger();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureAppSettings(builder);
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureDbContext(builder.Configuration);

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
