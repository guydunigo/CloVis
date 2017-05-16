using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace CloVis
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class EditionMode : Page
    {
        public List<Resume.Resume> Resumes { get => ((App)(Application.Current)).Resumes; }
        public List<Resume.Template> Templates { get => ((App)(Application.Current)).Templates; }

        private Resume.Resume resume;
        public Resume.Resume Resume { get => resume; set => resume = value; }

        public EditionMode()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            resume = (e.Parameter as Resume.Resume);
            CV.Child = new Resume_Preview() { Resume = resume, BorderThickness=new Thickness(1), BorderBrush=Application.Current.Resources["CloVisBlue"] as SolidColorBrush };
        }

        private void Resumes_ItemClick(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Templates_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
        private void LeftButtonClick(object sender, RoutedEventArgs e)
        {
            if (LeftPane.IsPaneOpen)
            {
            LeftPane.IsPaneOpen = false;
            LeftButtonText.Text = "augmenter";
            LeftButtonIcon.Symbol = Symbol.OpenPane;
            }
            else
            {
            LeftPane.IsPaneOpen = true;
            LeftButtonText.Text = "réduire";
            LeftButtonIcon.Symbol = Symbol.ClosePane;
            }

        }
        private void RightButtonClick(object sender, RoutedEventArgs e)
        {
            if (RightPane.IsPaneOpen)
            {
            RightPane.IsPaneOpen = false;
            RightButtonText.Text = "augmenter";
            RightButtonIcon.Symbol = Symbol.ClosePane;
            }
            else
            {
            RightPane.IsPaneOpen = true;
            RightButtonText.Text = "réduire";
            RightButtonIcon.Symbol = Symbol.OpenPane;
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DetailsEdit));
        }
        
        private void Actualiser_Click(object sender, RoutedEventArgs e)
        {
            resume.UpdateFromIndex();

            (CV.Child as Resume_Preview).Resume = null;
            (CV.Child as Resume_Preview).Resume = resume;
        }

        private void ZoomSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sender is Slider slider && WorkBench != null)
                WorkBench.ChangeView(0, 0, (float)slider.Value);
        }

        private void WorkBench_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ZoomSlider.Value = WorkBench.ZoomFactor;
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;
            if (root == null)
                return;

            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (root.CanGoBack)
            {
                root.GoBack();
            }
        }
        private void Enregistrement(object sender, RoutedEventArgs e)
        {
           // FileManagement file = new FileManagement();
           // file.Create_File(resume);

        }
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Help));
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
