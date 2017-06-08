using System;
using System.ComponentModel;

namespace ResumeElements
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

    /// <summary>
    /// DataImage.Value is the file's name
    /// </summary>
    /*public class DataImage : DataText, INotifyPropertyChanged
    {
    }*/
}
