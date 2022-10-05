using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<WaitAndRetryPolicy>(new WaitAndRetryPolicy(2, 5));
builder.Services.AddSingleton<ResponseTimeoutPolicy>(new ResponseTimeoutPolicy(1)); //seconds
builder.Services.AddSingleton<CircuitPolicy>(new CircuitPolicy(2, TimeSpan.FromMinutes(1)));
builder.Services.AddSingleton<RateLimitPolicy>(new RateLimitPolicy(10, TimeSpan.FromSeconds(5), 1));
builder.Services.AddSingleton<BulkHeadPolicy>(new BulkHeadPolicy(4));
builder.Services.AddSingleton<FallbackPolicy>(new FallbackPolicy());


//or
builder.Services.AddHttpClient("PolicyTest")
    .AddPolicyHandler(new FallbackPolicy().FallBack)
    .AddPolicyHandler(new WaitAndRetryPolicy().WaitAndRetry)
    .AddPolicyHandler(new CircuitPolicy(2, TimeSpan.FromMinutes(1)).Breaker)
    .AddPolicyHandler(new ResponseTimeoutPolicy(1).Timeout);


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

app.UseAuthorization();

app.MapControllers();

app.Run();

