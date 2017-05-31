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

// Pour plus d'informations sur le modèle d'élément Boîte de dialogue de contenu, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace CloVis
{
    public enum SavingTemplateResult
    {
        Validate,
        NotValidate
    }
    
    
    public sealed partial class PreventSavingTemplateWithoutNameDialog : ContentDialog
    {
        public SavingTemplateResult Result { get; private set; }

        public string CV_name;
        public PreventSavingTemplateWithoutNameDialog()
        {
            this.InitializeComponent();
        }
        private void SaveName_Click(object sender, RoutedEventArgs e)
        {
            Validate(sender);
            if (Window.Current.Content is Frame f && f.Content is EditionMode root)
            {
                root.IsModified = false;

                this.Result = SavingTemplateResult.Validate;
                this.Hide();
            }
        }
        private void CvName_KeyDown(object sender, KeyRoutedEventArgs e)
        {
        }

        public void Validate(object sender)
        {
            TextBlock txt = new TextBlock();

            if (CvName.Text == "")
            {
                txt.Text = "Veuillez renseigner un nom.";
                txt.Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;
            }
            else if ((Application.Current as App).ExistResume(CvName.Text))
            {
                txt.Text = "Ce nom est déjà utilisé";
                txt.Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;
            }
            else
            {
                //ajouter le nom au CV
                CV_name = CvName.Text;
                txt.Text = "Cv Sauvegardé !";

            }
            var fo = new Flyout()
            {
                Content = txt
            };
            fo.ShowAt(CvName);
        }
    }
}
