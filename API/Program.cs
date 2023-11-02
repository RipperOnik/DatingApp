using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);



var app = builder.Build();
// Exception middleware has to be here 
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.

// set cors headers
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.UseAuthentication(); // do you have a valid token?
app.UseAuthorization(); // are you authorized to do smh

app.UseHttpsRedirection();

app.MapControllers();

// seeding data to database
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    // migrating on every app launch 
    await context.Database.MigrateAsync();
    // seeding data
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
