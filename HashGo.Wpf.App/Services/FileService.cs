using System.IO;
using System.Text;
using System.Text.Json;
using HashGo.Core.Contracts.Services;

namespace HashGo.Wpf.App.Services
{

    public class FileService : IFileService
    {
        public T Read<T>(string folderPath, string fileName)
        {
            var path = Path.Combine(folderPath, fileName);
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<T>(json);
            }

            return default;
        }

        public void Save<T>(string folderPath, string fileName, T content)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileContent = JsonSerializer.Serialize(content);
            File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
        }

        public void Delete(string folderPath, string fileName)
        {
            if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
            {
                File.Delete(Path.Combine(folderPath, fileName));
            }
        }

        //public Task<T> ReadAsync<T>(string folderPath, string fileName)
        //{
        //    TaskFactory.StartNew<T>(Read, folderPath, fileName);
        //}

        //public Task SaveAsync<T>(string folderPath, string fileName, T content)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task DeleteAsync(string folderPath, string fileName)
        //{
        //    throw new NotImplementedException();
        //}
    }
}