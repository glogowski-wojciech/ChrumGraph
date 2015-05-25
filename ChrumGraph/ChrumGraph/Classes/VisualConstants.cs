using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChrumGraph
{
    public partial class Visual
    {
        private static double verticeToEdgeRatio = 5;

        private static Color vertexColorConst = Color.FromRgb(39, 171, 7);
        private static Color edgeColorConst = Color.FromRgb(15, 61, 26);

        private static Color pinnedColorConst = Colors.DeepSkyBlue;
        private static Color labelColorConst = Colors.Black;
        private static Color clickedColorConst = Colors.YellowGreen;

        private static Color selectVertexColorConst = Colors.SaddleBrown;
        private static Color selectEdgeColorConst = Color.FromRgb(102, 0, 51);
        private static Color selectRectColorConst = Colors.MistyRose;

        private static Color sidebarColorConst = Colors.CadetBlue;
        private static Color backgroundColorV = Colors.LemonChiffon;
    }
}
