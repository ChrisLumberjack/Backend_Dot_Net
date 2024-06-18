using Backend_Net.Interfaces;
using Backend_Net.Services;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using System.Text.Json;



IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();


string type = Environment.GetEnvironmentVariable("TYPE");
string projectId = Environment.GetEnvironmentVariable("PROJECT_ID");
string privateKeyId = Environment.GetEnvironmentVariable("PRIVATE_KEY_ID");
string privateKey = Environment.GetEnvironmentVariable("PRIVATE_KEY");
string clientEmail = Environment.GetEnvironmentVariable("CLIENT_EMAIL");
string clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
string authUri = Environment.GetEnvironmentVariable("AUTH_URI");
string tokenUri = Environment.GetEnvironmentVariable("TOKEN_URI");
string authProviderCertUrl = Environment.GetEnvironmentVariable("AUTH_PROVIDER_CERT_URL");
string clientCertUrl = Environment.GetEnvironmentVariable("CLIENT_CERT_URL");
string universeDomain = Environment.GetEnvironmentVariable("UNIVERSE_DOMAIN");
string GOOGLE_APPLICATION = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION");
object firebaseConfig = new
{
    type,
    project_id = projectId,
    private_key_id = privateKeyId,
    private_key = privateKey.Replace("\\n", "\n"),
    client_email = clientEmail,
    client_id = clientId,
    auth_uri = authUri,
    token_uri = tokenUri,
    auth_provider_x509_cert_url = authProviderCertUrl,
    client_x509_cert_url = clientCertUrl,
    universe_domain = universeDomain
};

string json = JsonSerializer.Serialize(firebaseConfig);


string tempFilePath = Path.GetTempFileName();
string tempJsonFilePath = Path.ChangeExtension(tempFilePath, ".json");

// Escribir el contenido JSON en el archivo temporal
File.WriteAllText(tempJsonFilePath, GOOGLE_APPLICATION);

// Establecer la variable de entorno GOOGLE_APPLICATION_CREDENTIALS con la ruta del archivo temporal
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempJsonFilePath);

// Ejemplo de lectura de la variable de entorno configurada
string GOOGLE_APPLICATION_CREDENTIALS = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");


GoogleCredential credential;
using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
{
    credential = GoogleCredential.FromStream(stream);
}
FirebaseApp.Create(new AppOptions()
{
    Credential = credential,
    ProjectId = projectId,
});

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IFirestoreService, FirestoreService>();

builder.Services.AddSingleton<FirestoreDb>(provider =>
{
    var projectId = Environment.GetEnvironmentVariable("PROJECT_ID");
    return FirestoreDb.Create(projectId);
});

builder.Services.AddSingleton<FirebaseAuth>(provider =>
{
    return FirebaseAuth.DefaultInstance;
});
builder.Services.AddRazorPages();


var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Program>();
                webBuilder.UseUrls("https://localhost:7298", "http://localhost:5007");
            });

app.Run();
