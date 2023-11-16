using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    public TokenService(IConfiguration config)
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
    }
    public string CreateToken(AppUser user)
    {
        // Step 1: Define the claims for the JWT, which typically include user-specific data.
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
        };

        // Step 2: Define the credentials used for signing the token, typically with a secret key.
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        // Step 3: Create a descriptor for the JWT token, specifying its properties.
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // Define the subject of the token, which typically includes the claims.
            Subject = new ClaimsIdentity(claims),

            // Set the expiration date for the token (7 days from now in this case).
            Expires = DateTime.Now.AddDays(7),

            // Set the signing credentials, which include the key and the algorithm.
            SigningCredentials = creds
        };

        // Step 4: Create a JWT security token handler.
        var tokenHandler = new JwtSecurityTokenHandler();

        // Step 5: Create a JWT token using the descriptor.
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Step 6: Write the JWT token to a string.
        return tokenHandler.WriteToken(token);
    }

}
