namespace Backend_Net.Interfaces
{
    
    public interface IFirestoreService
    {
        Task AddUserAsync(string collectionName, Dictionary<string,object> userData);
        Task<object> ReadUserAsync(string collectionName, string documentId);
        Task DeleteUserAsync(string collectionName, string documentId);
        Task UpdateUserAsync(string collectionName, string documentId, object updatedData);


    }
}
