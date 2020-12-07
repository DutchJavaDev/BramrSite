using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BramrSite.Classes.Interfaces;
using BramrSite.Classes;
using BramrSite.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
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

            builder.Services.AddSingleton<IApiDesignConnection, ApiDesignConnection>();
            builder.Services.AddSingleton<ApiService>();

            await builder.Build().RunAsync();
        }
    }
}
