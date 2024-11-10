using Microsoft.Extensions.Options;
using MongoDB.Driver;
using pravra_api.Interfaces;
using pravra_api.Services;
using pravra_api.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// MongoDB configuration
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoClient, MongoClient>(
    s => new MongoClient(builder.Configuration.GetValue<string>("MongoDbSettings:ConnectionString")));
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // In development, show detailed exception pages
    app.UseDeveloperExceptionPage();
}
else
{
    // In production, use a global error handler that redirects to a custom error page
    app.UseExceptionHandler("/Home/Error"); // You can set this to a controller route or handle error logic
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();