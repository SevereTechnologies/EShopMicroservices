var builder = WebApplication.CreateBuilder(args);

// Add services to the containter

var app = builder.Build();  // separator between add services and configure services

// Configure the HTTP request pipeline

app.Run(); // run the application
