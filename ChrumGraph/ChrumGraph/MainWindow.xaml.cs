using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace ChrumGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Core core;
        private Visual visual;
        private bool addVertex = false;
        private Ellipse addedVertex;
        private string newLabel;

        /// <summary>
        /// Initialize window.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            visual = new Visual(this) 
            {
                Visible = true,
            };
            core = new Core(visual);
            visual.Core = core;
        }

        public string NewLabel
        {
            get { return newLabel; }
            set { newLabel = value; }
        }

        private void MenuButtonClicked(object sender, RoutedEventArgs e)
        {
            if(addVertex)
            {
                MainCanvas.Children.Remove(addedVertex);
                addVertex = false;
            }
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            MenuButtonClicked(sender, e);
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
            MenuButtonClicked(sender, e);
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.DefaultExt = ".graph";
            saveDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveDialog.Title = "Save graph to a file";
            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true && saveDialog.FileName != "")
                core.SaveGraph(saveDialog.FileName);
        }

        private void SaveProjectClick(object sender, RoutedEventArgs e)
        {
            MenuButtonClicked(sender, e);
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.DefaultExt = ".graph";
            saveDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveDialog.Title = "Save graph to a file";
            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true && saveDialog.FileName != "")
                core.SaveVisualGraph(saveDialog.FileName);
        }

        private void AddVertex(object sender, RoutedEventArgs e)
        {
            MenuButtonClicked(sender, e);
            var addVertexForm = new AddVertexForm(core);
            var result = addVertexForm.ShowDialog();
            addVertexForm.Close();
            if (result == System.Windows.Forms.DialogResult.Cancel)
                return;
            newLabel = addVertexForm.GetNewLabel();
            addVertex = true;
            addedVertex = core.Visual.getVisualVertex();
            MainCanvas.Children.Add(addedVertex);
            Canvas.SetLeft(addedVertex, Mouse.GetPosition(MainCanvas).X - visual.VertexSize / 2);
            Canvas.SetTop(addedVertex, Mouse.GetPosition(MainCanvas).Y - visual.VertexSize / 2);
        }

        private void AddEdge(object sender, RoutedEventArgs e)
        {
            if (addVertex)
                MainCanvas.Children.Remove(addedVertex);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if(addVertex)
            {
                double x = e.GetPosition(MainCanvas).X - visual.VertexSize / 2;
                double y = e.GetPosition(MainCanvas).Y - visual.VertexSize / 2;
                Canvas.SetLeft(addedVertex, x);
                Canvas.SetTop(addedVertex, y);
                if (Mouse.GetPosition(this).Y <= MainMenu.Height)
                    addedVertex.Visibility = System.Windows.Visibility.Hidden;
                else
                    addedVertex.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            if(addVertex)
            {
                addVertex = false;
                if (Mouse.GetPosition(this).Y <= MainMenu.Height + visual.VertexSize / 2)
                    MainCanvas.Children.Remove(addedVertex);
                else
                {
                    System.Diagnostics.Debug.WriteLine(visual.ViewWindow.Width);
                    System.Diagnostics.Debug.WriteLine(visual.ViewWindow.Height);
                    System.Diagnostics.Debug.WriteLine(e.GetPosition(MainCanvas).X);
                    System.Diagnostics.Debug.WriteLine(e.GetPosition(MainCanvas).Y);

                    visual.ViewWindow.Static = true;
                    Point corePosition = visual.ViewWindow.VisualToCorePosition(e.GetPosition(MainCanvas));
                    core.CreateVertex(corePosition.X, corePosition.Y, newLabel);
                    MainCanvas.Children.Remove(addedVertex);
                }
            }
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            visual.ViewWindow.Static = false;
        }
    }
}
