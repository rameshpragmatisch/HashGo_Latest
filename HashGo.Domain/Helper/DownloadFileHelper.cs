namespace HashGo.Domain.Helper
{
    public static class DownloadFileHelper
    {
        private static HttpClient client = new HttpClient();

        public static async Task<string> DownloadFileAsync(string sourceFilePath, string localFilePath)
        {
            var uri = new Uri(sourceFilePath);
            var result = await client.GetAsync(uri);
            using (var fs = new FileStream(localFilePath, FileMode.CreateNew))
            {
                await result.Content.CopyToAsync(fs);

                return localFilePath;
            }
        }
    }
}
