using System.Globalization;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Models;
using HashGo.Infrastructure;
using HashGo.Infrastructure.Common;
using HashGo.Infrastructure.Setting;
using Newtonsoft.Json;

namespace HashGo.Wpf.App.Converters
{
    public class ImageValueConverter : MarkupExtension, IValueConverter
    {
        private readonly ILoggingService _logger;

        public static ImageValueConverter Instance = new ImageValueConverter();

        public ImageValueConverter(ILoggingService loggingService) 
        {
            _logger = loggingService;
        }

        public ImageValueConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null &&
                value == DependencyProperty.UnsetValue)
            {
                return null;
            }

            if (value is string stringValue)
            {
                if (System.IO.Path.Exists(stringValue))
                {
                    return stringValue;
                }
            }

            if (value is string files && !string.IsNullOrEmpty(files) && !files.ToUpper().Equals("NULL"))
            {
                List<ImageFile> imageFiles = new List<ImageFile>();
                try
                {
                    var v = JsonConvert.DeserializeObject<object>(files);
                    imageFiles = JsonConvert.DeserializeObject<List<ImageFile>>(files);
                }
                catch(Exception ex)
                {
                    _logger.TraceException(ex);
                }

                if (imageFiles.Count == 0)
                {
                    return null;
                }
                else
                {
                    if (imageFiles[0].fileName.Contains("gif", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var fileFullName = $"{LocalSetting.ImagesPath}\\{imageFiles[0].fileName}";
                        if (!File.Exists(fileFullName))
                        {
                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFile(new Uri(imageFiles[0].fileSystemName), fileFullName);
                            }
                        }
                        return fileFullName;
                    }

                    Task<string> task = Task.Run(() =>
                    {
                        if (imageFiles.Count > 0)
                        {
                            var fileFullName = $"{LocalSetting.ImagesPath}\\{imageFiles[0].fileName}";
                            if (!File.Exists(fileFullName))
                            {
                                using (WebClient client = new WebClient())
                                {
                                    client.DownloadFile(new Uri(imageFiles[0].fileSystemName), fileFullName);
                                }
                            }

                            return fileFullName;
                        }

                        return null;
                    });

                    return new TaskCompletionNotifier<string>(task);
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class HashTechImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           if(value is string imgPath)
            {
                if (imgPath.StartsWith(@"pack://application"))
                    return imgPath;
                else
                {
                    var arr = imgPath.Split('\\');
                    string fileName = arr[arr.Length - 1];
                    var fileFullName = $"{LocalSetting.ImagesPath}\\{fileName}";
                    if (!File.Exists(fileFullName))
                    {
                        using (WebClient client = new WebClient())
                        {
                            string url = HashGoAppSettings.Url + imgPath;
                            url = url.Replace("\\", "//");
                            client.DownloadFile(url, fileFullName);
                        }
                    }

                    return fileFullName;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
