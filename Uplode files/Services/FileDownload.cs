using Microsoft.AspNetCore.StaticFiles;
using Microsoft.JSInterop;

namespace Uplode_files.Services
{
    public class FileDownload : IFileDownload
    {
        private IWebHostEnvironment _webHostEnvironment;

        

        private readonly IJSRuntime _jSRuntime;



        public FileDownload(IWebHostEnvironment webHostEnvironment, IJSRuntime jSRuntime)
        {
            _webHostEnvironment = webHostEnvironment;
            
            _jSRuntime = jSRuntime;
        }

        public async Task DownloadFile(string Url)
        {
            await _jSRuntime.InvokeVoidAsync("downloadFile", Url);
        }

        public async Task<List<string>> GetUploadFiles()
        {
            var base64URLs = new List<string>();
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploades");
            var files = Directory.GetFiles(uploadPath);

            if(files is not null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    using (var fileInput = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        var memoStream = new MemoryStream();

                        await fileInput.CopyToAsync(memoStream);
                        var buffer = memoStream.ToArray();
                        var fileType = GetMimeType(file);
                        base64URLs.Add($"data:{fileType};base64,{Convert.ToBase64String(buffer)}");
                    }
                }
                
            }
            return base64URLs;
        }

        private string GetMimeType(string file)
        {
            const string Detaulf = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(file,out string contenttype))
            {
                contenttype = Detaulf;
            }
            return contenttype;
        }
    }
}
