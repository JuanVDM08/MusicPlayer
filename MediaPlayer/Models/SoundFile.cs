using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.UI.Xaml.Media.Imaging;

namespace MediaPlayer.Models
{
    public class SoundFile
    {
        public string Name { get; set; }
        public BitmapImage Image { get; set; }
        public PdfPage Page { get; set; }

    }
}
