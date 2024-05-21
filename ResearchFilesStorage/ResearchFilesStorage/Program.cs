using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using ResearchFilesStorage.Domain.Repository;
using ResearchFilesStorage.Infrastructure;
using System.Text.Json.Serialization;
using ResearchFilesStorage.Application;
using ResearchFilesStorage.Domain.Builders;
using ResearchFilesStorage.Application.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                     .AddEnvironmentVariables()
                     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: false, reloadOnChange: true);

//Add mongoDb
builder.Services.Configure<ResearchFileDatabaseSettings>(builder.Configuration.GetSection("ResearchFileDatabase"));
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
builder.Services.AddScoped<IResearchFileRepository, ResearchFileRepository>();

builder.Services.AddTransient<IFileBuilder, FileBuilder>();
builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();

//Add mediator 
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

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
