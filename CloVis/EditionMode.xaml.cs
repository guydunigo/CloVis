using Resume;
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

        public bool IsModified { get; set; }

        public EditionMode()
        {
            this.InitializeComponent();
            IsModified = false;

            // Handle the back button use : (go back to start page)
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested -= (Application.Current as App).OnBackRequested;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += EditionMode_BackRequested;
        }

        private async void EditionMode_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            e.Handled = true;

            ClosingResult res = await PreventExitingWithoutSavingAsync();

            if (res == ClosingResult.Close)
            {
                (Application.Current as App).OnBackRequested(null,null);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (Application.Current as App).OnBackRequested;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested -= EditionMode_BackRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            resume = (e.Parameter as Resume.Resume);
            CV.Child = new Controls.Resume_Preview() { Resume = resume, BorderThickness=new Thickness(1), BorderBrush=Application.Current.Resources["CloVisBlue"] as SolidColorBrush };
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
        
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            resume.UpdateFromIndex();

            (CV.Child as Controls.Resume_Preview).Resume = null;
            (CV.Child as Controls.Resume_Preview).Resume = resume;

            IsModified = true;
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Back();
        }
        private void SaveResume_Click(object sender, RoutedEventArgs e)
        {
            SaveResume();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Help));
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public void SaveResume()
        {
            (Application.Current as App).SaveResume(resume);
            IsModified = false;
        }
        public async void Back()
        {
            ClosingResult res = await PreventExitingWithoutSavingAsync();
            // Navigate back if possible
            if (res == ClosingResult.Close && Window.Current.Content is Frame root && root.CanGoBack)
            {
                root.GoBack();
            }
        }

        private async System.Threading.Tasks.Task<ClosingResult> PreventExitingWithoutSavingAsync()
        {
            if (IsModified == true)
            {
                var temp = new PreventClosingWithoutSavingDialog();
                await temp.ShowAsync();
                return temp.Result;
            }
            else
                return ClosingResult.Close;
        }
    }
}
