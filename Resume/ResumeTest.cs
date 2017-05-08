﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResumeElements;
using Windows.UI;

namespace Resume
{
    public static class ResumeTest
    {
        public static Resume GetResumeTest()
        {
            Resume CV;

            var coordonnees = new ElementList<Element>("Coordonnées")
            {
                new Data<string>("Nom", "Clovis", -1, "",true),
                new Data<string>("Téléphone", "+33660606060", -1, "", true),
                new Data<string>("Mél", "gloubiboulga@enib.fr", -1, "", true)
            };

            var competences = new ElementList<Element>("Compétences")
            {
                new ElementList<Element>("Informatique")
                {
                    new Data<string>("C", "C", 2, "",true),
                    new Data<string>("C++","C++",1, "",true),
                    new Data<string>("Java","java",1, "",true)
                },
                new Data<string>("Business process", "Business Process", 4, "",true)
            };

            var langues = new ElementList<Element>("Langues")
            {
                new Data<string>("Anglais", "anglais", 5, "",true),
                new Data<string>("Allemand","allemand",2, "",true),
                new Data<string>("Chinois","chinois",1.5, "",true),
            };

            var diplomes = new ElementList<Element>("Diplômes")
            {
                new DataDated<string>("Flying Spaghetti Monster degree", "Flying Spaghetti Monster degree", new DateTime(2017,12,24), new DateTime(2017,12,24), "Le $1(d)$",-1, "",true),
                new DataDated<string>("Bac","bac",new DateTime(1992,11,14), new DateTime(1992,11,14), "Le $1(D)$",-1, "",true)
            };

            var fonts = new Fonts("Polices_cv")
            {
                new FontElement("Tahoma", 7, new Color() { R = 0, G = 0, B = 255, A = 255 },true, true, true, true), //ARGB 0 on voit rien, 255 opaque
                new FontElement("Tahoma", 6, new Color() { R = 0, G = 0, B = 150, A = 255 },false, true, false, true),
                new FontElement("Tahoma", 5, new Color() { R = 100, G = 100, B = 200, A = 255 },true, false, false, false),
                new FontElement("Calibri", 5, new Color() { R = 70, G = 70, B = 200, A = 190 })
            };

            var boite_de_langues = new BoxText(105, 150, 60, 105, 72.5, 0, "Langues"); //boite de texte qui contiendra les competences
            var boite_de_coordonnees = new BoxText(0, 0, 0, 210, 40, 0, "Coordonnées");
            var boite_de_competences = new BoxText(0, 150, 60, 105, 60, 0, "Compétences");
            var boite_de_diplomes = new BoxText(105, 222.5, 60, 105, 72.5, 0, "Diplômes");
            
            var fond = new BoxBackground(0, 70, 100, 210, 60)
            {
                Color = new Color() { A = 140, G = 255 } //verte un peu transparente
            }; //boite de fond

			var fond2 = new BoxBackground(115, 150, 100, 160, 73,null,-40)
			{
				Color = new Color() { A = 190, B = 170, G = 255 } //turquoise ? transparent
			}; //boite de fond

            var fond3 = new BoxBackground(-15, -4, 100, 700, 65, null, 50)
			{
				Color = new Color() { A = 255, B = 200, G = 200, R = 255}
			};

			boite_de_competences.Element = competences;
            boite_de_coordonnees.Element = coordonnees;
            boite_de_langues.Element = langues;
            boite_de_diplomes.Element = diplomes;

            CV = new Resume()
            {
                Fonts = fonts,
                Name = "CV 1"
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
    }
}
