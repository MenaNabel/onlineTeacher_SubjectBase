using Microsoft.AspNetCore.Http;

namespace OnlineTeacher.Shared.Interfaces
{
    public interface IFileImageUploading
    {
        bool UploadPhoto(IFileImage FileOrImage, out string FileOrImagePath);
        bool UploadPhoto(IFormFile FileOrImage, out string FileOrImagePath);
        bool UploadFile(IFileImage File, out string FileName, out byte[] FileData);
    }
}