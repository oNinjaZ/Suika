using Microsoft.AspNetCore.Authentication;

namespace Suika.Api.Auth;

public class ApiKeyAuthSchemeOptions : AuthenticationSchemeOptions
{
    public string ApiKey { get; set; } = "ggmom";
 
}