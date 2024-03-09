using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    // Extension method to add identity services for JWT authentication
    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false; // specifying type of password 
            // opt.User.AllowedUserNameCharacters  - We can also tweak a user type 
        })
            .AddRoles<AppRole>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddEntityFrameworkStores<DataContext>(); // Adds an Entity Framework implementation of identity information stores.


        // Add JWT Bearer authentication with the specified configuration
        // This ensures the token validation 
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Configure token validation parameters
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Validate the issuer's signing key
                    ValidateIssuerSigningKey = true,

                    // Set the issuer's signing key from the configuration
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["TokenKey"])
                    ),

                    // Disable issuer and audience validation
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                // Settong a JWT token for the SignalR 
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"]; // we take it from the query params 

                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs")) // if we are using the correct path and if the token is ok 
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        // This ensures authorization for Authorize(Policy="RequireAdminRole") and Authorize(Policy="ModeratePhotoRole")
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            opt.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
        });

        // Return the modified service collection
        return services;
    }
}

