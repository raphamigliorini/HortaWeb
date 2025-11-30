using FiapWebAluno.Data.Contexts;
using FiapWebAluno.Service.Implementations;
using FiapWebAluno.Service.Interface;
using FiapWebAluno.Middlewares;
using FiapWebAluno.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services da aplicação (regras da horta ESG)
builder.Services.AddScoped<IEspecieService, EspecieService>();
builder.Services.AddScoped<ICanteiroService, CanteiroService>();
builder.Services.AddScoped<IIrrigacaoService, IrrigacaoService>();
builder.Services.AddScoped<ISensorService, SensorService>();

// Banco de dados (Oracle via EF Core)
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

builder.Services.AddDbContext<DatabaseContext>(opt =>
{
    opt.UseOracle(connectionString);

    if (builder.Environment.IsDevelopment())
    {
        opt.EnableSensitiveDataLogging(true);
    }
});

// Autenticação simples baseada em header (SimpleRoleAuth)
builder.Services
    .AddAuthentication("SimpleRoleAuth")
    .AddScheme<AuthenticationSchemeOptions, SimpleRoleAuthenticationHandler>(
        "SimpleRoleAuth",
        options => { }
    );

// Autorização (roles: Admin, Gestor, etc.)
builder.Services.AddAuthorization();

// MVC / Controllers
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware global de tratamento de erros
app.UseMiddleware<ErrorHandlingMiddleware>();

// Pipeline padrão
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Autenticação + Autorização
app.UseAuthentication();
app.UseAuthorization();

// Rotas MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Teste da conexão com o banco no startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

    try
    {
        var ok = db.Database.CanConnect();
        Console.WriteLine($"Conexão com Oracle: {ok}");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro ao conectar no Oracle:");
        Console.WriteLine(ex.Message);
    }
}

app.Run();

// Necessário para os testes xUnit com WebApplicationFactory<Program>
public partial class Program { }
