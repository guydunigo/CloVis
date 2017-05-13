using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Resume;
using ResumeElements;
using System.Collections.ObjectModel;
using System.Collections;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CloVis
{
    public class ElementListToObservableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ObservableCollection<Element> temp = null;
            if (value is ICollection<Element> ic)
            {
                temp = new ObservableCollection<Element>(ic);
            }
            return temp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class SubElementTemplateSelector: DataTemplateSelector
    {
        public new DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (item is Data<string> d)
                return null;

            return null;
        }
    }

    public sealed partial class IndexElementListView : UserControl, INotifyPropertyChanged
    {
        public IndexElementListView()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ResumeElements.ElementList ElementList
        {
            get => (ResumeElements.ElementList)(GetValue(ElementListProperty));
            set
            {
                SetValue(ElementListProperty, value);
                NotifyPropertyChanged("ElementList");
            }
        }

        public static readonly DependencyProperty ElementListProperty =
            DependencyProperty.Register("ElementList", typeof(ResumeElements.ElementList), typeof(IndexElementListView_old), new PropertyMetadata(null, OnElementListChanged));

        private static void OnElementListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IndexElementListView instance && instance.ElementList != null)
            {
                instance.NotifyPropertyChanged("ElementList");
            }
        }

        public void OpenClose_ElementList()
        {
            if (AddSymbol.Symbol == Symbol.Add)
            {
                AddSymbol.Symbol = Symbol.Remove;
                AddSymbol.Foreground = Application.Current.Resources["CloVisBlue"] as SolidColorBrush;

                ((AddSymbol.Parent as StackPanel).Parent as StackPanel).Children[1].Visibility = Visibility.Visible;
            }
            else if (AddSymbol.Symbol == Symbol.Remove)
            {
                AddSymbol.Symbol = Symbol.Add;
                AddSymbol.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);

                ((AddSymbol.Parent as StackPanel).Parent as StackPanel).Children[1].Visibility = Visibility.Collapsed;
            }
        }

        // Events handlers :

        private void ElementList_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            OpenClose_ElementList();
        }

        private void SymbolIcon_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var temp = sender as SymbolIcon;
            if (temp.Symbol == Symbol.Add)
                temp.Foreground = Application.Current.Resources["CloVisBlue"] as SolidColorBrush;
        }

        private void SymbolIcon_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var temp = sender as SymbolIcon;
            if (temp.Symbol == Symbol.Add)
                temp.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
        }

        private void SymbolIcon_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            OpenClose_ElementList();
        }
    }
}
