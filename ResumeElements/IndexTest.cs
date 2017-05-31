using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeElements
{
    public static class IndexTest
    {
        public static void FillIndex()
        {
            (Index.DataIndex["Nom"] as Data<string>).Value = "Clovis";
            (Index.DataIndex["Téléphone"] as Data<string>).Value = "+33660606060";
            (Index.DataIndex["Mél"] as Data<string>).Value = "clovis@enib.fr";
            (Index.DataIndex["Profession"] as Data<string>).Value = "Roi des Francs";
            (Index.DataIndex["Langue 1"] as Data<string>).Value = "Anglais";
            (Index.DataIndex["Langue 2"] as Data<string>).Value = "Allemand";
            (Index.DataIndex["Langue 3"] as Data<string>).Value = "Chinois";
            (Index.DataIndex["Langue 4"] as Data<string>).Value = "Espéranto";



            Index.Root["Compétences"].Add(new ElementList<Element>("Informatique")
            {
                new Data<string>("C", 2),
                new Data<string>("C++",1),
                new Data<string>("Java",1)
            });

            Index.Root["Compétences"].Add(new ElementList<Element>("Diplomatie")
            {
                new Data<string>("Politique", 4),
                new Data<string>("Meurtre",3),
                new Data<string>("Mariage",2),
                new Data<string>("Ruse",1)
            });

            Index.Root["Divers"].Add(new ElementList<Element>("Mon Objectif")
            {
                new Data<string>("obj", "Je compte unir les Francs, conquérir le nord de la Loire, puis l'Est, et je vaincrai les Burgondes. Mais je ne cesserai pas, et je m'emparerai du Centre et du Sud Ouest de la gaule !")
            });


            Index.Root["Compétences"].Add(new Data<string>("Business Process", 4));
            Index.Root["Diplômes"].Add(new DataDated<string>("Flying Spaghetti Monster degree", new DateTime(2016, 12, 24), new DateTime(2017, 06, 1), "En $1(Y)$"));
            Index.Root["Diplômes"].Add(new DataDated<string>("Doctorat en Sociologie", new DateTime(1992, 11, 14), new DateTime(1992, 11, 14), "En $1(y)$"));
            Index.Root["Études"].Add(new DataDated<string>("L'école de la vie", new DateTime(466, 11, 14), new DateTime(1998, 08, 24), "De $1(yy)$ à $2(yy)$"));
            //Index.Root["Diplômes"].Add(Index.Find("Université de Ulm"));
            Index.Root["Expériences professionnelles"].Add(new DataTimeSpan<string>("Ingénieur narcoleptiques", new TimeSpan(300, 0, 0, 0), "Pendant $(%d)$ jours"));

            var temp = new DataDated<string>("Life", new DateTime(2000, 01, 02), new DateTime(2222, 11, 22), "Jusque $2(y)$ et depuis $1(y)$");
            temp.AddCategory(Index.Find("Divers") as ElementList);
        }
    }
}
