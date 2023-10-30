
using System.Text;
using API.Data;
using API.Interfaces;
using API.Services;
using API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);




// builder.Services.AddDbContext<DataContext>(opt =>
// {
//     opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
// });
// builder.Services.AddCors();

// builder.Services.AddScoped<ITokenService, TokenService>();

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true, // otherwise our server won't check if token was signed by the issuer
//             IssuerSigningKey = new SymmetricSecurityKey(
//                 Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])
//             ),
//             ValidateIssuer = false,
//             ValidateAudience = false
//         };

//     });

var app = builder.Build();

// Configure the HTTP request pipeline.

// set cors headers
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.UseAuthentication(); // do you have a valid token?
app.UseAuthorization(); // are you authorized to do smh

app.UseHttpsRedirection();


app.MapControllers();

app.Run();
