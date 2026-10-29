using Microsoft.EntityFrameworkCore;     // UseMySql ke liye zaroori
using ProductManagement.Data;
using ProductManagement.Filters;
using ProductManagement.Filters.Filters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine("CONN = " + connStr);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySql(connStr, new MySqlServerVersion(new Version(8,0,45))));

builder.Services.AddScoped<AdoProductRepository>();
builder.Services.AddScoped<LogActionFilter>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();   // global filter
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();