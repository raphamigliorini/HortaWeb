using FiapWebAluno;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace FiapWebAluno.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Se quiser mudar algo só para o ambiente de teste, faz aqui.
        builder.UseEnvironment("Development");
    }
}
