using StackExchange.Redis;
using WavesOfFoodDemo.Server.AppSettings;
using WavesOfFoodDemo.Server.DataContext;
using WavesOfFoodDemo.Server.Services;
using WavesOfFoodDemo.Server.Services.Implements;

var builder = WebApplication.CreateBuilder(args);
// settings
var postgreSetting = new PostgreSetting();
builder.Configuration.Bind("PostgreSetting", postgreSetting);
builder.Services.AddSingleton(postgreSetting);
// Load c?u hình Redis
var redisConfig = builder.Configuration.GetSection("Redis:ConnectionString").Value;
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfig));
builder.Services.AddSingleton<IRedisService, RedisService>();
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", policyOption =>
    {
        policyOption.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
    });
});

builder.Services.AddApplicationServicesExtension();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProductDbContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("AllowOrigin");

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