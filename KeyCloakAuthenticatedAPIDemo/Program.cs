using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json.Linq;

using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add keycloak authentication


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer",options =>
    {
        // Set the metadata address for the OpenID configuration
        options.MetadataAddress = builder.Configuration["Keycloak:OpenIdConfigMetaAddr"]!;
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = builder.Configuration["Keycloak:ClientId"];
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Keycloak:Authority"],
            ValidAudience = builder.Configuration["Keycloak:Audience"],
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var claimsIdentity = context.Principal!.Identity as ClaimsIdentity;
                var claims = context.Principal!.Claims.ToList();

                // Extract roles from "resource_access.angular-app.roles"
                var resourceAccess = claims.FirstOrDefault(c => c.Type == "resource_access");
                if (resourceAccess != null)
                {
                    //Deserialize the resource access claim
                    var parsedResourceAccess = JObject.Parse(resourceAccess.Value);
                    var angularAppRoles = parsedResourceAccess["angular-app"]?["roles"]?.ToObject<List<string>>();

                    if (angularAppRoles != null)
                    {
                        foreach (var role in angularAppRoles)
                        {
                            claimsIdentity!.AddClaim(new Claim(ClaimTypes.Role,role)); // ✅ Map roles properly
                        }
                    }
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddHttpClient();

// Define a CORS policy with a name
var corsPolicyName = "AllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular app URL
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Required for cookies/auth headers
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(corsPolicyName);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
