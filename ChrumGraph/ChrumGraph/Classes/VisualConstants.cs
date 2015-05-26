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

        private static Color vertexColor = Color.FromRgb(39, 171, 7);
        private static Color edgeColor = Color.FromRgb(15, 61, 26);

        private static Color pinnedColor = Colors.DeepSkyBlue;
        private static Color labelColor = Colors.Black;
        private static Color clickedColor = Colors.YellowGreen;

        private static Color selectVertexColor = Color.FromRgb(240, 0, 50);
        private static Color selectEdgeColor = Color.FromRgb(138, 10, 12);
        private static Color selectRectColor = Colors.MistyRose;

        public static Color sidebarColor = Color.FromRgb(165, 208, 182);
        public static Color backgroundColor = Colors.LemonChiffon;
    }
}
