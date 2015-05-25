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

        private static Color selectVertexColor = Colors.Brown;
        private static Color selectEdgeColor = Color.FromRgb(102, 0, 51);
        private static Color selectRectColor = Colors.MistyRose;

        private static Color sidebarColor = Colors.CadetBlue;
        private static Color backgroundColor = Colors.LemonChiffon;
    }
}
