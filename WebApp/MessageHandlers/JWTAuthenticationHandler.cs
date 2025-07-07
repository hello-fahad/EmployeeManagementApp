
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Model;

namespace WebApp.MessageHandlers
{
    public class JWTAuthenticationHandler : DelegatingHandler
    {

        public IHttpContextAccessor HttpContextAccessor { get; }
        public IConfiguration Configuration { get; }

        public JWTAuthenticationHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            HttpContextAccessor = httpContextAccessor;
            Configuration = configuration;
        }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var httpContext = HttpContextAccessor.HttpContext;
            var session = httpContext?.Session;

            // Get Token

            JsonWebToken token;

            string? strJWTToken = session?.GetString("access_token");
            if(string.IsNullOrWhiteSpace(strJWTToken))
            {
                token = await Login();
            }
            else
            {
                token = JsonSerializer.Deserialize<JsonWebToken>(strJWTToken) ?? new JsonWebToken();
            }

            // Handle token expiration
            if(token == null ||
                string.IsNullOrWhiteSpace(token.AccessToken) ||
                token.ExpiresAt <= DateTime.UtcNow)
            {
                token = await Login();
            }


            // Send token: add the token to authorize header
            if (!string.IsNullOrWhiteSpace(token.AccessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            }


            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<JsonWebToken> Login()
        {

            // Create a new Http Client
            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync($"{Configuration["WebApi:Url"]}account/login",
                new {
                    ClientId = Configuration["WebApi:ClientId"],
                    ClientSecret = Configuration["WebApi:ClientSecret"]
                });

            response.EnsureSuccessStatusCode();

            string strJwt = await response.Content.ReadAsStringAsync();

            // Store the token in the session
            HttpContextAccessor.HttpContext?.Session.SetString("access_token", strJwt);

            return JsonSerializer.Deserialize<JsonWebToken>(strJwt) ?? new JsonWebToken();
        }

    }
}
