using HashGo.Core.Models;
using Newtonsoft.Json;

namespace HashGo.Domain.Helper
{
    public static class JsonImageSerializerHelper
    {
        public static async Task<string> GetImageToLocalStorage(string jsonString, string basePath)
        {
            if (jsonString is string files && !string.IsNullOrEmpty(files))
            {
                List<ImageFile> imageFiles = JsonConvert.DeserializeObject<List<ImageFile>>(files);

                if (imageFiles?.Any() != true)
                {
                    return string.Empty;
                }


                var firstImage = imageFiles[0].fileName;
                var sourFilePath = imageFiles[0].fileSystemName;

                return await GetFile(basePath, firstImage, sourFilePath);
            }

            return string.Empty;
        }

        private static async Task<string> GetFile(string targetBasePath, string targetImagefileName, string sourFilePath)
        {
            if (!string.IsNullOrEmpty(targetImagefileName) &&
                !string.IsNullOrEmpty(sourFilePath))
            {
                targetImagefileName = targetImagefileName.ToLowerInvariant();

                var fileFullName = System.IO.Path.Combine(targetBasePath, targetImagefileName);

                if (!File.Exists(fileFullName))
                {
                    return await DownloadFileHelper.DownloadFileAsync(sourFilePath, fileFullName);
                }

                return fileFullName;
            }

            return string.Empty;
        }
    }
}
