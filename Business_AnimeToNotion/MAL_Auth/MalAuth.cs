using Azure.Core;
using Azure.Data.AppConfiguration;
using Business_AnimeToNotion.Model.Auth;
using Business_AnimeToNotion.Model.MAL;
using Data_AnimeToNotion.DataModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Cryptography;

namespace Business_AnimeToNotion.MAL_Auth
{
    public class MalAuth : IMalAuth
    {
        private readonly IConfiguration _configuration;

        public MalAuth(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GeneratePKCECodeVerifier()
        {
            //https://developers.tapkey.io/api/authentication/pkce/
            var rng = RandomNumberGenerator.Create();

            var bytes = new byte[96];
            rng.GetBytes(bytes);

            var code_verifier = Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');

            return code_verifier;
        }

        public string BuildAuthorisationUrl(string code_verifier, string state)
        {
            return $"https://myanimelist.net/v1/oauth2/authorize?"
                + "response_type=code&"
                + $"client_id={_configuration["MAL-ApiKey"]}&"
                + $"state={state}&"
                + $"code_challenge={code_verifier}";
        }

        public bool CheckStateParameter(string state)
        {
            switch (state)
            {
                case "postman":
                case "browser":
                case "test":
                case "app":
                    return true;
            }
            return false;
        }

        public async Task GetAccessToken(string code, string code_verifier)
        {
            string url = "https://myanimelist.net/v1/oauth2/token";

            Dictionary<string, string> reqData = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", _configuration["MAL-ApiKey"] },
                { "code_verifier", code_verifier },
                { "grant_type", "authorization_code" },
            };

            HttpClient client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(reqData) };
            client.Timeout = TimeSpan.FromSeconds(20);
            var response = await client.SendAsync(req);

            var result = await response.Content.ReadAsStringAsync();
            ResponseTokens tokens = JsonConvert.DeserializeObject<ResponseTokens>(result);

            var appConfig = new ConfigurationClient("temp");
            appConfig.SetConfigurationSetting("MalRefreshToken", tokens.refresh_token);
        }
    }
}
