using CityExplorerV2.Services;
using CityExplorerV2.Config;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add framework services
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbService>();

// Configure External API's
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ExternalApiService>();

var app = builder.Build();

// Force MongoDbService to be instantiated at startup
using (var scope = app.Services.CreateScope())
{
    var mongoCheck = scope.ServiceProvider.GetRequiredService<MongoDbService>();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();