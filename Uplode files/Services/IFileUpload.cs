using Microsoft.AspNetCore.Components.Forms;

namespace Uplode_files.Services
{
    public interface IFileUpload
    {
        Task UploadFile(IBrowserFile browserFile);
        Task<string> GenratePreviewURL(IBrowserFile file);
    }
}
