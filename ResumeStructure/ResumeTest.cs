﻿using ResumeElements;
using System;
using Windows.UI;

namespace ResumeStructure
{
    public static class ResumeTest
    {
        public static Resume GetResumeTest()
        {
            Resume CV;

            var coordonnees = new ElementList("Coordonnées")
            {
                new DataText("Nom", "Clovis", -1),
                new DataText("Téléphone", "+33660606060", -1),
                new DataText("Mél", "RoiDesFranc@enib.fr", -1)
            };

            var competences = new ElementList("Compétences")
            {
                new ElementList("Informatique")
                {
                    new DataText("C", "C", 2),
                    new DataText("C++","C++",1),
                    new DataText("Java","java",1)
                },
                new DataText("Business process", "Business Process", 4)
            };

            var langues = new ElementList("Langues")
            {
                new DataText("Anglais", "anglais", 5),
                new DataText("Allemand","allemand",2),
                new DataText("Chinois","chinois",1.5),
            };

            var diplomes = new ElementList("Diplômes")
            {
                new DataTextDated("Flying Spaghetti Monster degree", "Flying Spaghetti Monster degree", new DateTime(2017,12,24), new DateTime(2017,12,24), "Le $1(d)$",-1),
                new DataTextDated("Bac","bac",new DateTime(1992,11,14), new DateTime(1992,11,14), "Le $1(D)$",-1)
            };

            var fonts = new Fonts()
            {
                new FontElement("Tahoma", 7, new Color() { R = 0, G = 0, B = 0, A = 255 },false, true, false, false), //ARGB 0 on voit rien, 255 opaque
                new FontElement("Tahoma", 6, new Color() { R = 0, G = 0, B = 0, A = 255 },false, false, false, false),
                new FontElement("Tahoma", 5, new Color() { R = 0, G = 0, B = 0, A = 255 },false, false, false, false),
                new FontElement("Calibri", 5, new Color() { R = 0, G = 0, B = 0, A = 190 })
            };

            var boite_de_langues = new BoxText(10, 230, 60, 105, 72.5, 0, "Langues"); //boite de texte qui contiendra les competences
            var boite_de_coordonnees = new BoxText(110, 10, 0, 210, 40, 0, "Coordonnées");
            var boite_de_competences = new BoxText(10, 150, 60, 105, 60, 0, "Compétences");
            var boite_de_diplomes = new BoxText(10, 100, 60, 105, 72.5, 0, "Diplômes");

            var fond = new BoxBackground(0, 70, 10, 210, 60)
            {
                Fill = new Color() { A = 255, R = 196, G = 215, B = 237 } //verte un peu transparente
            }; //boite de fond

            var fond2 = new BoxBackground(115, 150, 30, 160, 73, -40)
            {
                Fill = new Color() { A = 255, R = 171, B = 226, G = 200 } //turquoise ? transparent
            }; //boite de fond

            var fond3 = new BoxBackground(-15, -4, 50, 700, 65, 50)
            {
                Fill = new Color() { A = 255, B = 129, G = 93, R = 55 }
            };

            boite_de_competences.Element = competences;
            boite_de_coordonnees.Element = coordonnees;
            boite_de_langues.Element = langues;
            boite_de_diplomes.Element = diplomes;

            CV = new Resume("CV 1")
            {
                Fonts = fonts
            };

            CV.Layout = new Layout();
            CV.Layout.AddBackBox(fond);
            CV.Layout.AddBackBox(fond2);
            CV.Layout.AddBackBox(fond3);
            CV.Layout.AddTextBox(boite_de_competences);
            CV.Layout.AddTextBox(boite_de_coordonnees);
            CV.Layout.AddTextBox(boite_de_langues);
            CV.Layout.AddTextBox(boite_de_diplomes);

            return CV;
        }

