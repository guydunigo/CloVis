using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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
using System.Threading.Tasks;
using Windows.Storage;

namespace CloVis
{
    /// <summary>
    /// Fournit un comportement spécifique à l'application afin de compléter la classe Application par défaut.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initialise l'objet d'application de singleton.  Il s'agit de la première ligne du code créé
        /// à être exécutée. Elle correspond donc à l'équivalent logique de main() ou WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoqué lorsque l'application est lancée normalement par l'utilisateur final.  D'autres points d'entrée
        /// seront utilisés par exemple au moment du lancement de l'application pour l'ouverture d'un fichier spécifique.
        /// </summary>
        /// <param name="e">Détails concernant la requête et le processus de lancement.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Done here (not in the constructor) because of Async problems
            LoadIndex();
            await LoadContent();

            Frame rootFrame = Window.Current.Content as Frame;

            // Ne répétez pas l'initialisation de l'application lorsque la fenêtre comporte déjà du contenu,
            // assurez-vous juste que la fenêtre est active
            if (rootFrame == null)
            {
                // Créez un Frame utilisable comme contexte de navigation et naviguez jusqu'à la première page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: chargez l'état de l'application précédemment suspendue
                }

                // Placez le frame dans la fenêtre active
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Quand la pile de navigation n'est pas restaurée, accédez à la première page,
                    // puis configurez la nouvelle page en transmettant les informations requises en tant que
                    // paramètre
                    rootFrame.Navigate(typeof(StartPage), e.Arguments);
                }
                // Vérifiez que la fenêtre actuelle est active
                Window.Current.Activate();
            }

            // Handle the back button use : (go back to start page)
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            // Display it if needed
            rootFrame.Navigated += (sender, eargs) =>
            {
                // Each time a navigation event occurs, update the Back button's visibility
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ? Windows.UI.Core.AppViewBackButtonVisibility.Visible :
                Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
            };
        }

        public void OnBackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;
            if (root == null)
                return;

            if (root.CanGoBack)
            {
                root.GoBack();
            }
        }

        /// <summary>
        /// Appelé lorsque la navigation vers une page donnée échoue
        /// </summary>
        /// <param name="sender">Frame à l'origine de l'échec de navigation.</param>
        /// <param name="e">Détails relatifs à l'échec de navigation</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Appelé lorsque l'exécution de l'application est suspendue.  L'état de l'application est enregistré
        /// sans savoir si l'application pourra se fermer ou reprendre sans endommager
        /// le contenu de la mémoire.
        /// </summary>
        /// <param name="sender">Source de la requête de suspension.</param>
        /// <param name="e">Détails de la requête de suspension.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: enregistrez l'état de l'application et arrêtez toute activité en arrière-plan
            deferral.Complete();
        }

        public List<Resume.Resume> Resumes { get; set; }
        public List<Resume.Template> Templates { get; set; }

        public async Task LoadResumes()
        {
            // async ?
            Resumes = new List<Resume.Resume>
            {
               // ResumeTest.GetResumeTest(),
                //ResumeTest.GetResumeTest2()
            };

            //charger les cv déjà remplis
            var storagelist = await FileManagement.GetResumeFoldersList();
            foreach (var stlist in storagelist)
            {
                var files = await stlist.GetFilesAsync();
                foreach (var file in files)
                {
                    if (Path.GetExtension(file.Name) == ".cv")
                    {
                        var temp = await FileManagement.Read_file(Path.GetFileNameWithoutExtension(file.Name), stlist);
                        Resumes.Add(temp);
                    }
                }
            }

            // var temp = await FileManagement.Read_file("CV_test");
            // Resumes.Add(temp);
        }

        public async Task LoadTemplates()
        {
            Templates = new List<Template>
            {
                // async ?

                //TemplateTest.GetTemplate_1(),
                //TemplateTest.GetTemplate_2(),
                TemplateTest.GetTemplate_3(),
                TemplateTest.GetTemplate_4()
            };

            //chargement des templates stockés dans les fichiers
            var storagelist = await FileManagement.GetTemplateFoldersList();
            foreach (var stlist in storagelist)
            {
                var files = await stlist.GetFilesAsync();
                foreach (var file in files)
                {
                    if (Path.GetExtension(file.Name) == ".cv")
                    {
                        Resume.Template temp = await FileManagement.Read_template(Path.GetFileNameWithoutExtension(file.Name), stlist);
                        Templates.Add(temp );
                    }
                }
            }

            // Fill templates with defaults informations
            foreach (Template e in Templates)
            {
                e.UpdateFromIndex();
                if (e.Fonts == null) e.Fonts = Fonts.GetDefault();
            }
        }

        public async void RemoveResume(Resume.Resume cv)
        {
            var temp = FindResume(cv.Name);

            if (temp != null) //si c'est un cv et pas un template
            {
                var storagelist = await FileManagement.GetResumeFoldersList();
                foreach (var stlist in storagelist)
                {
                    var files = await stlist.GetFilesAsync();
                    foreach (var file in files)
                    {
                        if (file.Name == cv.Name)
                        {
                            await file.DeleteAsync();
                        }
                    }
                }
                Resumes.Remove(cv);
            }

        }

        public async Task LoadContent()
        {
            await LoadResumes();
            await LoadTemplates();
        }

        public void LoadIndex()
        {
            LoadImages();
            Deprecated_Index_Test.FillIndex();
        }

        public void LoadImages()
        {
            Deprecated_Index.ReloadImages();
        }

        public async void SaveResume(Resume.Resume cv)
        {
            //throw new NotImplementedException("Async ?")
            //var file_saving = FileManagement.Save_File(cv);
            var temp = FindResume(cv.Name);

            if (temp == null)
            {
                var tempRes = new PreventSavingTemplateWithoutNameDialog();
                await tempRes.ShowAsync();
                if (tempRes.Result == SavingTemplateResult.Validate && tempRes.CV_name != "")
                {
                    cv.Name = tempRes.CV_name;
                    FileManagement.Save_File(cv);
                    SaveResumeInResumes(cv);
                }
            }
            else
            {
                FileManagement.Save_File(cv);
                SaveResumeInResumes(cv);
            }

            //await file_saving;
        }

        public void SaveResumeInResumes(Resume.Resume cv)
        {

            var temp = FindResume(cv.Name);
            if (temp != null)
            {
                Resumes.Remove(temp);
                Resumes.Insert(0, cv);
            }
            else
            {
                Resumes.Insert(0, cv);
            }
        }

        public Resume.Resume FindResume(string name)
        {
            foreach (Resume.Resume r in Resumes)
            {
                if (r.Name == name)
                    return r;
            }
            return null;
        }

        public bool ExistResume(string name)
        {
            foreach (Resume.Resume r in Resumes)
            {
                if (r.Name == name)
                    return true;
            }
            return false;
        }

        public void SaveResumes()
        {
            throw new NotImplementedException();
        }
    }
}
