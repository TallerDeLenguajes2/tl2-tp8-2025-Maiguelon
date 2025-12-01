using tl2_tp8_2025_Maiguelon.Interfaces;
using tl2_tp8_2025_Maiguelon.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// -------------------------------------------------------------------------
// 1. INYECCIÓN DE DEPENDENCIAS (TP 10 - Fase 2)
// -------------------------------------------------------------------------
// Registramos los Repositorios: "Cuando pidan la Interfaz, dales la Clase Concreta"
// Nota: Si tus repos están en el namespace global, no necesitas el using de arriba.
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IPresupuestoRepository, PresupuestoRepository>();
builder.Services.AddScoped<IUserRepository, UsuarioRepository>();

// Registramos el Servicio de Autenticación
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Registramos el acceso al contexto HTTP (Necesario para usar Session en el Servicio)
builder.Services.AddHttpContextAccessor();


// -------------------------------------------------------------------------
// 2. CONFIGURACIÓN DE SESIONES (TP 10 / Tema 15)
// -------------------------------------------------------------------------
// Habilitamos la memoria caché para guardar las sesiones
builder.Services.AddDistributedMemoryCache();

// Configuramos las opciones de la Cookie de Sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // La sesión muere tras 30 mins de inactividad
    options.Cookie.HttpOnly = true; // Seguridad: JS no puede leer la cookie
    options.Cookie.IsEssential = true; // La cookie es necesaria para que la app funcione
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// -------------------------------------------------------------------------
// 3. ACTIVAR EL MIDDLEWARE DE SESIÓN
// -------------------------------------------------------------------------
// IMPORTANTE: Debe ir ANTES de UseAuthorization y DESPUÉS de UseRouting
app.UseSession(); 

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();