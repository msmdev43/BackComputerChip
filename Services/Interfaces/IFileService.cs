using Microsoft.AspNetCore.Http;

namespace computerChip.Services.Interfaces
{
    public interface IFileService
    {
        public interface IFileService
        {
            Task<string> SaveFileAsync(IFormFile file, string folder);
            Task DeleteFileAsync(string fileName, string folder);
        }
    }
}