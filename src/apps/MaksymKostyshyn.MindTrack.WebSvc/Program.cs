using MaksymKostyshyn.MindTrack.Application.Extensions;
using MaksymKostyshyn.MindTrack.Data.ADO.Extensions;
using MaksymKostyshyn.MindTrack.Data.EF.Extensions;
using MaksymKostyshyn.MindTrack.WebSvc.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MindTrack")
    ?? throw new InvalidOperationException("Connection string 'MindTrack' was not found. Configure it via user secrets.");

builder.Services.AddMindTrackEntityFrameworkDataAccess(connectionString);
builder.Services.AddMindTrackAdoDataAccess(connectionString);
builder.Services.AddMindTrackApplication();

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

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

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
