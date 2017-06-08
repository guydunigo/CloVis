using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ResumeElements
{
    public abstract class Element : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Elements name, there can't be two elements with the same name</param>
        /// <param name="isDefault">Defines wehter the element is to be shown on a resume by default (may not be used)</param>
        public Element(string name, bool isDefault = true)
        {
            Name = name;
            IsDefault = isDefault;
        }

        public string Name { get; }

        protected bool isDefault;
        public bool IsDefault
        {
            get => isDefault;
            set
            {
                isDefault = value;
                NotifyPropertyChanged("IsDefault");
            }
        }

        // This is used for XAML binding mainly :
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Provides a deep copy of every Elements and sub-Elements.
        /// </summary>
        /// <returns></returns>
        public abstract Element Copy();

        public abstract Element Find(string name);

        public abstract void UpdateFromIndex(NewIndex index = null);
    }
}