using System.Text.Json.Serialization;
using StackExchange.Redis;
using WavesOfFoodDemo.Server.AppSettings;
using WavesOfFoodDemo.Server.DataContext;
using WavesOfFoodDemo.Server.Hubs;
using WavesOfFoodDemo.Server.Services.Implements;

var builder = WebApplication.CreateBuilder(args);
// settings
var postgreSetting = new PostgreSetting();
builder.Configuration.Bind("PostgreSetting", postgreSetting);
builder.Services.AddSingleton(postgreSetting);

// Load config Redis
var redisConfig = builder.Configuration.GetSection("Redis:Instances");
var defaultRedisConnString = redisConfig["Default"];
var secondaryRedisConnString = redisConfig["Secondary"];

// register defaut 6379 Redis connections
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(defaultRedisConnString));

// register secondary 6380 Redis connections w KeyedService
builder.Services.AddKeyedSingleton<IConnectionMultiplexer>("SecondaryRedis",
    (sp, _) => ConnectionMultiplexer.Connect(secondaryRedisConnString));

// config background service
builder.Services.AddHostedService<MLBackgroundService>();
builder.Services.AddHostedService<DailyProductBackgroundService>();

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", policyOption =>
    {
        policyOption.WithOrigins("http://localhost:8081", "http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddApplicationServicesExtension();
//builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProductDbContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSignalR();
builder.Services.AddHttpClient();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowOrigin");


// config signalR
app.MapHub<CartHub>("/cartHub");

app.UseRouting();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();