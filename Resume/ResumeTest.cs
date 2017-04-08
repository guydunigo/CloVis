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
                new Data<string>("nom", "Clovis"),
                new Data<string>("Tel", "+33660606060"),
                new Data<string>("mail", "gloubiboulga@enib.fr")
            };

            var competences = new ElementList<Element>("competences")
            {
                new ElementList<Data>("info")
                {
                    new Data<string>("C", "C", 2),
                    new Data<string>("C++","C++",1),
                    new Data<string>("java","java",1)
                },
                new Data<string>("Business Process", "Business Process", 4)
            };

            var langues = new ElementList<Element>("langues")
            {
                new Data<string>("anglais", "anglais", 5),
                new Data<string>("allemand","allemand",2),
                new Data<string>("chinois","chinois",1.5),
            };

            var diplomes = new ElementList<Element>("diplomes")
            {
                new DataDated<string>("Flying Spaghetti Monster degree", "Flyer Spaghetti Monster degree", new DateTime(2017,12,24)),
                new DataDated<string>("bac","bac",new DateTime(1992,11,14)),
            };

            var fonts = new Fonts("Polices_cv")
            {
                new FontElement("Titre 1", "Tahoma", 14, new Color() { R = 0, G = 0, B = 255, A = 255 }), //ARGB 0 on voit rien, 255 opaque
                new FontElement("Corps", "Calibri", 11, new Color() { R = 155, G = 120, B = 12, A = 190 })
            };

            var boite_de_competences = new BoxText(10.5, 15, 60, 10.5, 6); //boite de texte qui contiendra les competences

            var fond = new BoxBackground(0, 10, 10, 21, 6)
            {
                Color = new Color() { A = 140, R = 255 } //verte un peu transparente
            }; //boite de fond

            boite_de_competences.Element = competences;

            //throw new NotImplementedException("ajouter au layout, puis au CV + ajout liste de polices");

            CV = new Resume()
            {
                Fonts = fonts
             
            };

            CV.Layout = new Layout();
            CV.Layout.AddBackBox(fond);
            CV.Layout.AddTextBox(boite_de_competences);

            return CV;
        }
    }
}
