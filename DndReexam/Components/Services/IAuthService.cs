namespace DndReexam.Services
{
    public interface IAuthService
    {
        Task LoginAsync(string username, string password);
    }
}
