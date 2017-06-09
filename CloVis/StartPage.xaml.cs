using ResumeStructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class StartPage : Page
    {
        /*private ObservableCollection<NavLink> topNavLinks = new ObservableCollection<NavLink>()
        {
            new NavLink() { Symbol= Windows.UI.Xaml.Controls.Symbol.OpenFile , Label="Ouvrir / Importer", Tag="OpenFile" },
            new NavLink() { Symbol= Windows.UI.Xaml.Controls.Symbol.Edit , Label="Modifiez vos données", Tag="DetailsEdit" },
        };
        public ObservableCollection<NavLink> TopNavLinks { get => topNavLinks; }

        private ObservableCollection<NavLink> bottomNavLinks = new ObservableCollection<NavLink>()
        {
            new NavLink() { Symbol= Windows.UI.Xaml.Controls.Symbol.Setting , Label="Paramètres", Tag="Settings" },
            new NavLink() { Symbol= Windows.UI.Xaml.Controls.Symbol.Help , Label="Aide", Tag="Help" },
        };
        public ObservableCollection<NavLink> BottomNavLinks { get => bottomNavLinks; }
        */
        public List<Resume> Resumes { get; set; }
        public List<Template> Templates { get; set; }

        public StartPage()
        {
            Resumes = ((App)(Application.Current)).Resumes;
            Templates = ((App)(Application.Current)).Templates;
            foreach (Template t in Templates)
            {
                t.UpdateFromIndex();
            }
            this.InitializeComponent();
        }

        private void NavLinkList_ItemClick(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Resumes_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Resume r)
            {
                this.Frame.Navigate(typeof(EditionMode), r.Copy());
            }
        }

        private void Templates_ItemClick(object sender, ItemClickEventArgs e)
        {
            Resumes_ItemClick(sender, e);
        }

        private void StackPanel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //((sender as Grid).FindName("Btn") as Button).Visibility = Visibility.Visible;
            //throw new NotImplementedException();
        }

        private void StackPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //((sender as Grid).FindName("Btn") as Button).Visibility = Visibility.Collapsed;
            //throw new NotImplementedException();
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DetailsEdit));
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Help));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem b)
            {
                // Tag retourne null
                if (b.Tag is Resume res)
                {
                    (Application.Current as App).RemoveResume(res);
                }
            }
        }

        private async void Open_Import(object sender, RoutedEventArgs e)
        {

            FileOpenPicker openPicker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            openPicker.FileTypeFilter.Add(".cv");
            var ImportCV = await openPicker.PickSingleFileAsync();

            if (ImportCV != null)
            {
                StorageFolder folder = await ResumeStructure.FileManagement.Resumes.GetLocalResumeFolder();
                await ImportCV.CopyAsync(folder, ImportCV.Name, NameCollisionOption.ReplaceExisting);

                var cv = await ResumeStructure.FileManagement.Resumes.Read_file(Path.GetFileNameWithoutExtension(ImportCV.Name), folder);

                bool exist = false;
                foreach (Resume r in Resumes)
                {
                    if (r.Name == cv.Name)
                    {
                        Resumes.Remove(r);
                        Resumes.Insert(0, cv);
                        exist = true;
                        break;
                    }
                }
                if (!exist) Resumes.Add(cv);
            }
            Window.Current.Content = new StartPage();
        }
    }
}
