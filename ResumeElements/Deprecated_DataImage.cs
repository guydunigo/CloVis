using ResumeElements.FileManagment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ResumeElements
{
    /// <summary>
    /// DataImage.Value is the file's name
    /// </summary>
    public class Deprecated_DataImage : Deprecated_Data<string>, INotifyPropertyChanged
    {
        /// <summary>
        /// If you use this constructor, there must be an existing image named name no matter the extension
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public Deprecated_DataImage(string name, bool isIndependant = false) : base(name, -1, "", true, false)
        {
            if (!isIndependant)
                Deprecated_Index.AddImage(this);
        }

        public Deprecated_DataImage(StorageFile source) : this(source.Name, source)
        { }

        public Deprecated_DataImage(string name, StorageFile source) :
            this(name)
        {
            ConstructorImportImageAsync(source, name);
            // + with resume/template
        }

        public async Task<StorageFile> GetImageFileAsync()
        {
            return await FileManagment.Images.GetFileAsync(value);
        }

        /// <summary>
        /// Constructor can't be async so I'm doing this dirty thing :/.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="desiredName"></param>
        protected async void ConstructorImportImageAsync(StorageFile file, string desiredName)
        {
            await ImportImageAsync(file, desiredName);
        }

        public async Task ImportImageAsync(StorageFile file, string desiredName)
        {
            value = await Images.ImportFileAsync(file,desiredName);
        }
        public async Task ReplaceImageFile(StorageFile newFile)
        {
            await Images.ReplaceFileAsync(newFile, Name);
        }
        public async Task<ImageRemovedOutput> RemoveAsync()
        {
            var res = await FileManagment.Images.RemoveFileAsync(Name);

            // If it is not a default picture
            if ((await Images.GetFileAsync(value, await Images.GetAppFolderAsync())) == null)
            {
                Deprecated_Index.Images.Remove(this);
                res = ImageRemovedOutput.RemovedFromIndex;
            }

            return res;
        }

        public override Element Copy()
        {
            throw new NotImplementedException();
        }

        public override void UpdateFromIndex(Index indexToUse = null)
        {
            throw new NotImplementedException();
        }
    }
}
