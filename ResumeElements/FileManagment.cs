using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ResumeElements.FileManagment
{
    public class FileAlreadyExistsException : Exception
    {
        public FileAlreadyExistsException() : base()
        { }

        public FileAlreadyExistsException(string message) : base(message)
        { }
    }

    public enum ImageRemovedOutput
    {
        NothingDone,
        RestoredToDefault,
        RemovedFromIndex
    }

    public static class Images
    {
        /// <summary>
        /// Use to get the parameter of an <Image Source=""/> tag.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="supplementaryFolder"></param>
        /// <returns></returns>
        public static async Task<BitmapSource> GetImageSourceAsync(string name, StorageFolder supplementaryFolder)
        {
            return await GetImageSourceAsync(name, new StorageFolder[] { supplementaryFolder });
        }
        public static async Task<BitmapSource> GetImageSourceAsync(string name, StorageFolder[] supplementaryFolders = null)
        {
            var img = new BitmapImage();

            StorageFile imgFile = await GetFileFromEverywhereOrDefaultAsync(name, supplementaryFolders);

            FileRandomAccessStream stream = (FileRandomAccessStream)(await imgFile.OpenAsync(FileAccessMode.Read));
            img.SetSource(stream);

            return img;
        }

        public static async Task<StorageFolder> GetAppFolderAsync()
        {
            StorageFolder appFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("ResumeElements");
            appFolder = await appFolder.GetFolderAsync("ResumeElements_Resources");

            return await appFolder.GetFolderAsync("Images");
        }
        /// <summary>
        /// Open it or create it if it doesn't exist.
        /// </summary>
        /// <returns></returns>
        public static async Task<StorageFolder> GetLocalFolderAsync()
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

        public static async Task<StorageFolder[]> GetFoldersListAsync(StorageFolder supplementaryFolder)
        {
            return await GetFoldersListAsync(new StorageFolder[] { supplementaryFolder });
        }
        public static async Task<StorageFolder[]> GetFoldersListAsync(StorageFolder[] supplementaryFolders = null)
        {
            List<StorageFolder> folds = null;

            if (supplementaryFolders == null)
                folds = new List<StorageFolder>();
            else
                folds = new List<StorageFolder>(supplementaryFolders);

            folds.Add(await GetLocalFolderAsync());
            folds.Add(await GetAppFolderAsync());

            StorageFolder[] res = new StorageFolder[folds.Count];

            folds.CopyTo(res);

            return res;
        }

        public static async Task<StorageFile> GetFileFromEverywhereAsync(string name, StorageFolder supplementaryFolder)
        {
            return await GetFileFromEverywhereAsync(name, new StorageFolder[] { supplementaryFolder });
        }
        public static async Task<StorageFile> GetFileFromEverywhereAsync(string name, StorageFolder[] supplementaryFolders = null)
        {
            var imgFolds = await GetFoldersListAsync(supplementaryFolders);

            StorageFile imgFile = null;

            foreach (StorageFolder imgFold in imgFolds)
            {
                imgFile = await GetFileAsync(name, imgFold);

                if (imgFile != null)
                    break;
            }

            return imgFile;
        }

        public static async Task<StorageFile> GetFileFromEverywhereOrDefaultAsync(string name, StorageFolder supplementaryFolder)
        {
            return await GetFileFromEverywhereOrDefaultAsync(name, new StorageFolder[] { supplementaryFolder });
        }
        public static async Task<StorageFile> GetFileFromEverywhereOrDefaultAsync(string name, StorageFolder[] supplementaryFolders = null)
        {
            StorageFile imgFile = await GetFileFromEverywhereAsync(name, supplementaryFolders);

            if (imgFile == null)
                imgFile = await GetDefaultAsync();

            return imgFile;
        }

        public static async Task<StorageFile> GetFileAsync(string name, StorageFolder imgFold = null)
        {
            StorageFile imgFile = null;

            if (imgFold == null)
                imgFold = await GetLocalFolderAsync();

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

        public static async Task<bool> IsFilePresentLocallyAsync(string name, StorageFolder sf = null)
        {
            if (sf == null)
                sf = await GetLocalFolderAsync();

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

        public static async Task<StorageFile> GetDefaultAsync()
        {
            return await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///ResumeElements/ResumeElements_Resources/Images/StoreLogo.png"));
        }

        public static FileOpenPicker GetPicker()
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

        public static async Task<string> ImportFileAsync(StorageFile file, string desiredName, StorageFolder destFold = null)
        {
            if (destFold == null)
                destFold = await GetLocalFolderAsync();

            StorageFile copiedFile = null;

            //if (!(await IsFilePresentLocallyAsync(desiredName, imgFold)))
            copiedFile = await file.CopyAsync(destFold, desiredName + file.FileType, NameCollisionOption.GenerateUniqueName);
            //else
            //    throw new FileAlreadyExistsException("An image already exists with that name");

            return GetNameWithoutExtension(copiedFile.Name);
        }
        public static async Task<ImageRemovedOutput> RemoveFileAsync(string name, StorageFolder folder = null)
        {
            var imgFile = await GetFileAsync(name, folder);
            if (imgFile != null)
            {
                await imgFile.DeleteAsync();
                return ImageRemovedOutput.RestoredToDefault;
            }

            return ImageRemovedOutput.NothingDone;
        }

        public static async Task ReplaceFileAsync(StorageFile newFile, string name, StorageFolder destFold = null)
        {
            if (destFold == null)
                destFold = await GetLocalFolderAsync();

            var imgFile = await GetFileAsync(name, destFold);

            if (imgFile == null)
            {
                await ImportFileAsync(newFile, name);
            }
            else
            {
                await newFile.CopyAndReplaceAsync(imgFile);
            }
        }

        public static string GetNameWithoutExtension(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName).ToLower();
        }
    }
}
