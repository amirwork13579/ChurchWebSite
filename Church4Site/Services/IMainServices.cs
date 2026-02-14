using Church4Site.Entities;
namespace Church4Site.Services

{
    public interface IMainServices
    {
        Task<string> CreateImageAsync(IFormFile imageFile, string location);
        void DeleteServerPhoto(string path);
    }
}
