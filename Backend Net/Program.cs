using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
//prueba de git
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Configuración de Firebase Admin
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.GetApplicationDefault(),
    ProjectId = "backendnet-c442c",
});

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddRazorPages();

// Registrar servicios Firestore y otros servicios necesarios
builder.Services.AddSingleton<FirestoreDb>(provider =>
{
    var projectId = configuration["backendnet-c442c"];
    return FirestoreDb.Create(projectId);
});

// Configurar Firebase Authentication
builder.Services.AddSingleton<FirebaseAuth>(provider =>
{
    return FirebaseAuth.DefaultInstance;
});

var app = builder.Build();

// Middleware y configuraciones de la aplicación
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Endpoints
app.MapRazorPages();
app.MapControllers();

// Ejemplo de endpoint con Firebase Firestore
/*app.MapGet("/users", async (FirestoreDb db) =>
{
    var usersCollection = db.Collection("users");
    var query = await usersCollection.GetSnapshotAsync();
    var users = query.Documents.Select(doc => doc.ConvertTo<User()).ToList();

    return Results.Ok(users);
});*/

app.Run();
