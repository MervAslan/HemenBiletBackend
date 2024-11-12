// using Microsoft.Extensions.Options;
// using MongoDB.Driver;
// using HemenBilet.Models;
// using HemenBilet.Data;
// using HemenBilet.Services;



// var builder = WebApplication.CreateBuilder(args);
// builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
// builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
// {
//     var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
//     return new MongoClient(settings.ConnectionString);
// });

// builder.Services.AddSingleton<MongoDbContext>();  // MongoDbContext'i enjekte et
// builder.Services.AddScoped<ApiService>();
// // Add services to the container.
// builder.Services.AddControllersWithViews();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

// app.Run();
using HemenBilet.Data;
using HemenBilet.Models;
using HemenBilet.Services;

var builder = WebApplication.CreateBuilder(args);

// MongoDbSettings'i yapılandırma ve bağımlılıkları kaydetme
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<ApiService>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    var apiService = scopedProvider.GetRequiredService<ApiService>();

    await apiService.FetchAndSaveAllFlightData();
}


// Configure the HTTP request pipeline.
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
