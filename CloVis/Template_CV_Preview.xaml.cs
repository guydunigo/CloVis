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
using Resume;
using ResumeElements;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace CloVis
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Template_CV_Preview : Page
    {
        public Template_CV_Preview()
        {
            this.InitializeComponent();
            var resume1 = (Application.Current as App).Resumes[0];
            var resume2 = (Application.Current as App).Resumes[1];
            vboxLeft.Child = new Resume_Preview() { Resume = resume1 };
            vboxRight.Child = new Resume_Preview() { Resume = resume2 };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //var resume = (e.Parameter as Resume.Resume);
            //vboxLeft.Child = new Resume_Preview() { Resume = resume };
        }
    }
}
