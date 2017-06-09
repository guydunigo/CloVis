using ResumeElements.FileManagment;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace ResumeElements
{
    /// <summary>
    /// DataImage.Value is the file's name
    /// </summary>
    public class DataImage : DataText, INotifyPropertyChanged
    {
        /// <summary>
        /// If you use this constructor, there should be an existing image named name no matter the extension.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public DataImage(string name, Index index = null) : base(name, name, -1, null, false)
        {
            Index = index;
            if (index != null)
                index.AddImage(this);
        }

        public DataImage(StorageFile source) : this(source.Name, source)
        { }

        public DataImage(string name, StorageFile source) :
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
            value = await Images.ImportFileAsync(file, desiredName);
        }
        public async Task ReplaceImageFile(StorageFile newFile)
        {
            await Images.ReplaceFileAsync(newFile, Name);
        }
        public async Task<ImageRemovedOutput> RemoveAsync()
        {
            var res = await FileManagment.Images.RemoveFileAsync(Name);

            // If it is not a default picture, remove it from the index.
            if (Index != null && (await Images.GetFileAsync(value, await Images.GetAppFolderAsync())) == null)
            {
                Index.Images.Remove(this);
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
