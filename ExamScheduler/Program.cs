using Microsoft.EntityFrameworkCore;
using ExamScheduler.Contexts;
using ExamScheduler.Services.Interfaces;
using ExamScheduler.Services;

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

app.MapGet("/", () => "Hello World!");

app.Run();
