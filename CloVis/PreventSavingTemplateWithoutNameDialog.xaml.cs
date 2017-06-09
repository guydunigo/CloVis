using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

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
            Result = SavingTemplateResult.NotValidate;
        }
        private void SaveName_Click(object sender, RoutedEventArgs e)
        {
            Validate(sender);
        }
        private void CvName_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                SaveName_Click(sender, e);
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

                if (Window.Current.Content is Frame f && f.Content is EditionMode root)
                {
                    root.IsModified = false;

                    this.Result = SavingTemplateResult.Validate;
                    this.Hide();
                }

                return;
            }
            var fo = new Flyout()
            {
                Content = txt
            };
            fo.ShowAt(CvName);

        }

        private void Save_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Validate(sender);
        }

        private void Cancel_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }
    }
}
