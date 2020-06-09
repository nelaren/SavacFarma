using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SavacFarma.Client.Shared.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;


namespace SavacFarma.Client.Shared.Auth
{
    using System.Globalization;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.Extensions.Configuration;
    using SavacFarma.Client.Repositorios;
    using SavacFarma.Shared.Localization;
    using SavacFarma.Shared.DTOs;
    using SavacFarma.Shared.Resources;

    public class ProveedorAutenticacionJWT : AuthenticationStateProvider, ILoginService, IAsyncDisposable
    {

        public ProveedorAutenticacionJWT(IJSRuntime js, HttpClient httpClient,IRepositorio repositorio, ILocalizationManager localizationManager)
        {
            this.js = js;
            this.httpClient = httpClient;
            _repositorio = repositorio;
            _localizationManager = localizationManager;
        }

        public static readonly string TOKENKEY = "TOKENKEY";
        public static readonly string EXPIRATIONTOKENKEY = "EXPIRATIONTOKENKEY";
        private readonly IJSRuntime js;
        private readonly HttpClient httpClient;
        private readonly IRepositorio _repositorio;
        private readonly ILocalizationManager _localizationManager;

        private AuthenticationState Anonimo =>
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await js.GetFromLocalStorage(TOKENKEY);

            if (string.IsNullOrEmpty(token))
            {
                return Anonimo;
            }
            string expirationTokenKey = await js.GetFromLocalStorage(EXPIRATIONTOKENKEY);

            DateTime expirationTime;
            if (DateTime.TryParse(expirationTokenKey, out expirationTime))
            {
                if (TokenHasExpired(expirationTime))
                {
                    await ClearStorage();
                    return Anonimo;
                }
                else
                {
                    if (HasToRenovateToken(expirationTime))
                    {
                        token = await RenovateToken(token);
                    }
                }
            }


            return ConstruirAuthenticationState(token);
        }

        public async Task HandlingTokenExpiration()
        {
            string expirationTokenKey = await js.GetFromLocalStorage(EXPIRATIONTOKENKEY);

            DateTime expirationTime;
            if (DateTime.TryParse(expirationTokenKey, out expirationTime))
            {
                if (TokenHasExpired(expirationTime))
                {
                    await Logout();
                }
                else
                {
                    if (HasToRenovateToken(expirationTime))
                    {
                        var token = await js.GetFromLocalStorage(TOKENKEY);
                        var authState = ConstruirAuthenticationState(token);
                        NotifyAuthenticationStateChanged(Task.FromResult(authState));
                    }
                }
            }
        }

        private bool TokenHasExpired(DateTime expirationTime)
        {
            return (expirationTime <= DateTime.UtcNow);
        }
        private bool HasToRenovateToken(DateTime expirationTime)
        {
            return expirationTime.Subtract(DateTime.UtcNow) <  TimeSpan.FromMinutes(5);
        }

        private async Task<string> RenovateToken(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var responseNuevoTtoken = await _repositorio.Get<UserToken>("api/usuarios/RenovateToken");
            var nuevoToken = responseNuevoTtoken.Response;
            await js.SetInLocalStorage(TOKENKEY, nuevoToken.Token);
            await js.SetInLocalStorage(EXPIRATIONTOKENKEY, nuevoToken.Expiration.ToString());
            return nuevoToken.Token;

        }

        public AuthenticationState ConstruirAuthenticationState(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
            return new AuthenticationState(claimsPrincipal);
        }


        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }
            

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task Login(UserToken token)
        {
            await js.SetInLocalStorage(TOKENKEY, token.Token);
            await js.SetInLocalStorage(EXPIRATIONTOKENKEY, token.Expiration.ToString());
            var authState = ConstruirAuthenticationState(token.Token);

            
            var ci = authState.User.Claims.FirstOrDefault(k => k.Type == "CultureInfoName");
            if (ci != null)
            {
                var temp = ci.Value.ToString();
                _localizationManager.SetLanguage(temp);
            }
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await ClearStorage();
            httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
        }

        private async Task ClearStorage()
        {
            await js.RemoveItem(TOKENKEY);
            await js.RemoveItem(EXPIRATIONTOKENKEY);
        }

        public async ValueTask DisposeAsync()
        {
            await Logout();
        }
    }


}
