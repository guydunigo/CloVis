﻿using System;
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
                RichTextBlock tempText = null;

                foreach (BoxText b in Resume.Layout.TextBoxes)
                {
                    tempText = RenderTextBox(b.Element, Resume.Fonts);
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
                    elementsToAdd.Add(tempText);
                }
            }
        }

        public Inline RenderText(string text, FontElement font = null)
        {
            //throw new NotImplementedException();

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
                //throw new NotImplementedException("default ?");
            }

            return res;
        }

        public RichTextBlock RenderTextBox(Element element, Resume.Fonts fonts)
        {
            RichTextBlock box = new RichTextBlock();
            if (Resume != null && element != null && fonts != null)
            {
                //throw new NotImplementedException("Generate rich text up to how many layers ? and shift ? (tab)");
                var LayersNumber = 3;

                RenderElement(box, fonts, element, LayersNumber);
            }
            else
            {
                var para = new Paragraph();
                para.Inlines.Add(new Run()
                {
                    Text = "Error rendering text !",
                    Foreground = new SolidColorBrush(Windows.UI.Colors.Red)
                });
                box.Blocks.Add(para);
            }

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
                // throw new NotImplementedException("Generic bug (gen necessary ?)");
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

                //throw new NotImplementedException("Carriage return ? Dots for enum ?);

                fonts.TryGetValue(layer, out fe);
                para.Inlines.Add(RenderText(d.Value, fe));
                box.Blocks.Add(para);
            }
        }
    }
}
