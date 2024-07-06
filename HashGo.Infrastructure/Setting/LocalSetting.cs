using NLog.Layouts;

namespace HashGo.Infrastructure.Setting
{
    public class LocalSetting
    {
        public static string DocumentPath =>
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + AppName;

        public static string AppName => "HashGo";
        public static string LogPath => DocumentPath +"\\Logs";
        public static string DbPath => DocumentPath +"\\Database";
        public static string ImagesPath => DocumentPath +"\\Images";

        static LocalSetting()
        {
            if (!Directory.Exists(DocumentPath))
                Directory.CreateDirectory(DocumentPath);

            if (!Directory.Exists(LogPath))
                Directory.CreateDirectory(LogPath);

            if (!Directory.Exists(DbPath))
                Directory.CreateDirectory(DbPath);

            if (!Directory.Exists(ImagesPath))
                Directory.CreateDirectory(ImagesPath);
        }

    }
}
