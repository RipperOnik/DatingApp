using API.Extensions;
using API.Middleware;

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

app.Run();
