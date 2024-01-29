using Azure.Core;
using Azure.Data.AppConfiguration;
using Business_AnimeToNotion.Model.Auth;
using Business_AnimeToNotion.Model.MAL;
using Data_AnimeToNotion.DataModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography;

namespace Business_AnimeToNotion.MAL_Auth
{
    public class MalAuth : IMalAuth
    {
        private readonly IConfiguration _configuration;
        private ConfigurationClient appConfig;

        public MalAuth(IConfiguration configuration)
        {
            _configuration = configuration;

            appConfig = new ConfigurationClient(_configuration["AppConfiguration-ConnectionString"]);
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

            await GetAndSaveTokens(url, reqData, 20);
        }

        public async Task<ResponseTokens> RefreshAccessToken(string client_id = null)
        {
            string refreshToken = (await appConfig.GetConfigurationSettingAsync("MalRefreshToken")).Value.Value;

            string url = "https://myanimelist.net/v1/oauth2/token";

            Dictionary<string, string> reqData = new Dictionary<string, string>
            {
                { "client_id", _configuration["MAL-ApiKey"] ?? client_id },
                { "refresh_token", refreshToken },
                { "grant_type", "refresh_token" },
            };

            return await GetAndSaveTokens(url, reqData, 10);
        }

        private async Task<ResponseTokens> GetAndSaveTokens(string url, Dictionary<string, string> data, int timeout)
        {
            HttpClient client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(data) };
            client.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await client.SendAsync(req);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            ResponseTokens tokens = JsonConvert.DeserializeObject<ResponseTokens>(result);
            
            await appConfig.SetConfigurationSettingAsync("MalRefreshToken", tokens.refresh_token);

            return tokens;
        }
    }
}
