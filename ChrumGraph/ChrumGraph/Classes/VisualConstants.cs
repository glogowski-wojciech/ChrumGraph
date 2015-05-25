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

        public static Color vertexColor = Color.FromRgb(39, 171, 7);
        public static Color edgeColor = Color.FromRgb(15, 61, 26);

        public static Color pinnedColor = Colors.DeepSkyBlue;
        public static Color labelColor = Colors.Black;
        public static Color clickedColor = Colors.YellowGreen;

        public static Color selectVertexColor = Colors.Brown;
        public static Color selectEdgeColor = Color.FromRgb(102, 0, 51);
        public static Color selectRectColor = Colors.MistyRose;

        public static Color sidebarColor = Colors.CadetBlue;
        public static Color backgroundColor = Colors.LemonChiffon;
    }
}
