namespace Uplode_files.Services
{
    public interface IFileDownload
    {
        Task<List<String>> GetUploadFiles();
        Task DownloadFile(string Url);
    }
}
