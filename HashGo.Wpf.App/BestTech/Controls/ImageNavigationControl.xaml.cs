using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HashGo.Wpf.App.BestTech.Controls
{
    /// <summary>
    /// Interaction logic for ImageNavigationControl.xaml
    /// </summary>
    public partial class ImageNavigationControl : UserControl, INotifyPropertyChanged
    {
        public ImageNavigationControl()
        {
            InitializeComponent();
        }

        #region Proerties

        int currentImageIndex = 0;

        #endregion

        public List<string> ImageSources
        {
            get { return (List<string>)GetValue(ImageSourcesProperty); }
            set { SetValue(ImageSourcesProperty, value); }
        }

        public string CurrentImageSource
        {
            get => currentImageSource;
            set
            {
                currentImageSource = value;
                OnPropertyChanged();
            }
        }

        string currentImageSource;

        // Using a DependencyProperty as the backing store for ImageSources.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourcesProperty =
            DependencyProperty.Register("ImageSources", typeof(List<string>), typeof(ImageNavigationControl), new PropertyMetadata(null, OnSetImageSources));

        private static void OnSetImageSources(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageNavigationControl imageNavigationControl &&
                imageNavigationControl.ImageSources?.Count > 0)
            {
                imageNavigationControl.nextNavigateButton.IsEnabled = true;
                imageNavigationControl.CurrentImageSource = imageNavigationControl.ImageSources.First();
            }
        }

        private void BackButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (currentImageIndex > 0) currentImageIndex--;
            CurrentImageSource = ImageSources[currentImageIndex];

            if (currentImageIndex > 0)
                nextNavigateButton.IsEnabled = true;

            if (currentImageIndex == 0) backNavigateButton.IsEnabled = false;
        }

        private void NextButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (currentImageIndex < ImageSources?.Count - 1)
                currentImageIndex++;

            CurrentImageSource = ImageSources[currentImageIndex];

            if (currentImageIndex > 0)
                backNavigateButton.IsEnabled = true;

            if (currentImageIndex == ImageSources.Count - 1)
                nextNavigateButton.IsEnabled = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
