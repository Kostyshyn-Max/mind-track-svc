using MaksymKostyshyn.MindTrack.Application.Extensions;
using MaksymKostyshyn.MindTrack.Auth.Extensions;
using MaksymKostyshyn.MindTrack.Data.ADO.Extensions;
using MaksymKostyshyn.MindTrack.Data.EF.Extensions;
using MaksymKostyshyn.MindTrack.WebSvc.ExceptionHandling;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MindTrack")
    ?? throw new InvalidOperationException("Connection string 'MindTrack' was not found. Configure it via user secrets.");

builder.Services.AddControllers(); 
builder.Services.AddHttpClient();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    options.AddSecurityDefinition("google", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                TokenUrl = new Uri("/api/auth/token", UriKind.Relative),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "OpenID" },
                    { "email", "Email" },
                    { "profile", "Profile" }
                }
            }
        }
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("google", document)] = [ "openid", "email", "profile" ]
    });
});

builder.Services.AddOpenApi();

builder.Services.AddMindTrackEntityFrameworkDataAccess(connectionString);
builder.Services.AddMindTrackAdoDataAccess(connectionString);
builder.Services.AddMindTrackApplication();
builder.Services.AddMindTrackAuth(builder.Configuration);

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<AuthExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.Configure<RouteOptions>(options => 
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        c.OAuthClientId(builder.Configuration["Authentication:Google:ClientId"]);
        c.OAuthUsePkce();
        c.OAuthScopeSeparator(" ");
        c.OAuthScopes(["openid", "email", "profile"]);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
