using MaksymKostyshyn.MindTrack.Data.EF.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MindTrack")
    ?? throw new InvalidOperationException("Connection string 'MindTrack' was not found. Configure it via user secrets.");

builder.Services.AddMindTrackEntityFrameworkDataAccess(connectionString);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

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
