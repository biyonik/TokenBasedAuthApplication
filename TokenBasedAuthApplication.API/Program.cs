using TokenBasedAuthApplication.API.Extensions;
using TokenBasedAuthApplication.Core.Configuration;
using TokenBasedAuthApplication.SharedLibrary.Authentication;
using TokenBasedAuthApplication.SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddManuelService(builder.Configuration);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomException();
// app.UseCors("All");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();