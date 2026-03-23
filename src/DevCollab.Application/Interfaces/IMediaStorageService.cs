namespace DevCollab.Application.Interfaces;

public interface IMediaStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
    Task<string> GetUrlAsync(string fileName, CancellationToken cancellationToken = default);
    Task DeleteAsync(string fileName, CancellationToken cancellationToken = default);
}
