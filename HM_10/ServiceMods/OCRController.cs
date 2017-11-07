using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OnenoteOCR;
using System.IO;

namespace HM_10.ServiceMods
{
    class OCRController
    {
        public string getString(System.Drawing.Image img)
        {
            OnenoteOcrEngine ooe = new OnenoteOcrEngine();
            return ooe.Recognize(img);
        }
    }
}
