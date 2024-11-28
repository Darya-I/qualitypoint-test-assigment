using AddressService.Middlewares;
using AddressService.Services;
using Serilog;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateSlimBuilder(args);

// настройка Serilog
builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.Console()
          .WriteTo.File("log/log-.txt", rollingInterval: RollingInterval.Day);
});

// регистрация DaData настроек через IOptions
builder.Services.Configure<DaDataSettings>(
    builder.Configuration.GetSection("DaData"));

// регистрация типизированного HttpClient с использованием IOptions
builder.Services.AddHttpClient<IDadataService, DaDataService>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<DaDataSettings>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("Authorization", $"Token {options.ApiKey}");
});

// регистрация зависимостей
builder.Services.AddScoped<IDadataService, DaDataService>(); // DI для сервиса
builder.Services.AddAutoMapper(typeof(Program));            // AutoMapper
builder.Services.AddControllers();                          // контроллеры
builder.Services.AddEndpointsApiExplorer();                 // API Explorer для Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Стандартизация адресов",
        Version = "v1",
        Description = "API стандартизации через DaData",
    });
});

// настройка Kestrel для работы с HTTP и HTTPS
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000);
    options.ListenLocalhost(5001, listenOptions => listenOptions.UseHttps());
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandling>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
