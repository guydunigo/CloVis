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
            (Index.DataIndex["Profession"] as Data<string>).Value = "Ingénieur ENIB";
            (Index.DataIndex["Langue 1"] as Data<string>).Value = "Anglais";
            (Index.DataIndex["Langue 2"] as Data<string>).Value = "Allemand";
            (Index.DataIndex["Langue 3"] as Data<string>).Value = "Turc";


            Index.Root["Compétences"].Add(new ElementList<Element>("Informatique")
            {
                new Data<string>("C", 2),
                new Data<string>("C++",1),
                new Data<string>("Java",1)
            });
            Index.Root["Compétences"].Add(new Data<string>("Business Process", 4));
            Index.Root["Diplômes"].Add(new DataDated<string>("Flying Spaghetti Monster degree", new DateTime(2017, 12, 24), new DateTime(2017, 12, 24), "En $1(Y)$"));
            Index.Root["Diplômes"].Add(new DataDated<string>("Bac", new DateTime(1992, 11, 14), new DateTime(1992, 11, 14), "En $1(y)$"));
            Index.Root["Études"].Add(new DataDated<string>("Université de Ulm", new DateTime(1992, 11, 14), new DateTime(1998, 11, 14), "De $1(yy)$ à $2(yy)$"));
            Index.Root["Diplômes"].Add(Index.Find("Université de Ulm"));
            Index.Root["Expériences professionnelles"].Add(new DataTimeSpan<string>("Ingénieur narcoleptiques", new TimeSpan(300,0,0,0), "Pendant $(%d)$ jours"));

            var temp = new DataDated<string>("Life", new DateTime(2000, 01, 02), new DateTime(2222, 11, 22), "Jusque $2(y)$ et depuis $1(y)$");
            temp.AddCategory(Index.Find("Divers") as ElementList);
        }
    }
}
