using AddressService.Middlewares;
using AddressService.Services;
using Serilog;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateSlimBuilder(args);

// ��������� Serilog
builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.Console()
          .WriteTo.File("log/log-.txt", rollingInterval: RollingInterval.Day);
});

// ����������� DaData �������� ����� IOptions
builder.Services.Configure<DaDataSettings>(
    builder.Configuration.GetSection("DaData"));

// ����������� ��������������� HttpClient � �������������� IOptions
builder.Services.AddHttpClient<IDadataService, DaDataService>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<DaDataSettings>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("Authorization", $"Token {options.ApiKey}");
});

// ����������� ������������
builder.Services.AddScoped<IDadataService, DaDataService>(); // DI ��� �������
builder.Services.AddAutoMapper(typeof(Program));            // AutoMapper
builder.Services.AddControllers();                          // �����������
builder.Services.AddEndpointsApiExplorer();                 // API Explorer ��� Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "�������������� �������",
        Version = "v1",
        Description = "API �������������� ����� DaData",
    });
});

// ��������� Kestrel ��� ������ � HTTP � HTTPS
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