using gcp.api.auth.domain;
using gcp.api.cloud.storage;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuracion de Service Account 
builder.Services.Configure<GCPServiceAccountKey>(builder.Configuration.GetSection("GCP-images-service-account"));

// Services
builder.Services.AddScoped<IGCPBucketService, GCPBucketService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();