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
        private Core core;
        private bool addVertex = false;
        private Ellipse addedVertex;
        /// <summary>
        /// Initialize window.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            core = new Core(MainCanvas);
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();
            openDialog.DefaultExt = ".graph";
            openDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openDialog.Title = "Open graph from a file";
            Nullable<bool> result = openDialog.ShowDialog();

            if (result == true)
               core.LoadFromFile(openDialog.FileName);
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.DefaultExt = ".graph";
            saveDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveDialog.Title = "Save graph to a file";
            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true && saveDialog.FileName != "")
                core.SaveGraph(saveDialog.FileName);
        }

        private void AddVertex(object sender, RoutedEventArgs e)
        {
            addVertex = true;
            addedVertex = core.Visual.getVisualVertex();
            MainCanvas.Children.Add(addedVertex);
        }

        private void AddEdge(object sender, RoutedEventArgs e)
        {

        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if(addVertex)
            {
                double x = e.GetPosition(MainCanvas).X - 12.5;
                double y = e.GetPosition(MainCanvas).Y - 12.5;
                Canvas.SetLeft(addedVertex, x);
                Canvas.SetTop(addedVertex, y);
            }
        }

    }
}
