using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ResumeElements
{
    public class FileAlreadyExistsException : Exception
    {
        public FileAlreadyExistsException() : base()
        { }

        public FileAlreadyExistsException(string message):base(message)
        { }
    }

    /// <summary>
    /// DataImages value is the file's name located
    /// </summary>
    public class DataImage:Data<string>, INotifyPropertyChanged
    {
        /// <summary>
        /// If you use this constructor, there must be an existing image named
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public DataImage(string name):base(name, -1, "", true, false)
        {
            LoadImage();
            Index.AddImage(this);
        }

        public DataImage(string name, StorageFile source) :
            base(name, -1, "", true, false)
        {
            var temp = source.Name.Split('.');

            ImportImage(source, name + "." + temp[temp.Length-1]);
            Index.AddImage(this);
            // + with resume/template
        }

        protected BitmapImage image;
        public BitmapImage Image
        {
            get => image;
        }

        public override string Value
        {
            get => value;
            set
            {

            }
        }

        public async void LoadImage()
        {
            var imgFile = await GetImageFile();
            if (imgFile != null)
            {
                var imgFold = await GetImageFolder();

                image = new BitmapImage();
                FileRandomAccessStream stream = (FileRandomAccessStream)(await imgFile.OpenAsync(FileAccessMode.Read));
                image.SetSource(stream);

                NotifyPropertyChanged("Image");
            }
            else
                throw new FileNotFoundException("The current file was not defined and therefore cannot be loaded.");
        }

        public async Task<StorageFile> GetImageFile()
        {
            var imgFold = await GetImageFolder();
            var imfFile = await imgFold.GetFileAsync(value);

            return imfFile;
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
        public async static Task<StorageFile> GetImageDefault()
        {
            return await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Assets/StoreLogo.png"));
        }

        public async void ImportImage(StorageFile file, string desiredName)
        {
            var imgFold = await GetImageFolder();
            StorageFile copiedFile = null;

            if (await imgFold.TryGetItemAsync(desiredName) == null)
                copiedFile = await file.CopyAsync(imgFold, desiredName);
            else
                throw new FileAlreadyExistsException("An image already exists with that name");

            value = copiedFile.Name;
            LoadImage();
        }

        public override Element Copy()
        {
            throw new NotImplementedException();
        }

        public override void UpdateFromIndex()
        {
            throw new NotImplementedException();
        }

        public async void RenameImageFile(string newName)
        {
            var imgFile = await GetImageFile();
            if (newName != "" && (imgFile != null))
            {
                var imgFold = await GetImageFolder();
                if (await imgFold.TryGetItemAsync(newName) == null)
                {
                    await imgFile.RenameAsync(newName);
                    value = newName;
                }
                else
                    throw new FileAlreadyExistsException("An image already exists with that name");
            }
            else
                throw new FileNotFoundException("The current file was not defined and therefore cannot be renamed.");
        }

        public async void ReplaceImageFile(StorageFile newFile)
        {
            var imgFile = await GetImageFile();
            if (imgFile != null)
            {
                var imgFold = await GetImageFolder();
                var tempFold = ApplicationData.Current.TemporaryFolder;
                if (newFile != null)
                {
                    // Ensure the previous file will be kept if there is an error importing the file
                    await imgFile.MoveAsync(tempFold);

                    try
                    {
                        ImportImage(newFile, value);
                    }
                    catch (Exception e)
                    {
                        await imgFile.MoveAsync(imgFold, value, NameCollisionOption.ReplaceExisting);

                        throw e;
                    }

                    await imgFile.DeleteAsync();
                }
            }
            else
                throw new FileNotFoundException("The current file was not defined and therefore cannot be replaced.");
        }

        public async void Remove()
        {
            var imgFile = await GetImageFile();
            if (imgFile != null)
            {
                await imgFile.DeleteAsync();
                Index.Images.Remove(this);
            }
        }

        public static async Task<bool> IsImageFilePresent(string name)
        {
            var imgFold = await GetImageFolder();

            return (await imgFold.TryGetItemAsync(name) != null);
        }
    }
}
