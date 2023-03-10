using TokenBasedAuthApplication.Business.Services;
using TokenBasedAuthApplication.SharedLibrary.Authentication;
using TokenBasedAuthApplication.SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();
var symmetricSecurityKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey);
builder.Services.GetAuthenticationConfiguration(builder.Configuration, tokenOptions, symmetricSecurityKey);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();