using HemenBilet.Data;
using HemenBilet.Models;
using HemenBilet.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// MongoDbSettings'i yapılandırma
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// MongoClient'i DI container'a ekleme
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// MongoDbContext ve diğer servisleri DI container'a ekleme
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<FlightService>();
builder.Services.AddScoped<ApiService>();

// MVC servislerini ekleme
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Uygulama başlarken ApiService'i kullanarak uçuş verilerini kaydetme
using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    var apiService = scopedProvider.GetRequiredService<ApiService>();

    try
    {
        await apiService.FetchAndSaveAllFlightData();
        Console.WriteLine("Uçuş verileri başarıyla alındı ve kaydedildi.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Veri işleme sırasında bir hata oluştu: {ex.Message}");
    }
}
using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    var flightService = scopedProvider.GetRequiredService<FlightService>();

    await flightService.CreateTestFlightAsync(); // Test uçuşunu ekle
}


// HTTP request pipeline yapılandırması
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
