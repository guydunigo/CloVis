using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
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

        public FileAlreadyExistsException(string message) : base(message)
        { }
    }

    public enum RemoveOutput
    {
        NothingDone,
        RestoredToDefault,
        RemovedFromIndex
    }

    /// <summary>
    /// DataImages value is the file's name located
    /// </summary>
    public class DataImage : Data<string>, INotifyPropertyChanged
    {
        /// <summary>
        /// If you use this constructor, there must be an existing image named
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public DataImage(string name, bool isIndependant = false) : base(name, -1, "", true, false)
        {
            if (!isIndependant)
                Index.AddImage(this);
        }

        public DataImage(StorageFile source) : this(source.Name, source)
        { }

        public DataImage(string name, StorageFile source) :
            this(name)
        {
            ImportImage(source, name);
            // + with resume/template
        }

        public static async Task<BitmapSource> GetImageSource(string name, StorageFolder supplementaryFolder)
        {
            return await GetImageSource(name, new StorageFolder[] { supplementaryFolder });
        }
        public static async Task<BitmapSource> GetImageSource(string name, StorageFolder[] supplementaryFolders = null)
        {
            var img = new BitmapImage();

            StorageFile imgFile = await GetImageFileFromEverywhereOrDefault(name, supplementaryFolders);

            FileRandomAccessStream stream = (FileRandomAccessStream)(await imgFile.OpenAsync(FileAccessMode.Read));
            img.SetSource(stream);

            return img;
        }

        public static async Task<StorageFile> GetImageFileFromEverywhereOrDefault(string name, StorageFolder supplementaryFolder)
        {
            return await GetImageFileFromEverywhereOrDefault(name, new StorageFolder[] { supplementaryFolder });
        }
        public static async Task<StorageFile> GetImageFileFromEverywhereOrDefault(string name, StorageFolder[] supplementaryFolders = null)
        {
            StorageFile imgFile = await GetImageFileFromEverywhere(name, supplementaryFolders);

            if (imgFile == null)
                imgFile = await GetImageDefault();

            return imgFile;
        }

        public static async Task<StorageFile> GetImageFileFromEverywhere(string name, StorageFolder supplementaryFolder)
        {
            return await GetImageFileFromEverywhere(name, new StorageFolder[] { supplementaryFolder });
        }
        public static async Task<StorageFile> GetImageFileFromEverywhere(string name, StorageFolder[] supplementaryFolders = null)
        {
            var imgFolds = await GetImageFoldersList(supplementaryFolders);

            StorageFile imgFile = null;

            foreach (StorageFolder imgFold in imgFolds)
            {
                imgFile = await GetImageFileFrom(name, imgFold);

                if (imgFile != null)
                    break;
            }

            return imgFile;
        }

        public static async Task<StorageFile> GetImageFileFrom(string name, StorageFolder imgFold = null)
        {
            StorageFile imgFile = null;

            if (imgFold == null)
                imgFold = await GetLocalImageFolder();

            var nameCut = GetNameWithoutExtension(name);

            var files = await imgFold.GetFilesAsync();

            foreach (StorageFile f in files)
            {
                if (GetNameWithoutExtension(f.Name) == nameCut)
                {
                    imgFile = f;
                    break;
                }
            }

            return imgFile;
        }

        public async Task<StorageFile> GetImageFile()
        {
            return await GetImageFileFrom(value);
        }

        public async static Task<StorageFolder[]> GetImageFoldersList(StorageFolder supplementaryFolder)
        {
            return await GetImageFoldersList(new StorageFolder[] { supplementaryFolder });
        }

        public async static Task<StorageFolder[]> GetImageFoldersList(StorageFolder[] supplementaryFolders = null)
        {
            List<StorageFolder> folds = null;

            if (supplementaryFolders == null)
                folds = new List<StorageFolder>();
            else
                folds = new List<StorageFolder>(supplementaryFolders);

            folds.Add(await GetLocalImageFolder());
            folds.Add(await GetAppImageFolder());

            StorageFolder[] res = new StorageFolder[folds.Count];

            folds.CopyTo(res);

            return res;
        }

        public async static Task<StorageFolder> GetAppImageFolder()
        {
            StorageFolder appFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Resources");
            return await appFolder.GetFolderAsync("Images");
        }
        public async static Task<StorageFolder> GetLocalImageFolder()
        {
            var name = "Images";

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

        public static FileOpenPicker GetImagePicker()
        {
            FileOpenPicker openPicker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".bmp");

            return openPicker;
        }

        public async void ImportImage(StorageFile file, string desiredName)
        {
            var imgFold = await GetLocalImageFolder();
            StorageFile copiedFile = null;

            if (!await IsImageFilePresent(desiredName, imgFold))
            {
                copiedFile = await file.CopyAsync(imgFold, desiredName + file.FileType);
            }
            else
                throw new FileAlreadyExistsException("An image already exists with that name");

            value = GetNameWithoutExtension(copiedFile.Name);
        }

        public override Element Copy()
        {
            throw new NotImplementedException();
        }

        public override void UpdateFromIndex(NewIndex indexToUse = null)
        {
            throw new NotImplementedException();
        }

        public async Task ReplaceImageFile(StorageFile newFile)
        {
            var imgFile = await GetImageFile();
            // If the file was not found in the local folder, check app folder and copy it in the local folder :
            if (imgFile == null)
            {
                var appFold = await GetAppImageFolder();
                imgFile = await GetImageFileFrom(value, appFold);
                if (imgFile == null)
                    throw new FileNotFoundException("The current file was not defined and therefore cannot be replaced.");
                else
                {
                    var imgFold = await GetLocalImageFolder();
                    imgFile = await imgFile.CopyAsync(imgFold);
                }
            }

            if (imgFile != null)
            {
                var imgFold = await GetLocalImageFolder();
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
        }

        public async Task<RemoveOutput> Remove()
        {
            var res = RemoveOutput.NothingDone;

            var imgFile = await GetImageFile();
            if (imgFile != null)
            {
                await imgFile.DeleteAsync();
                res = RemoveOutput.RestoredToDefault;
            }

            // If it is not a default picture
            if ((await GetImageFileFrom(value, await GetAppImageFolder())) == null)
            {
                Index.Images.Remove(this);
                res = RemoveOutput.RemovedFromIndex;
            }

            return res;
        }

        public static async Task<bool> IsImageFilePresent(string name, StorageFolder sf = null)
        {
            if (sf == null)
                sf = await GetLocalImageFolder();

            var nameCut = GetNameWithoutExtension(name);

            var files = await sf.GetFilesAsync();
            var res = false;

            foreach (StorageFile f in files)
            {
                if (GetNameWithoutExtension(f.Name) == nameCut)
                {
                    res = true;
                    break;
                }
            }

            return res;
        }

        public static string GetNameWithoutExtension(string name)
        {
            var ext = GetExtension(name);
            return name.Replace("." + ext, "").ToLower();
        }
        public static string GetExtension(string name)
        {
            var temp = name.Split('.');
            return temp[temp.Length - 1];
        }
    }
}
