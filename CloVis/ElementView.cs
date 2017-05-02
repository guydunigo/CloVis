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
    public sealed class ElementView : Control
    {
        public ElementView()
        {
            this.DefaultStyleKey = typeof(ElementView);
            this.Loaded += OnLoaded;

            elementsToAdd = new List<UIElement>();
        }

        private List<UIElement> elementsToAdd;

        public ResumeElements.Element Element
        {
            get => (Element)(GetValue(ElementProperty));
            set
            {
                SetValue(ElementProperty, value);
            }
        }
        
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(Element), typeof(Resume_Preview), new PropertyMetadata(null, OnResumeChanged));

        private static void OnResumeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ElementView instance && instance.Element != null)
            {
                instance.elementsToAdd.Clear();
                // If element is already loaded, reload it :
                if (instance.GetTemplateChild("Element") != null) instance.OnLoaded(null, null);
            }
        }

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            (GetTemplateChild("Element") as Grid).Children.Clear();

            foreach (UIElement el in elementsToAdd)
            {
                (GetTemplateChild("Element") as Grid).Children.Add(el);
            }
        }

        public Inline RenderText(string text, FontElement font)
        {
            var tempText = new Run() { Text = text };
            Inline res = tempText;
            if (font != null)
            {
                tempText.Foreground = new SolidColorBrush(font.Color);
                tempText.FontFamily = font.Font;
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

        public RichTextBlock RenderTextBox(Element element, Resume.Fonts fonts)
        {
            RichTextBlock box = new RichTextBlock();
            if (element != null && fonts != null)
            {
                //throw new NotImplementedException("Generate rich text up to how many layers ? and shift (tab) ? Carriage return ? Dots for enum ?");
                var LayersNumber = 3;

                RenderElement(box, fonts, element, LayersNumber);
            }
            else if (fonts == null) throw new NullReferenceException("Fonts is null !");
            else if (element == null) throw new NullReferenceException("No element given.");

            return box;
        }

        public void RenderElement(RichTextBlock box, Resume.Fonts fonts, Element element, int remainingLayers, int layer = 0)
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
                    text = dd.StartTime.ToString(dd.DisplayFormat);

                    // If it hasn't finished
                    if (dd.EndTime == default(DateTime))
                    {
                        text = "Depuis " + text;
                    }
                    // If there's an end date
                    else if (dd.EndTime != dd.StartTime)
                    {
                        text += " - " + dd.EndTime.ToString(dd.DisplayFormat);
                    }

                }
                else if (d is DataTimeSpan<string> dts)
                {
                    text = dts.TimeSpan.ToString(dts.DisplayFormat);
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
