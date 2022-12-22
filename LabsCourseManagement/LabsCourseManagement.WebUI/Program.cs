using FluentValidation;
using FluentValidation.AspNetCore;
using LabsCourseManagement.Application;
using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Infrastructure;
using LabsCourseManagement.WebUI;
using LabsCourseManagement.WebUI.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(AnnouncementValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CourseValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(LaboratoryValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(ProfessorValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(StudentValidator).Assembly);
builder.Services.AddAplicationServices();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "My API" });
    c.SwaggerDoc("v2", new OpenApiInfo { Version = "v2", Title = "My API" });
});

builder.Services.AddWebUIServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("labsCourseCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
