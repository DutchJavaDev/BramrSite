﻿using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BramrSite.Classes;

namespace BramrSite.Auth
{
    public class JWTAuthentication : AuthenticationStateProvider, ITokenHandler
    {
        private readonly string AUTHTOKENKEY = "bramr_gYoRIYad";
        private readonly ILocalStorageService localStorage;
        private readonly ApiService apiService;

        private AuthenticationState Anonymous => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public JWTAuthentication(ILocalStorageService storage, ApiService api)
        {
            localStorage = storage;
            apiService = api;
        }

        /// <summary>
        /// Checks if there is a token in session storage, if there is then build a new state where the user is authenticated or else unautorized
        /// </summary>
        /// <returns></returns>
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorage.GetItemAsync<string>(AUTHTOKENKEY);

            if (string.IsNullOrEmpty(token))
            {
                await UpdateAutenticationState(string.Empty);
                return Anonymous;
            }
            
            return BuildAuthenticationState(token);
        }

        /// <summary>
        /// Updates the current state of the users authentication
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task UpdateAutenticationState(string token)
        {
            if (string.IsNullOrEmpty(token))
            {   // empty string is logout/no token
                apiService.SetToken(string.Empty);
                await localStorage.RemoveItemAsync(AUTHTOKENKEY);
                NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
            }
            else
            {
                // non empty string is login
                await localStorage.SetItemAsync(AUTHTOKENKEY, token);
                var authState = BuildAuthenticationState(token);
                NotifyAuthenticationStateChanged(Task.FromResult(authState));
            }
        }

        /// <summary>
        /// Makes sure that httpclient gets the token for sending request to authorized endpoints
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private AuthenticationState BuildAuthenticationState(string token)
        {
            apiService.SetToken(token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }

        /// <summary>
        /// Name says it
        /// </summary>
        /// <returns></returns>
        public async Task<bool> HasToken()
        {
            return await localStorage.ContainKeyAsync(AUTHTOKENKEY);
        }

        /// <summary>
        /// FROM STACKOVERFLOW: Parses the JWT token we can get the information out of it
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
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
    }
}
