using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CloVis
{
    public sealed class IndexElementListView : Control
    {
        public IndexElementListView()
        {
            this.DefaultStyleKey = typeof(IndexElementListView);
            this.Loaded += OnLoaded;

            elementsToAdd = new List<UIElement>();
            OnElementListChanged(this, null);
        }

        private List<UIElement> elementsToAdd;

        private void OnLoaded(object sender = null, RoutedEventArgs e = null)
        {
            if (GetTemplateChild("Name") is TextBlock tb)
                tb.Text = ElementList.Name;
            if (GetTemplateChild("SubList") is ListView lv)
            {
                lv.Items.Clear();

                foreach (UIElement el in elementsToAdd)
                    lv.Items.Add(el);
            }

            // Have to link events manually :/
            if (GetTemplateChild("AddSymbol") is SymbolIcon si)
            {
                si.PointerEntered += SymbolIcon_PointerEntered;
                si.PointerExited += SymbolIcon_PointerExited;
                si.PointerPressed += SymbolIcon_PointerPressed;
            }
            if (GetTemplateChild("Head") is StackPanel sp)
            {
                sp.DoubleTapped += ElementList_DoubleTapped;
            }
        }

        public ResumeElements.ElementList ElementList
        {
            get => (ResumeElements.ElementList)(GetValue(ElementListProperty));
            set => SetValue(ElementListProperty, value);
        }

        public static readonly DependencyProperty ElementListProperty =
            DependencyProperty.Register("ElementList", typeof(ResumeElements.ElementList), typeof(IndexElementListView), new PropertyMetadata(null, OnElementListChanged));

        private static void OnElementListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IndexElementListView instance && instance.ElementList != null)
            {
                if (instance.GetTemplateChild("Name") is TextBlock tb)
                    tb.Text = instance.ElementList.Name;
                
                instance.elementsToAdd.Clear();
                foreach(ResumeElements.Element elem in instance.ElementList.Values)
                {
                    instance.Add(elem);
                }
                if (instance.GetTemplateChild("ElementListStackPanel") != null) instance.OnLoaded();
            }
        }

        public void OpenClose_ElementList()
        {
            var temp = GetTemplateChild("AddSymbol") as SymbolIcon;
            if (temp.Symbol == Symbol.Add)
            {
                temp.Symbol = Symbol.Remove;
                temp.Foreground = Application.Current.Resources["CloVisBlue"] as SolidColorBrush;

                ((temp.Parent as StackPanel).Parent as StackPanel).Children[1].Visibility = Visibility.Visible;
            }
            else if (temp.Symbol == Symbol.Remove)
            {
                temp.Symbol = Symbol.Add;
                temp.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);

                ((temp.Parent as StackPanel).Parent as StackPanel).Children[1].Visibility = Visibility.Collapsed;
            }
        }

        public void Add(Control c)
        {
            elementsToAdd.Add(new ListViewItem() { Content = c });

            if (GetTemplateChild("SubList") != null) OnLoaded();
        }

        public void Add(ResumeElements.Element e)
        {
            if (e is ResumeElements.ElementList el)
                Add(new IndexElementListView() { ElementList = el });
            else if (e is ResumeElements.DataDated<string> dd)
            {
                //throw new NotImplementedException();
            }
            else if (e is ResumeElements.DataTimeSpan<string> dts)
            {
                //throw new NotImplementedException();
            }
            else if (e is ResumeElements.Data<string> d)
                Add(new IndexDataTextView() { Data = d });
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
