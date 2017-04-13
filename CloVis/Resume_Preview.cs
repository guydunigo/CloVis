using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Resume;
using ResumeElements;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CloVis
{
    public sealed class Resume_Preview : Control
    {
        public Resume_Preview()
        {
            this.DefaultStyleKey = typeof(Resume_Preview);
            this.Loaded += OnLoaded;

            elementsToAdd = new List<UIElement>();
        }

        private List<UIElement> elementsToAdd;
        
        public Resume.Resume Resume
        {
            get => (Resume.Resume)(GetValue(ResumeProperty));
            set
            {
                SetValue(ResumeProperty, value);
            }
        }

        public static readonly DependencyProperty ResumeProperty =
            DependencyProperty.Register("Resume", typeof(Resume.Resume), typeof(Resume_Preview), new PropertyMetadata(null, OnResumeChanged));

        private static void OnResumeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Resume_Preview instance && instance.Resume != null)
            {
                instance.elementsToAdd.Clear();
                instance.RenderBackgroundBoxes();
                instance.RenderTextBoxes();
                // If element is already loaded, reload it :
                if (instance.GetTemplateChild("Resume") != null) instance.OnLoaded(null, null);
            }
        }


        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            (GetTemplateChild("Resume") as Grid).Children.Clear();

            foreach (UIElement el in elementsToAdd)
            {
                (GetTemplateChild("Resume") as Grid).Children.Add(el);
            }
        }

        public void RenderBackgroundBoxes()
        {
            if (Resume != null)
            {
                foreach (BoxBackground b in Resume.Layout.BackBoxes)
                {
                    elementsToAdd.Add(new Windows.UI.Xaml.Shapes.Rectangle()
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
        }

        public void RenderTextBoxes()
        {
            if (Resume != null)
            {
                string tempText = RenderText(Resume.Fonts);

                foreach (BoxText b in Resume.Layout.TextBoxes)
                {
                    elementsToAdd.Add(new Windows.UI.Xaml.Controls.TextBlock()
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
        }

        public string RenderText(Resume.Fonts fonts)
        {
            if (Resume != null)
            {
                string temp = "Test";
                //throw new NotImplementedException("Generate rich text");
                return temp;
            }
            else
                return "Error rendering text !";
        }
    }
}
