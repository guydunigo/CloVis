using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resume;
using CloVis.Controls;
using Windows.UI.Xaml;

namespace CloVis
{
    public static class ExportToPdf
    {

        public static bool ExportCV(Resume.Resume cv)
        {
            var page = new Resume_Preview() { Resume = cv };
            
            return false;
        }
        
    }
}

