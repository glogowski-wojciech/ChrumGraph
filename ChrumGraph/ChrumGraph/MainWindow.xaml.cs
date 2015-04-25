using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Visual = ChrumGraph.Visual;

namespace ChrumGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Core core;

        /// <summary>
        /// Initialize window.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            core = new Core();
        }

        private void bOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();
            openDialog.DefaultExt = ".graph";
            openDialog.Title = "Open graph from a file";
            Nullable<bool> result = openDialog.ShowDialog();
            List<Vertex> vertices = core.Vertices;
            List<Edge> edges = core.Edges;
            if (result == true)
            {
                string filePath = openDialog.FileName;
                StreamReader reader = new StreamReader(filePath);
                string input = reader.ReadToEnd();
                reader.Close();

            }
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.DefaultExt = ".graph";
            saveDialog.Title = "Save graph to a file";
            saveDialog.ShowDialog();
            List<Vertex> vertices = core.Vertices;
            List<Edge> edges = core.Edges;

            if (saveDialog.FileName != "")
            {
                FileStream fs = (FileStream)saveDialog.OpenFile();
                UnicodeEncoding uniEncoding = new UnicodeEncoding();
                string line = vertices.Count.ToString() + " " + edges.Count.ToString() + Environment.NewLine;
                fs.Write(uniEncoding.GetBytes(line), 0, uniEncoding.GetByteCount(line));
                foreach (Edge edge in edges)
                {
                    line = edge.V1.ToString() + " " + edge.V2.ToString() + Environment.NewLine;
                    fs.Write(uniEncoding.GetBytes(line), 0, uniEncoding.GetByteCount(line));
                }
                fs.Close();
            }
        }
    }
}
