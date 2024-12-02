using AddressService.Middlewares;
using AddressService.Services;
using Serilog;

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

// регистрация HttpClient для DadataClient
builder.Services.AddHttpClient<DadataClient>(client =>
{
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// регистрация зависимостей
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
