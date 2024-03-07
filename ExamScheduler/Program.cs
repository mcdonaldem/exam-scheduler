using Microsoft.EntityFrameworkCore;
using ExamScheduler.Contexts;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ApplicationContext>(
        options => options.UseSqlServer(builder.Configuration["DB_CONNECTION"]));
}

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
