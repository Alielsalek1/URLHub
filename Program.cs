using ALL.Database;
using Microsoft.EntityFrameworkCore;
using URLshortner.Controllers;
using URLshortner.Repositories;
using URLshortner.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Database
builder.Services.AddDbContext<AppDbContext>(
     bd => bd.UseSqlServer("server=LEGIONFORELSALE;database=URLDB;integrated security=true;trust server certificate=True;")
);

// Add Database repositories
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<FriendRepository>();
builder.Services.AddScoped<URLRepository>();

// Add Validators
builder.Services.AddScoped<UserValidator>();
builder.Services.AddScoped<FriendValidator>();
builder.Services.AddScoped<URLValidator>();

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