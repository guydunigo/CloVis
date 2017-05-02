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
        public Resume.Resume Resume { get => resume; set => Resume = value.Copy(); }

        public EditionMode()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var resume = (e.Parameter as Resume.Resume);
            CV.Child = new Resume_Preview() { Resume = resume };
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
    }
}
