using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace FiapWebAluno.Auth
{
    public class SimpleRoleAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "SimpleAuth";

        public SimpleRoleAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("X-API-KEY", out var apiKeyValues))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var apiKey = apiKeyValues.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return Task.FromResult(AuthenticateResult.Fail("API Key vazia."));
            }

            string? role = apiKey switch
            {
                "admin-token"  => "Admin",
                "gestor-token" => "Gestor",
                _              => null
            };

            if (role is null)
            {
                return Task.FromResult(AuthenticateResult.Fail("API Key inválida."));
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "UsuarioAPI"),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, SchemeName);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, SchemeName);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
