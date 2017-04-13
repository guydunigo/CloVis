using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResumeElements;



namespace Resume
{
    public static class TemplateTest
    {
        public static Template GetTemplate_1()
        {
			//j'ai essayé de faire des boites vides, sans grande conviction; dis-moi si c'est ce que tu voulais :s
            var template = new Template();
	    var coordonnees = new ElementList<Element>("coordonnees");
	    var competences = new ElementList<Element>("compétences");
	    var langues = new ElementList<Element>("langues");
   	    var diplomes = new ElementList<Element>("diplômes");
	    var fonts = new Fonts("Polices_cv");


            throw new NotImplementedException();

            return template;
        }
        public static Template GetTemplate_2()
        {
            var template = new Template();
            throw new NotImplementedException();

            return template;
        }
    }
}
