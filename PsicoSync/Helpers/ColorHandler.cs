using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoSync.Helpers
{
    public class ColorHandler
    {
        public Color GetColor(string color)
        {
            //return (Color)Application.Current.Resources.MergedDictionaries.First().Keys[color];
            return Color.FromArgb(color);
        }

        public Color Primary => GetColor("#388E3C");
        public Color White => GetColor("#FFFFFF");
        public Color Black => GetColor("#000000");
    }
}