        public static Resume GetResumeTest2()
        {
            Resume CV;

            var nom = new DataText("Nom", "Clovis", -1);
            var titre = new DataText("Titre", "Roi des Francs", -1);
            var coordonnees = new ElementList("Coordonnées")
            {
                new DataText("Téléphone", "+33660606060", -1),
                new DataText("Mél", "clovis@enib.fr", -1),
                new DataText("Adresse", "17 rue de la Réussite\n75012 Paris", -1)
            };

            var objectif = new ElementList("Objectif")
            {
                new ElementList("Mon Objectif")
                {
                    new DataText("obj", "Je compte unir les Francs, conquérir le nord de la Loire, puis l'Est, et je vaincrai les Burgondes. Mais je ne cesserai pas, et je m'emparerai du Centre et du Sud Ouest de la gaule !", -1)
                }
            };

            var competences = new ElementList("Compétences")
            {
                new ElementList("Diplomatie")
                {
                    new DataText("Politique", "Politique", 4),
                    new DataText("Assassinat","Assassinat",3),
                    new DataText("Mariage","Mariage",2),
                    new DataText("Soudoiement","Soudoiement",2),
                },

            };

            var langues = new ElementList("Langues")
            {
                new DataText("Francique", "Francique", 5),
                new DataText("Français moderne","Français moderne",1),
                new DataText("Allemand moderne","Allemand moderne",1),
            };

            var diplomes = new ElementList("Dynastie")
            {
                new DataTextDated("Mérovingien", "Mérovingien", new DateTime(466,1,1), default(DateTimeOffset), "Depuis $1(yyy)$",-1),

            };

            var fonts = new Fonts()
            {
                new FontElement("Garamond", 5, new Color() { R = 0, G = 0, B = 255, A = 255 },false, false, false, true), //ARGB 0 on voit rien, 255 opaque
                new FontElement("Garamond", 5, new Color() { R = 0, G = 0, B = 150, A = 255 },false, true, false, false),
                new FontElement("Garamond", 5, new Color() { R = 100, G = 100, B = 200, A = 255 },true, false, false, false),
                new FontElement("Calibri", 5, new Color() { R = 70, G = 70, B = 200, A = 190 })
            };

            var fonts1 = new Fonts(Windows.UI.Xaml.TextAlignment.Justify)
            {
                new FontElement("Garamond", 5, new Color() { R = 168, G = 215, B = 203, A = 255 },false, false, false, false),
            };

            var boite_de_nom = new BoxText(90, 15, 60, 40, 20, 0, "nom");
            var boite_de_titre = new BoxText(80, 28, 60, 40, 10, 0, "titre");
            var boite_d_obj = new BoxText(15, 50, 60, 70, 40, 0, "obj", fonts1);

            var boite_de_coordonnees = new BoxText(140, 50, 60, 80, 60, 0, "Coordonnées");
            var boite_de_competences = new BoxText(20, 95, 60, 40, 50, 0, "Diplomatie");
            var boite_de_langues = new BoxText(140, 95, 60, 80, 50, 0, "Langues");
            var boite_de_diplomes = new BoxText(15, 160, 60, 150, 15, 0, "Dynastie");

            var fond = new BoxBackground(0, 90, 10, 210, 50)
            {
                Fill = new Color() { A = 140, G = 255 } //verte un peu transparente
            }; //boite de fond

            var img = new DataImage("CV 2_-_profil");
            var photo = new BoxBackground(105 - 50 / 2, 90 + 5, 50, 40, 40, 0, BoxBackgroundShape.Ellipse)
            {
                Image = img
            };

            boite_de_nom.Element = nom;
            boite_de_titre.Element = titre;
            boite_d_obj.Element = objectif;
            boite_de_competences.Element = competences;
            boite_de_coordonnees.Element = coordonnees;
            boite_de_langues.Element = langues;
            boite_de_diplomes.Element = diplomes;

            CV = new Resume("CV 2")
            {
                Fonts = fonts
            };

            CV.Layout = new Layout();
            CV.Layout.AddBackBox(fond);
            CV.Layout.AddBackBox(photo);

            CV.Layout.AddTextBox(boite_d_obj);
            CV.Layout.AddTextBox(boite_de_nom);
            CV.Layout.AddTextBox(boite_de_titre);
            CV.Layout.AddTextBox(boite_de_competences);
            CV.Layout.AddTextBox(boite_de_coordonnees);
            CV.Layout.AddTextBox(boite_de_langues);
            CV.Layout.AddTextBox(boite_de_diplomes);

            return CV;
        }
    }
}
