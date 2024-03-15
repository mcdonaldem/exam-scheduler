using Microsoft.EntityFrameworkCore;
using ExamScheduler.Contexts;
using ExamScheduler.Services.Interfaces;
using ExamScheduler.Services;
using Microsoft.AspNetCore.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using ExamScheduler.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMentorService, MentorService>();
builder.Services.AddScoped<IOutputSerializerService, OutputSerializerService>();
builder.Services.AddScoped<IParsingService, ParsingService>();
builder.Services.AddScoped<ISchedulingService, SchedulingService>();
builder.Services.AddScoped<IStudentService, StudentService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ApplicationContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = Text.Plain;
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            await context.Response.WriteAsync(exceptionHandlerPathFeature!.Error.ToErrorInfoString());
        });
    });
}

app.UseStatusCodePages();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();