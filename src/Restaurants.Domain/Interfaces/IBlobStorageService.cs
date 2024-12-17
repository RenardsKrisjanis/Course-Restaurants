namespace Restaurants.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadToBlobAsync(Stream data, string fileName);
    string GetBlobSasUrl(string? blobUrl);
    string GetBlobNameFromUrl(string blobUrl);
}
