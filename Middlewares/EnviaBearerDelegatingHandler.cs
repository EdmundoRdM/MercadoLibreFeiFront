using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;

namespace frontendnet.Middlewares;

public class EnviaBearerDelegatingHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("Authorization", "Bearer " + httpContextAccessor.HttpContext?.User.FindFirstValue("jwt"));
        return base.SendAsync(request, cancellationToken);
    }
}
