using Business_AnimeToNotion.Model.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.MAL_Auth
{
    public interface IMalAuth
    {
        string GeneratePKCECodeVerifier();
        string BuildAuthorisationUrl(string code_verifier, string state);
        bool CheckStateParameter(string state);
        Task GetAccessToken(string code, string code_verifier);
        Task<ResponseTokens> RefreshAccessToken(string client_id = null);
    }
}
