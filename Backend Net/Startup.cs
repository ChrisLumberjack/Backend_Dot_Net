using Backend_Net.Interfaces;
using Backend_Net.Services;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configuración de Firebase Admin SDK
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.GetApplicationDefault(),
            ProjectId = "backendnet-c442c",
        });

        services.AddRazorPages();

        // Registro de FirestoreDb como servicio singleton
        services.AddSingleton<FirestoreDb>(provider =>
        {
            var projectId = Configuration["Firebase:backendnet-c442c"];
            return FirestoreDb.Create(projectId);
        });

        // Configuración de Firebase Authentication (FirebaseAuth)
        services.AddSingleton<FirebaseAuth>(provider =>
        {
            return FirebaseAuth.DefaultInstance;
        });

        // Configuración de autenticación
        services.AddAuthentication(o =>
        {
            o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddGoogle(options =>
        {
            options.ClientId = Configuration["Google:296296323514-drk0cmrri57c7r5mgr7dshrajtg1mvlu.apps.googleusercontent.com"];
            options.ClientSecret = Configuration["Google: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"];
            options.CallbackPath = "/signin-google";
        });

        // Otros servicios y configuraciones aquí
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configuración de middleware y enrutamiento
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
        });
    }
}