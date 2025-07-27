public interface IBlobStorageService
{
    Task<string> UploadFileAsync(Stream stream, string fileName, string containerName);
    Task<Stream> DownloadFileAsync(string fileName, string containerName);
}