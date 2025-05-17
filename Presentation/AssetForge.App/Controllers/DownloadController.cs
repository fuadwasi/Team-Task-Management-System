using AssetForge.Core.Infrastructure;
using AssetForge.Core;
using AssetForge.Services.Media;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.App.Controllers
{
    public class DownloadController : BasePublicController
    {
        #region Fields

        private readonly IDownloadService _downloadService;
        private readonly IAssetForgeFileProvider _fileProvider;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public DownloadController(IDownloadService downloadService,
            IAssetForgeFileProvider fileProvider,
            IWorkContext workContext)
        {
            _downloadService = downloadService;
            _fileProvider = fileProvider;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        public virtual async Task<IActionResult> DownloadFile(int id)
        {
            var download = await _downloadService.GetDownloadByIdAsync(id);
            if (download == null)
                return Content("No download record found with the specified id");

            //A warning (SCS0027 - Open Redirect) from the "Security Code Scan" analyzer may appear at this point. 
            //In this case, it is not relevant. Url may not be local.
            if (download.UseDownloadUrl)
                return new RedirectResult(download.DownloadUrl);

            //use stored data
            if (download.DownloadBinary == null)
                return Content($"Download data is not available any more. Download GD={download.Id}");

            var fileName = !string.IsNullOrWhiteSpace(download.Filename) ? download.Filename : download.Id.ToString();
            var contentType = !string.IsNullOrWhiteSpace(download.ContentType)
                ? download.ContentType
                : MimeTypes.ApplicationOctetStream;
            return new FileContentResult(download.DownloadBinary, contentType)
            {
                FileDownloadName = fileName + download.Extension
            };
        }

        #endregion
    }
}
