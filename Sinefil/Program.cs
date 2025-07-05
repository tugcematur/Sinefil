using Microsoft.EntityFrameworkCore;
using Sinefil.Models.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache(); // Bellek i�i cache (Session verilerini saklamak i�in)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session s�resi
    options.Cookie.HttpOnly = true; // Session cookie'sinin yaln�zca HTTP �zerinden eri�ilebilir olmas�n� sa�lar
    options.Cookie.IsEssential = true; // Cookie'nin gerekli olmas�n� sa�lar
});

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SinefilContext>(options =>
   options.UseSqlServer("Server=DESKTOP-C1B5RJG\\SQLEXPRESS;Database=Sinefil;Trusted_Connection=True;TrustServerCertificate=True;"));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


app.UseSession();
app.Run();
