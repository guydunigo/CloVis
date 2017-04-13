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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var resume = (e.Parameter as Resume.Resume);
            DisplayBackgroundBoxes(resume);
            DisplayTextBoxes(resume);
        }
        /*
        public Template_CV_Preview(Resume.Resume resume)
        {
            DisplayBackgroundBoxes(resume);

            this.InitializeComponent();
        }*/

        public void DisplayBackgroundBoxes(Resume.Resume resume)
        {
            foreach (BoxBackground b in resume.Layout.BackBoxes)
            {
                Resume.Children.Add(new Windows.UI.Xaml.Shapes.Rectangle()
                {
                    Width = b.SizeX,
                    Height = b.SizeY,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness() { Left = b.X, Top = b.Y },
                    Fill = new SolidColorBrush(b.Color),
                    RadiusX = b.BorderRadius,
                    RadiusY = b.BorderRadius,
                    StrokeThickness = 2,
                    Stroke = new SolidColorBrush(b.BorderColor),
                    RenderTransform = new RotateTransform()
                    {
                        Angle = b.Angle
                    },
                });
            }
        }

        public void DisplayTextBoxes(Resume.Resume resume)
        {
            string tempText = RenderText(resume.Fonts);

            foreach (BoxText b in resume.Layout.TextBoxes)
            {
                Resume.Children.Add(new Windows.UI.Xaml.Controls.TextBlock()
                {
                    Width = b.SizeX,
                    Height = b.SizeY,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness() { Left = b.X, Top = b.Y },
                    RenderTransform = new RotateTransform()
                    {
                        Angle = b.Angle
                    },
                    Text = tempText,
                });
            }
        }

        public string RenderText(Resume.Fonts fonts)
        {
            string temp = "Test";
            //throw new NotImplementedException("Generate rich text");
            return temp;
        }
    }
}
