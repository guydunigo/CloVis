using System;
using System.ComponentModel;
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
            //ImportImage(source, name);
            // + with resume/template
        }
    }
}
