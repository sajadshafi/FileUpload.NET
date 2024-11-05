using Microsoft.AspNetCore.Http;

namespace FileUpload.Application.Contracts.Files;

public sealed record FileUploadRequest(IFormFile File);