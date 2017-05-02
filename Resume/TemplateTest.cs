﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResumeElements;
using Windows.UI;



namespace Resume
{
    public static class TemplateTest
    {
        public static Template GetTemplate_1()
        {

            var template = new Template();


            var fonts = new Fonts("Polices_cv")
            {
                new FontElement("Tahoma", 7,  new Color() { R = 120, G = 120, B = 200, A = 100 }, false, true, true, false),    //italic, gras, souligné
                new FontElement("Tahoma", 5, new Color() { R = 0, G = 0, B = 150, A = 255 },true, true, false, true),
                new FontElement("Calibri", 4, new Color() { R = 100, G = 100, B = 200, A = 255 },true, false, false, false),
                new FontElement("Calibri", 3, new Color() { R = 70, G = 70, B = 200, A = 190 },false, false, false, false)
        };
            var boite_competences = new BoxText(75, 80, 50, 105, 75, 0, "compétences");
            var boite_coordonnées = new BoxText(5, 40, 20, 200, 60, 10, "coordonnées");
            var boite_centres_dinteret = new BoxText(75, 200, 10, 150, 50, 0, "Centres d'intérêt");
            var boite_langues = new BoxText(5, 200, 10, 70, 50, 0, "langues");
            var boite_diplomes = new BoxText(75, 130, 20, 130, 50, 0, "diplômes");

            var fond = new BoxBackground(0, 0, 20, 70, 300, null, 20)
            {
                Color = new Color() { A = 50, B = 120, G = 130 } // fond peu transparent qui se trouve sous la boite contact
            };

            var fond2 = new BoxBackground(65, 0, 40, 150, 300)
            {
                Color = new Color() { A = 200, R = 50, G = 120, B = 120 }
            };

            template.Layout = new Layout();
            template.Layout.AddTextBox(boite_coordonnées);
            template.Layout.AddTextBox(boite_langues);
            template.Layout.AddTextBox(boite_competences);
            template.Layout.AddTextBox(boite_centres_dinteret);
            template.Layout.AddTextBox(boite_diplomes);
            template.Layout.AddBackBox(fond);
            template.Layout.AddBackBox(fond2);

            return template;
        }





        public static Template GetTemplate_2()
        {
            var template = new Template();

            var fonts = new Fonts("Polices_cv")
            {
                new FontElement("Colibri", 9,  new Color() { R = 120, G = 0, B = 200, A = 100 }, false, true, true, false),    //italic, gras, souligné
                new FontElement("Colibri", 7, new Color() { R = 0, G = 0, B = 150, A = 255 },true, true, false, true),
                new FontElement("Tahoma", 6, new Color() { R = 0, G = 100, B = 200, A = 255 },true, false, false, false),
                new FontElement("Tahoma", 4, new Color() { R = 170, G = 0, B = 0, A = 190 },false, false, true, false)
            };

            var boite_diplomes = new BoxText(20, 120, 35, 100, 75, 0, "diplômes");
            var boite_coordonnées = new BoxText(20, 40, 20, 60, 70, 10, "coordonnées");
            var boite_langues = new BoxText(20, 200, 10, 100, 50, 0, "langues");
            
            var fond = new BoxBackground(0, 100, 50, 210, 190, null, 20)
            { 
                Color = new Color() { A = 200, R = 255, G = 255, B=51} // fond jaune
            };

            var fond2 = new BoxBackground(0, 0, 40, 210, 100)
            {
                Color = new Color() { A = 150, R = 255, G = 130 } //fond orange
            };

            template.Layout = new Layout();
            template.Layout.AddTextBox(boite_coordonnées);
            template.Layout.AddTextBox(boite_diplomes);
            template.Layout.AddTextBox(boite_langues);
            template.Layout.AddBackBox(fond);
            template.Layout.AddBackBox(fond2);
           


            return template;
        }
    }
}
