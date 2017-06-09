using ResumeElements;
using Windows.UI;

namespace ResumeStructure
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
            var template = new Template("Green Peace");

            var fonts = new Fonts("Polices_cv")
            {
                new FontElement("Gadugi", 6,  new Color() { R = 120, G = 0, B = 0, A = 200 }, false, false, false, false)   //italic, gras, souligné
                
            };

            var fonts_titre = new Fonts("Polices_cv")
            {
                new FontElement("Gadugi", 10,  new Color() { R = 120, G = 0, B = 0, A = 200 }, false, false, false, false)   //italic, gras, souligné
            };

            var fonts_petit = new Fonts("Polices_cv")
            {
                new FontElement("Gadugi", 5,  new Color() { R = 120, G = 0, B = 0, A = 200 }, false, false, false, false)   //italic, gras, souligné
            };

            var fonts_petit_droit = new Fonts("Polices_cv", Windows.UI.Xaml.TextAlignment.Right)
            {
                new FontElement("Gadugi", 5,  new Color() { R = 120, G = 0, B = 0, A = 200 }, false, false, false, false)   //italic, gras, souligné
            };

            var fonts_tres_petit = new Fonts("Polices_cv", Windows.UI.Xaml.TextAlignment.Justify)
            {
                new FontElement("Gadugi", 4,  new Color() { R = 120, G = 0, B = 0, A = 200 }, false, false, false, false)   //italic, gras, souligné
            };

            var fonts_langues = new Fonts("Polices_cv")
            {
                new FontElement("Gadugi", 6.5,  new Color() { R = 120, G = 0, B = 0, A = 200 }, false, false, false, true),
                new FontElement("Gadugi", 1,  new Color() { R = 120, G = 0, B = 0, A = 0 }, false, false, false, false)
            };

            var fonts_corps = new Fonts("Polices_cv", Windows.UI.Xaml.TextAlignment.Right)
            {
                new FontElement("Gadugi", 6.5,  new Color() { R = 120, G = 0, B = 0, A = 0 }, false, false, false, true),
                new FontElement("Gadugi", 5,  new Color() { R = 120, G = 0, B = 0, A = 200 }, false, false, false, false)
            };

            var fonts_normal = new Fonts("Polices_cv")
            {
                new FontElement("Gadugi", 6.5,  new Color() { R = 120, G = 0, B = 0, A = 200 }, false, false, false, false),
                new FontElement("Gadugi", 5,  new Color() { R = 120, G = 0, B = 0, A = 200 }, false, false, false, false)
            };

            var boite_diplomes = new BoxText(8, 170, 20, 193, 64, 0, "Diplômes", fonts_normal);
            var boite_études = new BoxText(8, 200, 20, 193, 64, 0, "Études", fonts_normal);
            var boite_pro = new BoxText(8, 220, 20, 193, 64, 0, "Expériences professionnelles", fonts_normal);

            var boite_coordonnées2 = new BoxText(134, 45, 20, 67, 30, 0, "Coordonnées", fonts_corps);
            var boite_coordonnées = new BoxText(157, 38, 20, 67, 52, 0, "Coordonnées", fonts_langues);

            //var boite_coordonnées1 = new BoxText(134, 52, 20, 67, 52, 0, "Nom", fonts_petit_droit);
            //var boite_coordonnées2 = new BoxText(134, 59, 20, 67, 52, 0, "Téléphone", fonts_petit_droit);
            //var boite_coordonnées3= new BoxText(134, 66, 20, 67, 52, 0, "Mél", fonts_petit_droit);

            var boite_langues = new BoxText(172, 90, 10, 49, 28, 0, "Langues", fonts_langues);
            var boite_langue_1 = new BoxText(144, 108, 10, 29, 28, 0, "Langue 1", fonts_petit);
            var boite_langue_2 = new BoxText(144, 116, 10, 29, 28, 0, "Langue 2", fonts_petit);
            var boite_langue_3 = new BoxText(144, 124, 10, 29, 28, 0, "Langue 3", fonts_petit);
            var boite_langue_4 = new BoxText(144, 132, 10, 29, 28, 0, "Langue 4", fonts_petit);

            var boite_d_obj = new BoxText(8, 38, 60, 70, 40, 0, "Mon Objectif", fonts_langues);
            var boite_d_obj2 = new BoxText(7, 52, 60, 70, 40, 0, "obj", fonts_tres_petit);


            var boite_compétence_1 = new BoxText(7, 107, 10, 29, 28, 0, "Politique", fonts_petit);
            var boite_compétence_2 = new BoxText(7, 115, 10, 29, 28, 0, "Meurtre", fonts_petit);
            var boite_compétence_3 = new BoxText(7, 123, 10, 29, 28, 0, "Mariage", fonts_petit);
            var boite_compétence_4 = new BoxText(7, 131, 10, 29, 28, 0, "Ruse", fonts_petit);

            var boite_nom = new BoxText(88, 12, 20, 81, 15, 0, "Nom", fonts_titre);
            var boite_profession = new BoxText(84, 33, 0, 81, 20, 0, "Profession");
            var boite_competences = new BoxText(7, 90, 0, 50, 64, 0, "Compétences", fonts_langues);

            var img = new Deprecated_DataImage("profile", true);
            var boite_photo = new BoxBackground(105 - 50 / 2, 90 + 5, 50, 40, 40, 0, BoxBackgroundShape.Ellipse)
            {
                Image = img
            };

            var fond = new BoxBackground(0, 0, 0, 210, 297, 0)
            {
                Image = new Deprecated_DataImage("Temp3_-_fond", true)
            };

            template.Layout = new Layout();
            template.Fonts = fonts;
            template.Layout.AddTextBox(boite_coordonnées2);
            template.Layout.AddTextBox(boite_coordonnées);

            //template.Layout.AddTextBox(boite_coordonnées1);
            //template.Layout.AddTextBox(boite_coordonnées2);
            //template.Layout.AddTextBox(boite_coordonnées3);
            template.Layout.AddTextBox(boite_diplomes);
            template.Layout.AddTextBox(boite_études);
            template.Layout.AddTextBox(boite_pro);
            template.Layout.AddTextBox(boite_langues);
            template.Layout.AddTextBox(boite_langue_1);
            template.Layout.AddTextBox(boite_langue_2);
            template.Layout.AddTextBox(boite_langue_3);
            template.Layout.AddTextBox(boite_langue_4);
            template.Layout.AddTextBox(boite_compétence_1);
            template.Layout.AddTextBox(boite_compétence_2);
            template.Layout.AddTextBox(boite_compétence_3);
            template.Layout.AddTextBox(boite_compétence_4);
            template.Layout.AddTextBox(boite_nom);
            template.Layout.AddTextBox(boite_d_obj);
            template.Layout.AddTextBox(boite_d_obj2);
            template.Layout.AddTextBox(boite_profession);
            template.Layout.AddTextBox(boite_competences);
            template.Layout.AddBackBox(fond);
            template.Layout.AddBackBox(boite_photo);

            return template;
        }


        public static Template GetTemplate_4()
        {
            var template = new Template("Black & White");

            var fonts = new Fonts("Polices_cv")
            {
                new FontElement("Colibri", 6,  new Color() { R = 0, G = 0, B = 0, A = 200 }, false, false, false, false)

            };

            var fonts_titre = new Fonts("Polices_cv")
            {
                new FontElement("Colibri", 16,  new Color() { R = 0, G = 0, B = 0, A = 200 }, false, false, false, false)
            };

            var fonts_petit = new Fonts("Polices_cv")
            {
                new FontElement("Colibri", 6,  new Color() { R = 0, G = 0, B = 0, A = 200 }, false, false, false, false)
            };

            var fonts_langues = new Fonts("Polices_cv")
            {
                new FontElement("Colibri", 5,  new Color() { R = 0, G = 0, B = 0, A = 200 }, false, false, false, false)
            };

            var fonts_competences = new Fonts("Polices_cv")
            {
                new FontElement("Colibri", 7,  new Color() { R = 0, G = 0, B = 0, A = 200 }, false, false, false, false),
                new FontElement("Colibri", 5,  new Color() { R = 0, G = 0, B = 0, A = 200 }, false, false, false, false),
                new FontElement("Colibri", 4,  new Color() { R = 0, G = 0, B = 0, A = 200 }, false, false, false, false)
            };

            var boite_exp = new BoxText(57, 229, 20, 82, 32, 0, "Expériences professionnelles");
            var boite_coordonnées = new BoxText(123, 125, 20, 67, 52, 0, "Coordonnées");
            var boite_competences = new BoxText(57, 229, 20, 82, 100, 0, "Diplômes", fonts_competences);
            var boite_tel = new BoxText(123, 125, 20, 40, 10, 0, "Téléphone", fonts_langues);
            var boite_mel = new BoxText(67, 136, 20, 46, 10, 0, "Mél", fonts_langues);
            var boite_adresse = new BoxText(67, 125, 20, 45, 6, 0, "Adresse", fonts_langues);
            var boite_nom = new BoxText(57, 24, 40, 60, 30, 0, "Nom", fonts_titre);
            var boite_profession = new BoxText(57, 42, 0, 42, 8, 0, "Profession");
            var boite_diplomes = new BoxText(58, 164, 0, 82, 100, 0, "Compétences", fonts_competences);
            var fond = new BoxBackground(0, 0, 0, 210, 297, 0)
            {
                Image = new Deprecated_DataImage("Temp4_-_fond", true)
            };

            template.Layout = new Layout();
            template.Fonts = fonts;
            template.Layout.AddTextBox(boite_nom);
            template.Layout.AddTextBox(boite_tel);
            template.Layout.AddTextBox(boite_mel);
            template.Layout.AddTextBox(boite_adresse);
            template.Layout.AddTextBox(boite_diplomes);
            template.Layout.AddTextBox(boite_profession);
            template.Layout.AddTextBox(boite_competences);
            template.Layout.AddBackBox(fond);

            return template;
        }
    }
}
