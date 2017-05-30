using System;
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

            var template = new Template("Temp1");


            var fonts = new Fonts("Polices_cv")
            {
                new FontElement("Times New Roman", 7,  new Color() { R = 255, G = 0, B = 0, A = 100 }, false, true, true, false),    //italic, gras, souligné
                new FontElement("Calibri", 5, new Color() { R = 0, G = 255, B = 0, A = 255 },true, true, false, true),
                new FontElement("Verdana", 4, new Color() { R = 255, G = 0, B = 255, A = 255 },true, false, false, false),
                new FontElement("Comic Sans MS", 3, new Color() { R = 70, G = 70, B = 200, A = 190 },false, false, false, false)
        };
            var boite_competences = new BoxText(75, 80, 50, 105, 75, 0, "Compétences");
            var boite_coordonnées = new BoxText(5, 40, 20, 200, 60, 10, "Coordonnées");
            var boite_etudes = new BoxText(75, 200, 10, 150, 50, 0, "Études");
            var boite_exp = new BoxText(75, 250, 10, 150, 50, 0, "Expériences professionnelles");
            var boite_langues = new BoxText(5, 200, 10, 70, 50, 0, "Langues");
            var boite_diplomes = new BoxText(75, 130, 20, 130, 50, 0, "Diplômes");

            var fond = new BoxBackground(0, 0, 20, 70, 300, 0)
            {
                Fill = new Color() { A = 50, B = 120, G = 130 } // fond peu transparent qui se trouve sous la boite contact
            };

            var fond2 = new BoxBackground(65, 0, 40, 150, 300)
            {
                Fill = new Color() { A = 200, R = 50, G = 120, B = 120 }
            };

            var fond3 = new BoxBackground(120, 100, 20, 70, 70, 20)
            {
                Fill = new Color() { A = 200, R = 255 }
            };

            template.Layout = new Layout();
            template.Fonts = fonts;
            template.Layout.AddTextBox(boite_coordonnées);
            template.Layout.AddTextBox(boite_langues);
            template.Layout.AddTextBox(boite_competences);
            template.Layout.AddTextBox(boite_etudes);
            template.Layout.AddTextBox(boite_exp);
            template.Layout.AddTextBox(boite_diplomes);
            template.Layout.AddBackBox(fond);
            template.Layout.AddBackBox(fond2);
            template.Layout.AddBackBox(fond3);

            return template;
        }

        public static Template GetTemplate_2()
        {
            var template = new Template("Temp2");

            var fonts = new Fonts("Polices_cv")
            {
                new FontElement("Colibri", 9,  new Color() { R = 120, G = 0, B = 0, A = 200 }, false, true, true, false),    //italic, gras, souligné
                new FontElement("Times New Roman", 7, new Color() { R = 0, G = 0, B = 150, A = 255 },true, true, false, true),
                new FontElement("Tahoma", 6, new Color() { R = 0, G = 0, B = 0, A = 255 },true, false, false, false),
                new FontElement("Tahoma", 4, new Color() { R = 170, G = 0, B = 0, A = 190 },false, false, true, false)
            };

            var boite_diplomes = new BoxText(20, 120, 35, 100, 75, 0, "Diplômes");
            var boite_coordonnées = new BoxText(20, 40, 20, 60, 70, 10, "Coordonnées");
            var boite_langues = new BoxText(20, 200, 10, 100, 50, 0, "Langues");

            var fond = new BoxBackground(0, 100, 50, 210, 190, 0)
            {
                Fill = new Color() { A = 200, R = 255, G = 255, B = 51 } // fond jaune
            };

            var fond2 = new BoxBackground(0, 0, 40, 210, 100)
            {
                Fill = new Color() { A = 150, R = 255, G = 130 } //fond orange
            };

            template.Layout = new Layout();
            template.Fonts = fonts;
            template.Layout.AddTextBox(boite_coordonnées);
            template.Layout.AddTextBox(boite_diplomes);
            template.Layout.AddTextBox(boite_langues);
            template.Layout.AddBackBox(fond);
            template.Layout.AddBackBox(fond2);



            return template;
        }
        public static Template GetTemplate_3()
        {
            var template = new Template("Temp3");

            var fonts = new Fonts("Polices_cv")
            {
                new FontElement("Colibri", 3,  new Color() { R = 120, G = 0, B = 0, A = 200 }, false, false, false, false)   //italic, gras, souligné
            };

            var boite_diplomes = new BoxText(8, 170, 20, 193, 64, 0, "Diplômes");
            var boite_coordonnées = new BoxText(134, 52, 20, 67, 52, 0, "Coordonnées");
            var boite_langues = new BoxText(150, 113, 10, 24, 28, 0, "Langues");
            var boite_nom = new BoxText(64, 12, 20, 81, 15, 0, "Nom");
            var boite_profession = new BoxText(64, 33, 0, 81, 6, 0, "Profession");
            var boite_competences = new BoxText(8,112,0,22,28,0,"Compétences");
            var fond = new BoxBackground(0, 0, 0, 210, 297, 0)
            {
                Image = new DataImage("Temp2_-_fond", true)
            };

            template.Layout = new Layout();
            template.Fonts = fonts;
            template.Layout.AddTextBox(boite_coordonnées);
            template.Layout.AddTextBox(boite_diplomes);
            template.Layout.AddTextBox(boite_langues);
            template.Layout.AddTextBox(boite_nom);
            template.Layout.AddTextBox(boite_profession);
            template.Layout.AddTextBox(boite_competences);
            template.Layout.AddBackBox(fond);
            
            return template;
        }
    }
}
