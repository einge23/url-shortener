using IdGen.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var generatorIdStr = Environment.GetEnvironmentVariable("ID_GENERATOR_ID");
var generatorId = 0;

if (string.IsNullOrEmpty(generatorIdStr))
{
    throw new Exception("ID_GENERATOR_ID not set");
}
else if (!int.TryParse(generatorIdStr, out generatorId))
{
    throw new Exception($"Invalid ID_GENERATOR_ID value '{generatorIdStr}'");
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var cs = Environment.GetEnvironmentVariable("CONNECTION_STRING");
    options.UseMySql(cs, ServerVersion.AutoDetect(cs));
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
});

builder.Services.AddIdGen(generatorId);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
