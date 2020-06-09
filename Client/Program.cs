using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using SavacFarma.Client.Repositorios;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using SavacFarma.Client.Shared.Auth;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Builder;
using SavacFarma.Shared.Localization;

namespace SavacFarma.Client
{
    public class Program 
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddTelerikBlazor();
            ConfigureServices(builder.Services);
            var host = builder.Build();
            await SetCultureAsync(host);
            await host.RunAsync();
        }
       

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRepositorio, Repositorio>();
          

            services.AddBlazoredToast();
            
            //Authetication (se ha adicionado el paquete Microsoft.AspNetCore.Components.Authorization)
            services.AddAuthorizationCore();
            services.AddScoped<ProveedorAutenticacionJWT>();

            //La misma clase se utilizará en la inyección 
            //como AuthenticationStateProvider y ILoginService
            //Esta dualidad se expresa como sigue

            services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacionJWT>(
                provider => provider.GetRequiredService<ProveedorAutenticacionJWT>());
            services.AddScoped<ILoginService, ProveedorAutenticacionJWT>(
                provider => provider.GetRequiredService<ProveedorAutenticacionJWT>());

            services.AddScoped<TokenRenovator>();
            services.AddSingleton<ILocalizationManager,SavacFarmaLocManager>();

            
        }

        // based on https://github.com/pranavkm/LocSample
        private static async Task SetCultureAsync(WebAssemblyHost host)
        {
            var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
            var cultureName = await jsRuntime.InvokeAsync<string>("blazorCulture.get");

            if (cultureName != null)
            {
                var culture = new CultureInfo(cultureName);

                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;

            }
        }

       
    }
    
}
