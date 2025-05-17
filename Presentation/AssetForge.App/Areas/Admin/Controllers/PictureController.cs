using AssetForge.Core.Infrastructure;
using AssetForge.Services.Helpers;
using AssetForge.Services.Media;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.App.Areas.Admin.Controllers
{
    public partial class PictureController : BaseAdminController
    {
        #region Fields

        private readonly IPictureService _pictureService;
        private readonly IAssetForgeFileProvider _fileProvider;

        #endregion

        #region Ctor

        public PictureController(IPictureService pictureService,
            IAssetForgeFileProvider fileProvider)
        {
            _pictureService = pictureService;
            _fileProvider = fileProvider;
        }

        #endregion

        #region Methods

        [HttpPost]
        //do not validate request token (XSRF)
        [IgnoreAntiforgeryToken]
        public virtual async Task<IActionResult> AsyncUpload()
        {
            //if (!await _permissionService.Authorize(StandardPermissionProvider.UploadPictures))
            //    return Json(new { success = false, error = "You do not have required permissions" }, "text/plain");

            var httpPostedFile = Request.Form.Files.FirstOrDefault();
            if (httpPostedFile == null)
                return Json(new { success = false, message = "No file uploaded" });

            const string qqFileNameParameter = "qqfilename";
            FileProcessingHelper.CheckAndCreateDirectory(_fileProvider.GetAbsolutePath(MediaDefaults.ImageThumbsPath), _fileProvider);

            var qqFileName = Request.Form.ContainsKey(qqFileNameParameter)
                ? Request.Form[qqFileNameParameter].ToString()
                : string.Empty;

            var picture = await _pictureService.InsertPictureAsync(httpPostedFile, qqFileName);

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.

            if (picture == null)
                return Json(new { success = false, message = "Wrong file format" });

            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                imageUrl = (await _pictureService.GetPictureUrlAsync(picture, 100)).Url
            });
        }

        #endregion
    }
}