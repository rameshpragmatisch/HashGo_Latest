namespace HashGo.Core.Contracts.Services
{

    public interface IFileService : IApplicationService
    {
        T Read<T>(string folderPath, string fileName);

        void Save<T>(string folderPath, string fileName, T content);

        void Delete(string folderPath, string fileName);

        //Task<T> ReadAsync<T>(string folderPath, string fileName);

        //Task SaveAsync<T>(string folderPath, string fileName, T content);

        //Task DeleteAsync(string folderPath, string fileName);
    }
}