﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resume;
using ResumeElements;

namespace CloVis
{
    static class ResumeTest
    {
        static Resume.Resume getResumeTest()
        {
            Resume.Resume CV;

            var nom = new Data<string>("nom", "Clovis");
            var tel = new Data<string>("Tel", "+33");
			var mail = new Data<string>("mail", "gloubiboulga@enib.fr");

			var competences = new ElementList("competences");
			var info = new ElementList("info")
			{
				new Data<string>("C", "C", 2),
				new Data<string>("C++","C++",1),
				new Data<string>("java","java",1),
			};
			competences.Add(info);

			var management = new Data<string>("Business Process", "Business Process", 4);
			competences.Add(management);

			var langues = new ElementList("langues")
			{
				new Data<string>("anglais", "anglais", 5),
				new Data<string>("allemand","allemand",2),
				new Data<string>("chinois","chinois",1.5),
			};

			var diplomes = new ElementList("diplomes")
			{
				new DataDated<string>("Flying Spaghetti Monster degree", "Flyer Spaghetti Monster degree", new DateTime(2017,12,24)),
				new DataDated<string>("bac","bac",new DateTime(1992,11,14)),
			};

			var Titre1 = new FontElement("Tahoma", 14, blue); //ARGB 0 on voit rien, 255 opaque
			var corps = new FontElement("Calibri", 11, new Color() { R = 155, G = 120, B = 12, A = 190 });



			new Font("Polices_cv");
			Font.Add("Titre 1", Titre1); //Font.fonts.Add : classe Font(s) à modifier
			Font.Add("corps", corps);


            return CV;
        }
    }
}
