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
            (Index.DataIndex["nom"] as Data<string>).Value = "Clovis";
            (Index.DataIndex["téléphone"] as Data<string>).Value = "+33660606060";
            (Index.DataIndex["mél"] as Data<string>).Value = "robert@enib.fr";

            Index.Root["compétences"].Add(new ElementList<Element>("informatique")
            {
                new Data<string>("c", "C", 2),
                new Data<string>("c++","C++",1),
                new Data<string>("java","java",1)
            });
            Index.Root["compétences"].Add(new Data<string>("business process", "Business Process", 4));
            Index.Root["langues"].Add(new Data<string>("anglais", "anglais", 5));
            Index.Root["langues"].Add(new Data<string>("allemand", "allemand", 5));
            Index.Root["langues"].Add(new Data<string>("chinois", "chinois", 5));
            Index.Root["diplômes"].Add(new DataDated<string>("Flying Spaghetti Monster degree", "Flyer Spaghetti Monster degree", new DateTime(2017, 12, 24), new DateTime(2017, 12, 24), "D"));
            Index.Root["diplômes"].Add(new DataDated<string>("bac", "bac", new DateTime(1992, 11, 14), new DateTime(1992, 11, 14), "D"));
        }
    }
}
