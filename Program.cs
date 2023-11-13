using Microsoft.EntityFrameworkCore;
using Quark_Backend.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<QuarkDbContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization(options =>
{

    options.AddPolicy("PermissionLevel1", policy =>
    {
        policy.RequireClaim("PermissionLevel", "1", "2", "3", "4");
    });

    options.AddPolicy("PermissionLevel2", policy =>
    {
        policy.RequireClaim("PermissionLevel", "2", "3", "4");
    });

    options.AddPolicy("PermissionLevel3", policy =>
    {
        policy.RequireClaim("PermissionLevel", "3", "4");
    });

    options.AddPolicy("PermissionLevel4", policy =>
    {
        policy.RequireClaim("PermissionLevel", "4");
    });
}); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();




app.Run();
