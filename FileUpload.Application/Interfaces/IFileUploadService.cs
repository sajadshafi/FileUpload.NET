using FileUpload.Application.Contracts.Files;

namespace FileUpload.Application.Interfaces;

public interface IFileUploadService
{
    Task<FileUploadResponse> UploadFileAsync(FileUploadRequest request);
    bool DeleteFile(DeleteFileRequest request);
}