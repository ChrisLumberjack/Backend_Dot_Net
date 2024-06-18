using Backend_Net.Interfaces;
using Google.Cloud.Firestore;

namespace Backend_Net.Services
{

    public class FirestoreService : IFirestoreService

    {

        // Agrega este bloque al principio de tu archivo AuthController.cs
        public class VerifyTokenRequest
        {
            public string IdToken { get; set; }
        }

        private readonly FirestoreDb _db;
        private readonly string _name = Environment.GetEnvironmentVariable("PROJECT_ID");

        public FirestoreService()
        {
            _db = FirestoreDb.Create(_name);
        }

        public async Task AddUserAsync(string collectionName, Dictionary<string, object> userData)
        {
            DocumentReference docRef;


            docRef = _db.Collection(collectionName).Document();


            await docRef.SetAsync(userData);
        }

        public async Task<object> ReadUserAsync(string collectionName, string documentId)
        {
            DocumentReference docRef = _db.Collection(collectionName).Document(documentId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                // Leer los datos del documento y devolverlos
                return snapshot.ToDictionary();
            }
            else
            {
                // Manejar el caso en el que el documento no existe, por ejemplo, lanzar una excepción o devolver null
                throw new Exception($"El documento con ID {documentId} en la colección {collectionName} no existe.");
            }
        }

        public async Task DeleteUserAsync(string collectionName, string documentId)
        {
            DocumentReference docRef = _db.Collection(collectionName).Document(documentId);
            await docRef.DeleteAsync();
        }

        public async Task UpdateUserAsync(string collectionName, string documentId, object updatedData)
        {
            DocumentReference docRef = _db.Collection(collectionName).Document(documentId);
            await docRef.SetAsync(updatedData);
        }

    }
}
