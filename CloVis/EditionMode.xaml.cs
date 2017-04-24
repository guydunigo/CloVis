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
        public List<Resume.Resume> Resumes { get; set; }
        public List<Resume.Template> Templates { get; set; }

        public EditionMode()
        {
            Resumes = ((App)(Application.Current)).Resumes;
            Templates = ((App)(Application.Current)).Templates;
            this.InitializeComponent();
        }

        private void Resumes_ItemClick(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Templates_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
