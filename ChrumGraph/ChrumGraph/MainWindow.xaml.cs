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
            this.KeyDown += KeyHandler;
            MainCanvas.Background = new SolidColorBrush(Visual.backgroundColor);
            this.Background = new SolidColorBrush(Visual.sidebarColor);
        }

        private void KeyHandler(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
                visual.DeleteSelected();

            if(e.Key == Key.Escape)
                visual.CleanSelectedVertices();
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

        private void SaveProjectClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.DefaultExt = ".graph";
            saveDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveDialog.Title = "Save graph project to a file";
            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true && saveDialog.FileName != "")
                core.SaveVisualGraph(saveDialog.FileName);
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
                    ForcesMultiplierTextBlock.Text = Convert.ToString(Math.Round(ForcesMultiplierSlider.Value / 50.0, 2));
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
                    VertexForceTextBlock.Text = Convert.ToString(Math.Round(4 * VertexForceSlider.Value / 50.0, 2));
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
                    EdgeForceTextBlock.Text = Convert.ToString(Math.Round(EdgeForceSlider.Value / 50.0, 2));
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
                    EdgeLengthTextBlock.Text = Convert.ToString(Math.Round(EdgeLengthSlider.Value / 50.0, 2));
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
                    FrictionTextBlock.Text = Convert.ToString(Math.Round(FrictionSlider.Value / 50.0, 2));
                }
            }
            catch (Exception)
            {}
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
                MoveButton.IsChecked = false;
            }
            SelectButton.IsChecked = true;
        }

        private void ChangeModeToMove(object sender, RoutedEventArgs e)
        {
            if (SelectButton.IsChecked == true)
            {
                visual.GraphMode = GraphMode.DraggingMode;
                SelectButton.IsChecked = false;
            }
            MoveButton.IsChecked = true;
        }

        /// <summary>
        /// Enables vertex controls on sidebar.
        /// </summary>
        /// <param name="text">Text to be put into label editor</param>
        public void EnableVertexControls(string text)
        {
            LabelEditor.Text = text;
            LabelEditor.IsEnabled = true;
            PinnedCheckBox.IsEnabled = true;
        }

        /// <summary>
        /// Disables vertex controls on sidebar.
        /// </summary>
        public void DisableVertexControls()
        {
            LabelEditor.Text = "";
            LabelEditor.IsEnabled = false;
            PinnedCheckBox.IsEnabled = true;
        }

        private void LabelChanged(object sender, RoutedEventArgs e)
        {
            visual.ChangeSelectedLabel(LabelEditor.Text);
        }
    }
}
