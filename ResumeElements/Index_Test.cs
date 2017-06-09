using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeElements
{
    public static class Index_Test
    {
        public static void FillIndex(Index index)
        {
            (index.DataIndex["Nom"] as DataText).Value = "Clovis";
            (index.DataIndex["Téléphone"] as DataText).Value = "+33660606060";
            (index.DataIndex["Mél"] as DataText).Value = "clovis@enib.fr";
            (index.DataIndex["Profession"] as DataText).Value = "Roi des Francs";
            (index.DataIndex["Langue 1"] as DataText).Value = "Anglais";
            (index.DataIndex["Langue 2"] as DataText).Value = "Allemand";
            (index.DataIndex["Langue 3"] as DataText).Value = "Chinois";
            (index.DataIndex["Langue 4"] as DataText).Value = "Espéranto";
            
            (index.Root["Compétences"] as ElementList)?.Add(new ElementList("Informatique")
            {
                new DataText(index, "C", 2),
                new DataText(index, "C++",1),
                new DataText(index, "Java",1)
            });

            (index.Root["Compétences"] as ElementList)?.Add(new ElementList("Diplomatie")
            {
                new DataText(index, "Politique", 4),
                new DataText(index, "Meurtre",3),
                new DataText(index, "Mariage",2),
                new DataText(index, "Ruse",1)
            });

            (index.Root["Divers"] as ElementList)?.Add(new ElementList("Mon Objectif")
            {
                new DataText("obj", "Je compte unir les Francs, conquérir le nord de la Loire, puis l'Est, et je vaincrai les Burgondes. Mais je ne cesserai pas, et je m'emparerai du Centre et du Sud Ouest de la gaule !")
            });

            (index.Root["Compétences"] as ElementList)?.Add(new DataText(index, "Business Process", 4));
            (index.Root["Diplômes"] as ElementList)?.Add(new DataTextDated(index, "Flying Spaghetti Monster degree", new DateTime(2016, 12, 24), new DateTime(2017, 06, 1), "En $1(Y)$"));
            (index.Root["Diplômes"] as ElementList)?.Add(new DataTextDated(index, "Doctorat en Sociologie", new DateTime(1992, 11, 14), new DateTime(1992, 11, 14), "En $1(y)$"));
            (index.Root["Études"] as ElementList)?.Add(new DataTextDated(index, "L'école de la vie", new DateTime(466, 11, 14), new DateTime(1998, 08, 24), "De $1(yy)$ à $2(yy)$"));
            (index.Root["Expériences professionnelles"] as ElementList)?.Add(new Deprecated_DataTimeSpan<string>("Ingénieur narcoleptiques", new TimeSpan(300, 0, 0, 0), "Pendant $(%d)$ jours"));

            var temp = new DataTextDated(index, "Life", new DateTime(2000, 01, 02), new DateTime(2222, 11, 22), "Jusque $2(y)$ et depuis $1(y)$");
            temp.AddCategory(index.Find("Divers") as ElementList);
        }
    }
}
