using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeElements
{
    public static class Deprecated_Index_Test
    {
        public static void FillIndex()
        {
            (Deprecated_Index.DataIndex["Nom"] as Deprecated_Data<string>).Value = "Clovis";
            (Deprecated_Index.DataIndex["Téléphone"] as Deprecated_Data<string>).Value = "+33660606060";
            (Deprecated_Index.DataIndex["Mél"] as Deprecated_Data<string>).Value = "clovis@enib.fr";
            (Deprecated_Index.DataIndex["Profession"] as Deprecated_Data<string>).Value = "Roi des Francs";
            (Deprecated_Index.DataIndex["Langue 1"] as Deprecated_Data<string>).Value = "Anglais";
            (Deprecated_Index.DataIndex["Langue 2"] as Deprecated_Data<string>).Value = "Allemand";
            (Deprecated_Index.DataIndex["Langue 3"] as Deprecated_Data<string>).Value = "Chinois";
            (Deprecated_Index.DataIndex["Langue 4"] as Deprecated_Data<string>).Value = "Espéranto";
            
            Deprecated_Index.Root["Compétences"].Add(new Deprecated_ElementList<Element>("Informatique")
            {
                new Deprecated_Data<string>("C", 2),
                new Deprecated_Data<string>("C++",1),
                new Deprecated_Data<string>("Java",1)
            });

            Deprecated_Index.Root["Compétences"].Add(new Deprecated_ElementList<Element>("Diplomatie")
            {
                new Deprecated_Data<string>("Politique", 4),
                new Deprecated_Data<string>("Meurtre",3),
                new Deprecated_Data<string>("Mariage",2),
                new Deprecated_Data<string>("Ruse",1)
            });

            Deprecated_Index.Root["Divers"].Add(new Deprecated_ElementList<Element>("Mon Objectif")
            {
                new Deprecated_Data<string>("obj", "Je compte unir les Francs, conquérir le nord de la Loire, puis l'Est, et je vaincrai les Burgondes. Mais je ne cesserai pas, et je m'emparerai du Centre et du Sud Ouest de la gaule !")
            });

            Deprecated_Index.Root["Compétences"].Add(new Deprecated_Data<string>("Business Process", 4));
            Deprecated_Index.Root["Diplômes"].Add(new Deprecated_DataDated<string>("Flying Spaghetti Monster degree", new DateTime(2016, 12, 24), new DateTime(2017, 06, 1), "En $1(Y)$"));
            Deprecated_Index.Root["Diplômes"].Add(new Deprecated_DataDated<string>("Doctorat en Sociologie", new DateTime(1992, 11, 14), new DateTime(1992, 11, 14), "En $1(y)$"));
            Deprecated_Index.Root["Études"].Add(new Deprecated_DataDated<string>("L'école de la vie", new DateTime(466, 11, 14), new DateTime(1998, 08, 24), "De $1(yy)$ à $2(yy)$"));
            //Index.Root["Diplômes"].Add(Index.Find("Université de Ulm"));
            Deprecated_Index.Root["Expériences professionnelles"].Add(new Deprecated_DataTimeSpan<string>("Ingénieur narcoleptiques", new TimeSpan(300, 0, 0, 0), "Pendant $(%d)$ jours"));

            var temp = new Deprecated_DataDated<string>("Life", new DateTime(2000, 01, 02), new DateTime(2222, 11, 22), "Jusque $2(y)$ et depuis $1(y)$");
            temp.AddCategory(Deprecated_Index.Find("Divers") as Deprecated_ElementList);
        }
    }
}
