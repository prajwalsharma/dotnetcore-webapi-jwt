# JWT-Authentication-NET-Core-Web-API-5.0

.NET Core Web API 5.0 with JWT authentication.

- Generate JWT token & return as a string.

- Generate JWT token & return in response cookie.

- Refresh JWT token & cookie for silent token refresh.

  

Tutorial: [ASP.NET 5: Authorization and Authentication with Bearer and JWT](https://levelup.gitconnected.com/asp-net-5-authorization-and-authentication-with-bearer-and-jwt-2d0cef85dc5d)



### 1. Add below packages from Nuget

```c#
dotnet add package Microsoft.AspNetCore.Authentication 
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```



### 2. Create a Token Service

```c#
public string GenerateJwtToken(User user)
{

    // 1. Convert secret key to byte array
    var key = Encoding.ASCII.GetBytes(JWT_TOKEN);

    // 2. Create a token handler
    var tokenHandler = new JwtSecurityTokenHandler();

    // 3. Set properties for JWT token in a descriptor
    var descriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(
            new Claim[] {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            }
        ),
        Expires = DateTime.UtcNow.AddHours(EXPIRE_HOURS),
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key), 
            SecurityAlgorithms.HmacSha256Signature
        )
    };

    // 4. Create a token using token handler
    var token = tokenHandler.CreateToken(descriptor);

    // 5. Return the token string from token object
    return tokenHandler.WriteToken(token);

}
```



### 3. Add JWT Authentication in Startup file

```c#
public void ConfigureServices(IServiceCollection services)
{

    var JwtSecretKey = Encoding.ASCII.GetBytes("5d2efc20-d921-4319-840c-053e8c6c120b");

    services.AddAuthentication(x =>
                               {
                                   x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                   x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                               })
        .AddJwtBearer(x =>
                      {
                          x.RequireHttpsMetadata = false;
                          x.SaveToken = true;
                          x.TokenValidationParameters = new TokenValidationParameters
                          {
                              ValidateIssuerSigningKey = true,
                              IssuerSigningKey = new SymmetricSecurityKey(JwtSecretKey),
                              ValidateIssuer = false,
                              ValidateAudience = false
                          };
                      });

    services.AddControllers();
}
```

