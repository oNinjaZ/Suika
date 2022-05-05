using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Suika.Api.Auth;

public class ApiKeyAuthHandler : AuthenticationHandler<ApiKeyAuthSchemeOptions>
{
    public ApiKeyAuthHandler(IOptionsMonitor<ApiKeyAuthSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // check if request contains auth header
        if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));
        }

        //if so, check if authorization header key matches api key
        var header = Request.Headers["Authorization"].ToString();
        if (header != Options.ApiKey)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));
        }

        //if so, generate claims + ticket
        var claims = new[]{
            new Claim(ClaimTypes.Email, "ninjaz.gg@outlook.com"),
            new Claim(ClaimTypes.Name, "NinjaZ")
        };

        var claimsIdentity = new ClaimsIdentity(claims, "ApiKey");

        var ticket = new AuthenticationTicket(
            new ClaimsPrincipal(claimsIdentity), Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}