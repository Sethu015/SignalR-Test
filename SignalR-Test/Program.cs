using Microsoft.EntityFrameworkCore;
using SignalR_Test.EFModels;
using SignalR_Test.HubConfig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Add DB Context
builder.Services.AddDbContextPool<SignalrDbContext>(options =>
{

    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString"));
});
//Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllHeaders", policy =>
    {
        policy.AllowAnyHeader()
        .AllowAnyMethod().AllowAnyOrigin();
    });
});
//Add SignalR
builder.Services.AddSignalR(cfg =>
{
    cfg.EnableDetailedErrors = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllHeaders");

app.UseAuthorization();

app.MapControllers();

app.MapHub<MyHub>("/toastr");

app.Run();
