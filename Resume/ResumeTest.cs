using System;
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

            var coordonnees = new ElementList<Element>("coordonnees")
            {
                new Data<string>("nom", "Clovis", -1, "",true),
                new Data<string>("téléphone", "+33660606060", -1, "", true),
                new Data<string>("mél", "gloubiboulga@enib.fr", -1, "", true)
            };

            var competences = new ElementList<Element>("compétences")
            {
                new ElementList<Data>("informatique")
                {
                    new Data<string>("c", "C", 2, "",true),
                    new Data<string>("c++","C++",1, "",true),
                    new Data<string>("java","java",1, "",true)
                },
                new Data<string>("business process", "Business Process", 4, "",true)
            };

            var langues = new ElementList<Element>("langues")
            {
                new Data<string>("anglais", "anglais", 5, "",true),
                new Data<string>("allemand","allemand",2, "",true),
                new Data<string>("chinois","chinois",1.5, "",true),
            };

            var diplomes = new ElementList<Element>("diplômes")
            {
                new DataDated<string>("Flying Spaghetti Monster degree", "Flyer Spaghetti Monster degree", new DateTime(2017,12,24), new DateTime(2017,12,24), "D",-1, "",true),
                new DataDated<string>("bac","bac",new DateTime(1992,11,14), new DateTime(1992,11,14), "D",-1, "",true)
            };

            var fonts = new Fonts("Polices_cv")
            {
                new FontElement("Titre 1", "Tahoma", 14, new Color() { R = 0, G = 0, B = 255, A = 255 }), //ARGB 0 on voit rien, 255 opaque
                new FontElement("Corps", "Calibri", 11, new Color() { R = 155, G = 120, B = 12, A = 190 })
            };

            var boite_de_competences = new BoxText(105, 150, 60, 105, 60, 0, "Compétences"); //boite de texte qui contiendra les competences

            var fond = new BoxBackground(0, 100, 100, 210, 60)
            {
                Color = new Color() { A = 140, R = 255 } //verte un peu transparente
            }; //boite de fond

            boite_de_competences.Element = competences;

            //throw new NotImplementedException("ajouter au layout, puis au CV + ajout liste de polices");

            CV = new Resume()
            {
                Fonts = fonts,
                Name = "CV 1"
            };
            
            CV.Layout = new Layout();
            CV.Layout.AddBackBox(fond);
            CV.Layout.AddTextBox(boite_de_competences);
            
            return CV;
        }
    }
}
