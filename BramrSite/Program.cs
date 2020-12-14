using Blazored.LocalStorage;
using BramrSite.Auth;
using BramrSite.Classes;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tewr.Blazor.FileReader;

namespace BramrSite
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped<JWTAuthentication>();

            builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthentication>(
                provider => provider.GetRequiredService<JWTAuthentication>());

            builder.Services.AddScoped<ITokenHandler, JWTAuthentication>(
                provider => provider.GetRequiredService<JWTAuthentication>());

            builder.Services.AddBlazoredLocalStorage(config =>
                config.JsonSerializerOptions.WriteIndented = true);

            builder.Services.AddFileReaderService(option => {
                option.UseWasmSharedBuffer = true;
            });

            builder.Services.AddSingleton<ApiService>();

            await builder.Build().RunAsync();
        }
    }
}
