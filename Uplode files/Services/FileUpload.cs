using Microsoft.AspNetCore.Components.Forms;
 
namespace Uplode_files.Services
{
    public class FileUpload : IFileUpload
    {
        private IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileUpload> _logger;

        public FileUpload(IWebHostEnvironment webHostEnvironment, ILogger<FileUpload> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
        public async Task UploadFile(IBrowserFile browserFile)
        {
            if (browserFile is not null)
            {
                try
                {
                    long MaxSize = 524288000;
                    // Console.WriteLine(_webHostEnvironment.WebRootPath);
                    var uploadePathe = Path.Combine(_webHostEnvironment.WebRootPath, "Uploades", browserFile.Name);
                    var test = Path.Combine(_webHostEnvironment.WebRootPath, "Uploades");
                    _logger.LogInformation(Path.GetDirectoryName(test));
                    using (var stream = browserFile.OpenReadStream(MaxSize))
                    {
                        var fileStream = File.Create(uploadePathe);
                        
                        await stream.CopyToAsync(fileStream);

                        fileStream.Close();
                        
                    }
                }
                catch (Exception ex)
                {
                    
                    _logger.LogError(ex.ToString());
                }
            }
        }

        public async Task<string> GenratePreviewURL(IBrowserFile file)
        {
            //Console.WriteLine(file.ContentType);
            if (!file.ContentType.Contains("image")){
                if(file.ContentType.Contains("pdf"))
                {
                    return "serverIcons/pdf-svgrepo-com.svg";
                }
                else
                {
                    return "serverIcons/blank-file-svgrepo-com.svg";
                }
            }
            var resized = await file.RequestImageFileAsync(file.ContentType, 100, 100);
            
            var buffer = new byte[resized.Size];
            
            await resized.OpenReadStream().ReadAsync(buffer);
            
            return $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";
        }
    }
}
