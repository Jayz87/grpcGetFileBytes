
namespace FileBytes.Service
{
    public interface IGetFileClientService
    {
        Task<List<byte>> GetFileAsync(string fileName);
    }
}
