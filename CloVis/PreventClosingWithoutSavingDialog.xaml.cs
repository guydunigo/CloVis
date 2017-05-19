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

// Pour plus d'informations sur le modèle d'élément Boîte de dialogue de contenu, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace CloVis
{
    public enum ClosingResult
    {
        Stay,
        Close
    }

    public sealed partial class PreventClosingWithoutSavingDialog : ContentDialog
    {
        public ClosingResult Result { get; private set; }

        public PreventClosingWithoutSavingDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void SaveResume_Click(object sender, RoutedEventArgs e)
        {
            if (Window.Current.Content is Frame f && f.Content is EditionMode root)
            {
                root.SaveResume();

                this.Result = ClosingResult.Close;
                this.Hide();
            }
        }

        private void CancelExiting_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ClosingResult.Stay;
            this.Hide();
        }

        private void DiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            if (Window.Current.Content is Frame f && f.Content is EditionMode root)
            {
                root.IsModified = false;

                this.Result = ClosingResult.Close;
                this.Hide();
            }
        }
    }
}
