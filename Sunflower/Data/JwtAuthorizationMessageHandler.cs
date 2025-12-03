using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

public class JwtAuthorizationMessageHandler : DelegatingHandler
{
    private readonly IJSRuntime _js;

    public JwtAuthorizationMessageHandler(IJSRuntime js)
    {
        _js = js;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Get the JWT token from localStorage
        var token = await _js.InvokeAsync<string>("localStorage.getItem", "jwtToken");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
