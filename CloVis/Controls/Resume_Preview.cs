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
using Windows.UI.Xaml.Shapes;
using System.Threading.Tasks;
using System.Collections;
using Windows.Foundation;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CloVis.Controls
{
    public sealed class Resume_Preview : Control
    {
        public Resume_Preview()
        {
            this.DefaultStyleKey = typeof(Resume_Preview);
            this.Loaded += OnLoaded;

            elementsToAdd = new SortedList<double, UIElement>();
        }

        private SortedList<double,UIElement> elementsToAdd;

        public bool IsTextSelectionEnable
        {
            get => (bool)(GetValue(IsTextSelectionEnableProperty));
            set
            {
                SetValue(IsTextSelectionEnableProperty, value);
            }
        }

        public static readonly DependencyProperty IsTextSelectionEnableProperty =
            DependencyProperty.Register("IsTextSelectionEnable", typeof(bool), typeof(Resume_Preview), new PropertyMetadata(true, OnIsTextSelectionEnableChanged));

        private static void OnIsTextSelectionEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnResumeChanged(d, e);
        }

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

        private static async void OnResumeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Resume_Preview instance && instance.Resume != null)
            {
                instance.elementsToAdd.Clear();
                await instance.RenderBackgroundBoxes();
                instance.RenderTextBoxes();

                // If element is already loaded, reload it :
                if (instance.GetTemplateChild("Resume") != null)
                {
                    instance.OnLoaded(null, null);
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (GetTemplateChild("Resume") as Grid).Children.Clear();

            foreach (UIElement el in elementsToAdd.Values)
            {
                (GetTemplateChild("Resume") as Grid).Children.Add(el);
            }
        }

        private async Task<Brush> GetBackgroundBoxFillBrush(BoxBackground b)
        {
            if (b.Image != null)
            {
                var cv_template_Folds = await FileManagement.GetResumeTemplateFoldersList();
                //throw new NotImplementedException("other folders ...");
                return new ImageBrush() {
                    ImageSource = await DataImage.GetImageSource(b.Image.Value, cv_template_Folds)
                };
            }
            else
            {
                return new SolidColorBrush(b.Fill);
            }
        }

        private void AddToElementsToAdd(double z, UIElement elmt)
        {
            while (elementsToAdd.Keys.Contains(z))
                z++;
            elementsToAdd.Add(z, elmt);
        }

        private async Task RenderBackgroundBoxes()
        {
            if (Resume != null)
            {
                Shape shape = null;

                foreach (BoxBackground b in Resume.Layout.BackBoxes)
                {
                    switch (b.Shape)
                    {
                        case BoxBackgroundShape.Ellipse:
                            shape = new Ellipse();
                            break;
                        case BoxBackgroundShape.Rectangle:
                        default:
                            shape = new Windows.UI.Xaml.Shapes.Rectangle()
                            {
                                RadiusX = b.BorderRadius,
                                RadiusY = b.BorderRadius,
                            };
                            break;
                    }

                    shape.Width = b.SizeX;
                    shape.Height = b.SizeY;
                    shape.HorizontalAlignment = HorizontalAlignment.Left;
                    shape.VerticalAlignment = VerticalAlignment.Top;
                    shape.Margin = new Thickness() { Left = b.X, Top = b.Y };
                    shape.StrokeThickness = b.StrokeThickness;
                    shape.Stroke = new SolidColorBrush(b.Stroke);
                    shape.Fill = await GetBackgroundBoxFillBrush(b);
                    shape.RenderTransform = new RotateTransform()
                    {
                        Angle = b.Angle
                    };

                    AddToElementsToAdd(b.Z, shape);
                }
            }
        }

        private void RenderTextBoxes()
        {
            if (Resume != null)
            {
                RichTextBlock tempText = null;

                foreach (BoxText b in Resume.Layout.TextBoxes)
                {
                    if (b.Element != null)
                        tempText = RenderTextBox(b.Element, b.Fonts ?? Resume.Fonts);
                    else
                    {
                        tempText = new RichTextBlock();

                        var para = new Paragraph();
                        para.Inlines.Add(new Run()
                        {
                            Text = "Element non trouvé.\nCette boite attend un élément nommé : ",
                            FontSize = 5,
                            Foreground = new SolidColorBrush(Windows.UI.Colors.Black)
                        });
                        var temp = new Bold();
                        temp.Inlines.Add(new Run()
                        {
                            Text = b.DefaultElement,
                            FontSize = 5,
                            Foreground = new SolidColorBrush(Windows.UI.Colors.Red)
                        });
                        para.Inlines.Add(temp);
                        tempText.Blocks.Add(para);
                    }
                    tempText.Width = b.SizeX;
                    tempText.Height = b.SizeY;
                    tempText.HorizontalAlignment = HorizontalAlignment.Left;
                    tempText.VerticalAlignment = VerticalAlignment.Top;
                    tempText.Margin = new Thickness() { Left = b.X, Top = b.Y };
                    tempText.RenderTransform = new RotateTransform()
                    {
                        Angle = b.Angle
                    };
                    // throw new NotImplementedException("Z index");
                    AddToElementsToAdd(b.Z, tempText);
                }
            }
        }

        private Inline RenderText(string text, FontElement font)
        {
            var tempText = new Run() { Text = text };
            Inline res = tempText;
            if (font != null)
            {
                tempText.Foreground = new SolidColorBrush(font.Color);
                tempText.FontFamily = new FontFamily(font.FontName);
                tempText.FontSize = font.FontSize;
                if (font.UpperCase) tempText.Text = tempText.Text.ToUpper();
                if (font.Underlined)
                {
                    var temp = new Underline();
                    temp.Inlines.Add(res);
                    res = temp;
                }
                if (font.Bold)
                {
                    var temp = new Bold();
                    temp.Inlines.Add(res);
                    res = temp;
                }
                if (font.Italic)
                {
                    var temp = new Italic();
                    temp.Inlines.Add(res);
                    res = temp;
                }
            }
            else
            {
                throw new NullReferenceException("No FontElement given !");
            }

            return res;
        }

        private RichTextBlock RenderTextBox(Element element, Resume.Fonts fonts)
        {
            RichTextBlock box = new RichTextBlock();

            if (element != null && fonts != null)
            {
                //throw new NotImplementedException("Generate rich text up to how many layers ? and shift (tab) ? Carriage return ? Dots for enum ?");
                var LayersNumber = 3;

                RenderElement(box, fonts, element, LayersNumber);
                
                box.TextAlignment = fonts.TextAlignment;
            }
            else if (fonts == null) throw new NullReferenceException("Fonts is null !");
            else if (element == null) throw new NullReferenceException("No element given.");

            box.IsTextSelectionEnabled = IsTextSelectionEnable;
            return box;
        }

        private void RenderElement(RichTextBlock box, Resume.Fonts fonts, Element element, int remainingLayers, int layer = 0)
        {
            Paragraph para = null;

            FontElement fe = null;
            para = new Paragraph()
            {
                //TextIndent = layer > 0 ? 10 * (layer - 1) : 0
            };

            if (element is ElementList list)
            {
                fonts.TryGetValue(layer, out fe);

                para.Inlines.Add(RenderText(element.Name, fe));
                box.Blocks.Add(para);

                if (remainingLayers > 0)
                {
                    foreach (Element e in list.Values)
                    {
                        RenderElement(box, fonts, e, remainingLayers - 1, layer + 1);
                    }
                }
            }
            else if (element is Data<string> d)
            {
                string text = "";

                // Adds date if DataDated or DataTimeSpan then display the value
                if (d is DataDated<string> dd)
                {
                    text = dd.RenderDates();
                }
                else if (d is DataTimeSpan<string> dts)
                {
                    text = dts.RenderTimeSpan();
                }

                if (text != "")
                {
                    fonts.TryGetValue(layer++, out fe);
                    para.Inlines.Add(RenderText(text + " : ", fe));
                }

                fonts.TryGetValue(layer, out fe);
                para.Inlines.Add(RenderText(d.Value, fe));
                box.Blocks.Add(para);
            }
        }
    }
}
