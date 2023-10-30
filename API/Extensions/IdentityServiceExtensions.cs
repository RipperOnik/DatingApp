using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        // Add JWT Bearer authentication with the specified configuration
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
            });

        // Return the modified service collection
        return services;
    }
}

