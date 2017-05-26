﻿using System;
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
            Index.AddImage(this);
        }

        public DataImage(StorageFile source):this(source.Name,source)
        { }

        public DataImage(string name, StorageFile source) :
            this(name)
        {
            ImportImage(source, name);
            // + with resume/template
        }

        public static async Task<BitmapSource> GetImageSource(string name)
        {
            var img = new BitmapImage();

            StorageFile imgFile = await GetImageFileFromEverywhereOrDefault(name);

            FileRandomAccessStream stream = (FileRandomAccessStream)(await imgFile.OpenAsync(FileAccessMode.Read));
            img.SetSource(stream);

            return img;
        }

        public static async Task<StorageFile> GetImageFileFromEverywhereOrDefault(string name)
        {
            StorageFile imgFile = await GetImageFileFromEverywhere(name);

            if (imgFile == null)
                imgFile = await GetImageDefault();

            return imgFile;
        }

        public static async Task<StorageFile> GetImageFileFromEverywhere(string name)
        {
            var imgFolds = await GetImageFoldersList();
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

        public async static Task<StorageFolder[]> GetImageFoldersList()
        {
            StorageFolder[] res = new StorageFolder[]
            {
                await GetLocalImageFolder(),
                await GetAppImageFolder()
            };

            return res;
        }

        public async static Task<StorageFolder> GetAppImageFolder()
        {
            StorageFolder appFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
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

        public async void ImportImage(StorageFile file, string desiredName)
        {
            var imgFold = await GetLocalImageFolder();
            StorageFile copiedFile = null;

            if (!await IsImageFilePresent(desiredName, imgFold))
                copiedFile = await file.CopyAsync(imgFold, desiredName);
            else
                throw new FileAlreadyExistsException("An image already exists with that name");
            
            value = GetNameWithoutExtension(copiedFile.Name);
        }

        public override Element Copy()
        {
            throw new NotImplementedException();
        }

        public override void UpdateFromIndex()
        {
            throw new NotImplementedException();
        }

        public async void ReplaceImageFile(StorageFile newFile)
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

        public async void Remove()
        {
            var imgFile = await GetImageFile();
            if (imgFile != null)
            {
                await imgFile.DeleteAsync();
            }
            Index.Images.Remove(this);
        }

        public static async Task<bool> IsImageFilePresent(string name, StorageFolder sf = null)
        {
            if (sf == null)
                sf = await GetLocalImageFolder();

            var nameCut = GetNameWithoutExtension(name);

            var files = await sf.GetFilesAsync();
            var res = false;

            foreach(StorageFile f in files)
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
        public static string GetExtension(StorageFile sf)
        {
            return GetExtension(sf.Name);
        }
    }
}