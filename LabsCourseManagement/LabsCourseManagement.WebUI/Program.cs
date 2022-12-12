using FluentValidation;
using FluentValidation.AspNetCore;
using LabsCourseManagement.Application;
using LabsCourseManagement.Application.Repositories;
using LabsCourseManagement.Infrastructure;
using LabsCourseManagement.WebUI.Dtos;
using Microsoft.EntityFrameworkCore;
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseContext>(
    options => options.UseSqlite(
        builder.Configuration.GetConnectionString("LabsCourseManagementDb"),
    b => b.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName)));


builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();

builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<ILaboratoryRepository, LaboratoryRepository>();
builder.Services.AddScoped<IStudentGradesRepository, StudentGradesRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ITimeAndPlaceRepository, TimeAndPlaceRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddCors(options =>
{
    options.AddPolicy("labsCourseCors", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

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
