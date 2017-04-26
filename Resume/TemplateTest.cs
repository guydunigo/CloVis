using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResumeElements;
using Windows.UI;



namespace Resume
{
    public static class TemplateTest
    {
        public static Template GetTemplate_1()
        {
			
            var template = new Template();

            var boite_competences = new BoxText(100, 80, 50, 105, 75, 0, "compétences");
            var boite_contact = new BoxText(30, 40, 20, 60, 70, 10, "Contact");
            var boite_centres_dinteret = new BoxText(20, 200, 10, 150, 50, 0, "Centres d'intérêt");
            var boite_langues = new BoxText(20, 200, 10, 100, 50, 0, "langues");
            var fond = new BoxBackground(30, 40, 10, 100, 100, null, 20)
            {
                Color = new Color() { A = 50, B = 120, G = 130 } // fond peu transparent qui se trouve sous la boite contact
            };

            template.Layout = new Layout();
            template.Layout.AddTextBox(boite_langues);
            template.Layout.AddTextBox(boite_competences);
            template.Layout.AddTextBox(boite_centres_dinteret);
            template.Layout.AddBackBox(fond);

            return template;
        }
        public static Template GetTemplate_2()
        {
            var template = new Template();
            var boite_diplomes = new BoxText(20, 120, 35, 100, 75, 0, "diplômes");
            var boite_contact = new BoxText(100, 40, 20, 60, 70, 10, "Contact");
            var boite_langues = new BoxText(20, 200, 10, 100, 50, 0, "langues");
            var fond = new BoxBackground(100, 40, 50, 30, 50, null, 20)
            { 
                Color = new Color() { A = 200, R = 120, G = 130 } // fond plutot transparent qui se trouve sur la boite contact
            };


            template.Layout = new Layout();
            template.Layout.AddTextBox(boite_contact);
            template.Layout.AddTextBox(boite_diplomes);
            template.Layout.AddTextBox(boite_langues);
            template.Layout.AddBackBox(fond);


            return template;
        }
    }
}
