using FileUpload.Application.Contracts.Files;
using FileUpload.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FileUpload.Application.Concrete;

public sealed class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _env;

    public FileUploadService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<FileUploadResponse> UploadFileAsync(FileUploadRequest request)
    {
        string folderPath = Path.Combine(_env.WebRootPath, "uploads");
        
        // create the directory if it does not exist
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // extract the file extension for some specific purpose - like validation
        string extension = Path.GetExtension(request.File.FileName);
        
        // You can add validation here to allow only specific files using the extension extracted - 
        if (!IsValidImageFile(extension))
        {
            return new FileUploadResponse(string.Empty);
        }

        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(request.File.FileName);
        
        // I am concatenating a new guid to the end of the filename so that if file with same name is uploaded twice that is handled correctly
        string fileName = $"{fileNameWithoutExtension}_{Guid.NewGuid()}{extension}";
        string filePath = Path.Combine(folderPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await request.File.CopyToAsync(stream);

        return new FileUploadResponse($"/uploads/{fileName}");
    }

    public bool DeleteFile(DeleteFileRequest request)
    {
        string folderPath = Path.Combine(_env.WebRootPath, "uploads");
        var filePath = Path.Combine(folderPath, request.FileName);

        if (!File.Exists(filePath))
        {
            return false;
        }

        try
        {
            File.Delete(filePath);
            return true; // 204 No Content
        }
        catch (IOException ex)
        {
            // handle exception - Logging, etc
            return false;
        }
    }

    
    // This is an example validation code in large projects the validation will depend on the requirement but the concept will be same.
    private bool IsValidImageFile(string extension)
    {
        string[] imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        return imageExtensions.Contains(extension);
    }
}