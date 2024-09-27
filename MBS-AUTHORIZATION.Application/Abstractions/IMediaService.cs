using Microsoft.AspNetCore.Http;

namespace MBS_AUTHORIZATION.Application.Abstractions;

public interface IMediaService
{
    Task<string> UploadImageAsync(IFormFile file);
}