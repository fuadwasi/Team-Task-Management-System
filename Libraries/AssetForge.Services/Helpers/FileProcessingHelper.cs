using AssetForge.Core;
using AssetForge.Core.Domain.Media;
using AssetForge.Core.Infrastructure;
using AssetForge.Services.Common;
using Microsoft.AspNetCore.Http;

namespace AssetForge.Services.Helpers
{
    public static class FileProcessingHelper
    {
        private static readonly string[] _permittedExtensions = { ".png", ".jpg", ".jpeg" };

        public static bool ValidateFile(IFormFile file, int maxFileSize, bool considerFileExtension, FileType fileType, out string errorMessage)
        {
            errorMessage = string.Empty;
            var maxFileSizeInBytes = maxFileSize * 1024;

            if (file == null || file.Length == 0)
            {
                errorMessage = "No file uploaded.";
                return false;
            }

            if (file.Length > maxFileSizeInBytes)
            {
                errorMessage = $"File size exceeds {maxFileSize} kilobytes.";
                return false;
            }

            if (considerFileExtension)
            {
                string[] permittedExtensions;

                // Switch based on FileType to define permitted extensions
                switch (fileType)
                {
                    case FileType.Image:
                        permittedExtensions = new[] { ".png", ".jpg", ".jpeg", ".gif" };
                        break;
                    case FileType.Pdf:
                        permittedExtensions = new[] { ".pdf" };
                        break;
                    case FileType.Document:
                        permittedExtensions = new[] { ".doc", ".docx", ".txt" };
                        break;
                    default:
                        errorMessage = "Unknown file type.";
                        return false;
                }
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(fileExtension) || !_permittedExtensions.Contains(fileExtension))
                {
                    errorMessage = $"Invalid file type. Only {string.Join(", ", permittedExtensions)} are allowed.";
                    return false;
                }
            }

            return true;
        }

        public static bool CheckAndCreateDirectory(string directoryPath, IAssetForgeFileProvider fileProvider)
        {
            if (string.IsNullOrEmpty(directoryPath))
                return false;

            var dir = fileProvider.MapPath(directoryPath);

            if (!fileProvider.DirectoryExists(dir))
                fileProvider.CreateDirectory(dir);

            return true;
        }

        public static string GetContentFileUrl(string directoryPath, IWebHelper webHelper)
        {
            var filePath = CommonDefaults.ContentFilePath.Replace("~/wwwroot/", "") + "/" + directoryPath;
            return $"{webHelper.GetSiteLocation()}{filePath}";
        }
    }
}
