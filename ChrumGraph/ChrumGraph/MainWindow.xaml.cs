using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;

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
            ForcesMultiplierTextBlock.Text = Convert.ToString(1.0);
            VertexForceTextBlock.Text = Convert.ToString(core.Physics.VertexForceParam);
            EdgeForceTextBlock.Text = Convert.ToString(core.Physics.EdgeForceParam);
            EdgeLengthTextBlock.Text = Convert.ToString(core.Physics.EdgeLength);
            FrictionTextBlock.Text = Convert.ToString(core.Physics.FrictionParam);
            this.KeyDown += keyHandler;
            MainCanvas.Background = new SolidColorBrush(Visual.backgroundColor);
            this.Background = new SolidColorBrush(Visual.sidebarColor);
        }

        private void keyHandler(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
                visual.deleteSelected();

            if(e.Key == Key.Escape)
                visual.cleanSelectedVertices();
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
            saveDialog.Title = "Save graph project to a file";
            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true && saveDialog.FileName != "")
                core.SaveVisualGraph(saveDialog.FileName);
        }

      /*  private void AddVertex(object sender, RoutedEventArgs e)
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
        }*/

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

        private void SetForcesMultiplier(object sender, RoutedEventArgs e)
        {
            try
            {
                SetVertexForce(sender, e);
                SetEdgeForce(sender, e);
                SetEdgeLength(sender, e);
                SetFriction(sender, e);
                if (ForcesMultiplierTextBlock != null)
                {
                    ForcesMultiplierTextBlock.Text = Convert.ToString(ForcesMultiplierSlider.Value / 50.0);
                }
            }
            catch
            {}
        }

        private void SetVertexForce(object sender, RoutedEventArgs e)
        {
            try
            {
                core.Physics.VertexForceParam = 4 * VertexForceSlider.Value / 50.0
                        * ForcesMultiplierSlider.Value / 50.0;
                if (VertexForceTextBlock != null)
                {
                    VertexForceTextBlock.Text = Convert.ToString(4 * VertexForceSlider.Value / 50.0);
                }
            }
            catch (Exception)
            {}
        }

        private void SetEdgeForce(object sender, RoutedEventArgs e)
        {
            try
            {
                core.Physics.EdgeForceParam = EdgeForceSlider.Value / 50.0
                        * ForcesMultiplierSlider.Value / 50.0;
                if (EdgeForceTextBlock != null)
                {
                    EdgeForceTextBlock.Text = Convert.ToString(EdgeForceSlider.Value / 50.0);
                }
            }
            catch (Exception)
            {}
        }

        private void SetEdgeLength(object sender, RoutedEventArgs e)
        {
            try
            {
                core.Physics.EdgeLength = EdgeLengthSlider.Value / 50.0;
                if (EdgeLengthTextBlock != null)
                {
                    EdgeLengthTextBlock.Text = Convert.ToString(EdgeLengthSlider.Value / 50.0);
                }
            }
            catch (Exception)
            {}
        }

        private void SetFriction(object sender, RoutedEventArgs e)
        {
            try
            {
                core.Physics.FrictionParam = FrictionSlider.Value / 50.0
                        * ForcesMultiplierSlider.Value / 50.0;
                if (FrictionTextBlock != null)
                {
                    FrictionTextBlock.Text = Convert.ToString(FrictionSlider.Value / 50.0);
                }
            }
            catch (Exception)
            {}
        }

        private void SetVertexLabel(object sender, RoutedEventArgs e) // TODO
        {

        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            visual.ViewWindow.Static = false;
        }

        private void ChangeModeToInsert(object sender, RoutedEventArgs e)
        {
            if(MoveButton.IsChecked == true)
            {
                visual.GraphMode = GraphMode.InsertingMode;
               // visual.setEventHandlersSelect();
                MoveButton.IsChecked = false;
            }
            SelectButton.IsChecked = true;
        }

        private void ChangeModeToMove(object sender, RoutedEventArgs e)
        {
            if (SelectButton.IsChecked == true)
            {
                visual.GraphMode = GraphMode.DraggingMode;
                Console.WriteLine(visual.GraphMode);
               // visual.setEventHandlersMove();
                SelectButton.IsChecked = false;
            }
            MoveButton.IsChecked = true;
        }

        public void EnableVertexControls(string text)
        {
            LabelEditor.Text = text;
            LabelEditor.IsEnabled = true;
            PinnedCheckBox.IsEnabled = true;
        }

        public void DisableVertexControls()
        {
            LabelEditor.Text = "";
            LabelEditor.IsEnabled = false;
            PinnedCheckBox.IsEnabled = true;
        }

        private void LabelChanged(object sender, RoutedEventArgs e)
        {
            visual.changeSelectedLabel(LabelEditor.Text);
        }
    }
}
