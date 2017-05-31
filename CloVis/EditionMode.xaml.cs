using Resume;
using ResumeElements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Pdf;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace CloVis
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class EditionMode : Page, INotifyPropertyChanged
    {
        public List<Resume.Resume> Resumes { get => ((App)(Application.Current)).Resumes; }
        public List<Resume.Template> Templates { get => ((App)(Application.Current)).Templates; }

        public List<Resume.Resume> OpenResumes { get; set; }

        private void OpenCV(Resume.Resume res)
        {
            Resume = res;
            OpenResumes.Add(res);
            NotifyPropertyChanged("OpenResumes");
            ReloadCV();
            ReloadElementPanel();
        }
        private void CloseCV(Resume.Resume res)
        {
            if (OpenResumes.Count > 1)
            {
                OpenResumes.Remove(res);
            }
            else
                Back();

            if (Resume == res)
            {
                Resume = OpenResumes[0];

                ReloadCV();
                ReloadElementPanel();
            }

            NotifyPropertyChanged("OpenResumes");
        }
        private void ChangeCV(Resume.Resume res)
        {
            if (Resume != res)
            {
                Resume = res;

                ReloadCV();
                ReloadElementPanel();
            }
            
            NotifyPropertyChanged("OpenResumes");
        }

        public void ReloadCV()
        {
            (CV.Child as Controls.Resume_Preview).Resume = null;
            (CV.Child as Controls.Resume_Preview).Resume = resume;
        }

        public void ReloadElementPanel()
        {
            NotifyPropertyChanged("CurrentList");
            NotifyPropertyChanged("ListsHistory");
            EmptyElementListsHistory();
        }

        public Stack<ResumeElements.ElementList> ListsHistory { get; set; }
        public ResumeElements.ElementList CurrentList
        {
            get => ListsHistory.Peek();
        }
        public void AddToListsHistory(ElementList el)
        {
            ListsHistory.Push(el);
            if (ListsHistory.Count > 1)
                BackInListHistoryBtn.Visibility = Visibility.Visible;

            NotifyPropertyChanged("CurrentList");
            NotifyPropertyChanged("ListsHistory");
        }

        public ElementList RemoveLastInListsHistory()
        {
            ElementList res = null;
            if (ListsHistory.Count > 1)
                res = ListsHistory.Pop();

            if (ListsHistory.Count <= 1)
            {
                BackInListHistoryBtn.Visibility = Visibility.Collapsed;
                res = ListsHistory.Peek();
            }

            NotifyPropertyChanged("CurrentList");
            NotifyPropertyChanged("ListsHistory");

            return res;
        }

        private Resume.Resume resume;
        public Resume.Resume Resume { get => resume; set { resume = value; NotifyPropertyChanged("Resume"); } }

        public bool IsModified { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EditionMode()
        {
            this.InitializeComponent();
            IsModified = false;
            ListsHistory = new Stack<ElementList>();
            AddToListsHistory(Index.Root);
            OpenResumes = new List<Resume.Resume>();

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
                (Application.Current as App).OnBackRequested(null, null);
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

            var res = e.Parameter as Resume.Resume;
            Resume = res;
            OpenResumes.Add(res);
            NotifyPropertyChanged("OpenResumes");

            CV.Child = new Controls.Resume_Preview() { Resume = resume, BorderThickness = new Thickness(1), BorderBrush = Application.Current.Resources["CloVisBlue"] as SolidColorBrush };
        }

        private void Resumes_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Resume.Resume r)
            {
                OpenCV(r);
            }
        }

        private void Templates_ItemClick(object sender, ItemClickEventArgs e)
        {
            Resumes_ItemClick(sender, e);
        }

        private void LeftButtonClick(object sender, RoutedEventArgs e)
        {
            if (LeftPane.IsPaneOpen)
            {
                LeftPane.IsPaneOpen = false;
                // LeftButtonText.Text = "augmenter";
                LeftButtonIcon.Symbol = Symbol.OpenPane;
            }
            else
            {
                LeftPane.IsPaneOpen = true;
                //LeftButtonText.Text = "réduire";
                LeftButtonIcon.Symbol = Symbol.ClosePane;
            }

        }
        private void RightButtonClick(object sender, RoutedEventArgs e)
        {
            if (RightPane.IsPaneOpen)
            {
                RightPane.IsPaneOpen = false;
                //RightButtonText.Text = "augmenter";
                RightButtonIcon.Symbol = Symbol.ClosePane;
            }
            else
            {
                RightPane.IsPaneOpen = true;
                // RightButtonText.Text = "réduire";
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

            ReloadCV();
            EmptyElementListsHistory();

            IsModified = true;
        }


        public void EmptyElementListsHistory()
        {
            ListsHistory.Clear();
            AddToListsHistory(Index.Root);
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

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(ExportToPdf));

            /* // essai écriture en pdf, jpg, début Bitmap ok mais suite : pas les usings correspondants
            var name = "Resumes";
            StorageFolder folder;
            try
            {
                folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(name);
            }
            catch (FileNotFoundException)
            {
                folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(name);
            }
           
            RenderTargetBitmap rendertarget = new RenderTargetBitmap();
            await rendertarget.RenderAsync(this);
            var pixelBuffer = await rendertarget.GetPixelsAsync();
            using (var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream())
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)rendertarget.PixelWidth, (uint)rendertarget.PixelHeight, 96d, 96d, pixelBuffer.ToArray());

                await encoder.FlushAsync();
                //Load and draw the Bitmap image in png

            }
            */
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

        private void BackInListHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveLastInListsHistory();
        }

        private void EnterInLists_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ElementList el)
                AddToListsHistory(el);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox c)
            {
                if (Resume.LocalIndex.Find(CurrentList.Name) is ElementList<Element> parent && c.Tag is string s && !parent.ContainsKey(s) && Index.Find(s) is Element elmt)
                {
                    parent.Add(elmt.Copy());
                    IsModified = true;
                    ReloadCV();
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox c)
            {
                if (Resume.LocalIndex.Find(CurrentList.Name) is ElementList<Element> parent && c.Tag is string s && parent.ContainsKey(s))
                {
                    parent.Remove(parent.Find(c.Tag as string));
                    IsModified = true;
                    ReloadCV();
                }
            }
        }

        private async void CloseCV_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Resume.Resume r)
            {
                if (IsModified == true)
                {
                    ClosingResult res = await PreventExitingWithoutSavingAsync();

                    if (res == ClosingResult.Close)
                    {
                        CloseCV(r);
                    }
                }
                else
                    CloseCV(r);
            }
        }

        private void OpenResume_ItemClick(object sender, ItemClickEventArgs e)
        {
            ChangeCV(e.ClickedItem as Resume.Resume);
        }
    }
}
