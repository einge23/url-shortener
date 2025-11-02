using IdGen.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var replicaIdString = builder.Configuration["RAILWAY_REPLICA_ID"];
var replicaId = 0;
if (string.IsNullOrEmpty(replicaIdString))
{
    throw new Exception("RAILWAY_REPLICA_ID not set");
}
else 
{
    replicaId = replicaIdString.GetHashCode() % 1024;
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("CONNECTION_STRING");
    options.UseMySql(cs, ServerVersion.AutoDetect(cs));
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>  
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
});

builder.Services.AddIdGen(replicaId);


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
