using FileUpload.Application.Contracts.Files;
using FileUpload.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileUpload.API.Controllers;

[ApiController]
[Route("api/files")]
public class FileUploadController : ControllerBase
{
    private readonly IFileUploadService _fileUploadService;

    public FileUploadController(IFileUploadService fileUploadService)
    {
        _fileUploadService = fileUploadService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        var result = await _fileUploadService.UploadFileAsync(new FileUploadRequest(file));
        return Ok(result);
    }

    [HttpDelete]
    public IActionResult DeleteFile(string fileName)
    {
        var result = _fileUploadService.DeleteFile(new DeleteFileRequest(fileName));
        return Ok(result);
    }
}