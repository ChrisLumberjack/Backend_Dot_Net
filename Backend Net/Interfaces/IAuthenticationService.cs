namespace Backend_Net.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> GetGoogleClientIdAsync();
        Task<string> GetGoogleClientSecretAsync();
    }
}
