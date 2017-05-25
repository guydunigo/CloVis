using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ResumeElements
{
    /// <summary>
    /// DataImages value is the file's name located
    /// </summary>
    public class DataImage:Data<string>, INotifyPropertyChanged
    {
        public DataImage(string name, string description = "") : base(name, -1, description, true, false)
        {

        }

        protected BitmapImage image;
        public BitmapImage Image
        {
            get => image;
            set
            {
                ChangeImage(value);
            }
        }

        public async void ChangeImage(BitmapImage img)
        {
            var imgFold = await GetImageFolder();
            var temp = await imgFold.GetFileAsync(value);

            img = new BitmapImage();

            image = img;
        }

        public async static Task<StorageFolder> GetImageFolder()
        {
            //var bas = new Uri("ms-appdata:///local/images");
            var name = "images";

            StorageFolder folder;
            try
            {
                folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(name);
            }
            catch (FileNotFoundException)
            {
                folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(name);
            }
            return folder;
        }
    }
}
