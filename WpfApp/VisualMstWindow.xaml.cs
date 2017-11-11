using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// VisualMst.xaml 的交互逻辑
    /// </summary>
    public partial class VisualMstWindow : Window
    {
        private const string Prim = "Prim's Algorithm";
        private const string Kruskal = "Kruskal's Algorithm";

        private bool isDragging;
        private Label selectedLabel;
        private Brush lblBackground;
        private Brush lblBorder;
        private Brush lblForeground;
        private Brush selectedLabelBackground;
        private Brush normalEdgeBrush = Brushes.Black;
        private Brush mstEdgeStroke = Brushes.MediumVioletRed;

        private LinkedList<Label> vertices;
        private LinkedList<WeightLabel> weightLabels;
        private EdgeCoordinateConverter coordinateConverter;

        private VisualEdgeWeightedGraph visualGraph;

        private IEnumerator<VisualEdge> remainingEdges;

        public VisualMstWindow()
        {
            InitializeComponent();

            isDragging = false;
            lblBackground = Brushes.LightGray;
            lblBorder = Brushes.DarkGray;
            lblForeground = Brushes.Black;
            selectedLabelBackground = Brushes.Orange;
            vertices = new LinkedList<Label>();
            weightLabels = new LinkedList<WeightLabel>();
            coordinateConverter = new EdgeCoordinateConverter();
            visualGraph = new VisualEdgeWeightedGraph();
        }

        private void cmdAddVertex_Click(object sender, RoutedEventArgs e)
        {
            TextBlock txtVertexName = new TextBlock
            {
                Text = vertices.Count.ToString(),
                FontSize = 16,
            };
            Label vertex = new Label
            {
                Content = txtVertexName,
                BorderBrush = lblBorder,
                BorderThickness = new Thickness(2),
                Background = lblBackground,
                Foreground = lblForeground,
                Padding = new Thickness(10)
            };
            vertex.SetValue(Canvas.TopProperty, 10.0);
            vertex.SetValue(Canvas.LeftProperty, 10.0);
            vertex.MouseLeftButtonDown += lbl_MouseLeftButtonDown;
            vertex.MouseLeftButtonUp += lbl_MouseLeftButtonUp;
            vertex.MouseMove += lbl_MouseMove;
            graphCanvas.Children.Add(vertex);
            vertices.AddLast(vertex);
            visualGraph.AddVertex();
        }

        private void cmdAddEdge_Click(object sender, RoutedEventArgs e)
        {
            int v1 = int.Parse(txtVertex1.Text);
            int v2 = int.Parse(txtVertex2.Text);



            double weight = double.Parse(txtWeight.Text);

            Label vertexV1 = null;
            Label vertexV2 = null;
            foreach (Label lbl in vertices)
            {
                string vertexName = ((TextBlock)lbl.Content).Text;
                if (vertexName == txtVertex1.Text)
                    vertexV1 = lbl;
                else if (vertexName == txtVertex2.Text)
                    vertexV2 = lbl;
            }

            if ((vertexV1 == null) || (vertexV2 == null))
                return;

            Binding bindingX1 = new Binding
            {
                Source = vertexV1,
                Path = new PropertyPath("(Canvas.Left)"),
                Converter = coordinateConverter,
                ConverterParameter = vertexV1.ActualWidth
            };

            Binding bindingY1 = new Binding
            {
                Source = vertexV1,
                Path = new PropertyPath("(Canvas.Top)"),
                Converter = coordinateConverter,
                ConverterParameter = vertexV1.ActualHeight
            };

            Binding bindingX2 = new Binding
            {
                Source = vertexV2,
                Path = new PropertyPath("(Canvas.Left)"),
                Converter = coordinateConverter,
                ConverterParameter = vertexV2.ActualWidth
            };

            Binding bindingY2 = new Binding
            {
                Source = vertexV2,
                Path = new PropertyPath("(Canvas.Top)"),
                Converter = coordinateConverter,
                ConverterParameter = vertexV2.ActualHeight
            };

            Line edge = new Line();
            //edge.X1 = (double)vertexV1.GetValue(Canvas.LeftProperty) + vertexV1.ActualWidth / 2;
            //edge.Y1 = (double)vertexV1.GetValue(Canvas.TopProperty) + vertexV1.ActualHeight / 2;
            //edge.X2 = (double)vertexV2.GetValue(Canvas.LeftProperty) + vertexV2.ActualWidth / 2;
            //edge.Y2 = (double)vertexV2.GetValue(Canvas.TopProperty) + vertexV2.ActualHeight / 2;

            edge.SetBinding(Line.X1Property, bindingX1);
            edge.SetBinding(Line.Y1Property, bindingY1);
            edge.SetBinding(Line.X2Property, bindingX2);
            edge.SetBinding(Line.Y2Property, bindingY2);

            edge.StrokeThickness = 5;
            edge.Stroke = normalEdgeBrush;

            WeightLabel edgeWeight = new WeightLabel(edge) { Text = weight.ToString() };
            edgeWeight.RefreshCoordinate();
            weightLabels.AddLast(edgeWeight);

            graphCanvas.Children.Add(edge);
            graphCanvas.Children.Add(edgeWeight);
            graphCanvas.Children.Remove(vertexV1);
            graphCanvas.Children.Remove(vertexV2);
            graphCanvas.Children.Add(vertexV1);
            graphCanvas.Children.Add(vertexV2);

            VisualEdge visualEdge = new VisualEdge(v1, v2, weight, edge);
            visualGraph.AddEdge(visualEdge);

            txtVertex1.Text = "";
            txtVertex2.Text = "";
            txtWeight.Text = "";
        }

        private void cmdRemoveVertex_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(visualGraph.ToString());
        }

        private void cmdShowMst_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<VisualEdge> mst = new VisualKruskalMst(visualGraph).Edges;
            VisualEdge[] edges = mst.ToArray();
            for (int i = 0; i < edges.Length; i++)
                edges[i].Line.Stroke = mstEdgeStroke;
        }

        private void cmdShowNextStep_Click(object sender, RoutedEventArgs e)
        {
            if (remainingEdges != null)
                RenderNextMstEdge();
            else
            {
                IEnumerable<VisualEdge> mst = null;
                string selectedAlgorithm = ((TextBlock)cbAlgorithm.SelectedItem).Text;

                switch (selectedAlgorithm)
                {
                    case Kruskal:
                        mst = new VisualKruskalMst(visualGraph).Edges;
                        break;
                    case Prim:
                        mst = new VisualPrimMst(visualGraph).Edges;
                        break;
                    default:
                        break;
                }

                remainingEdges = mst.GetEnumerator();
                RenderNextMstEdge();
            }
        }

        private void cmdClearCanvas_Click(object sender, RoutedEventArgs e)
        {
            graphCanvas.Children.Clear();
            vertices.Clear();
            weightLabels.Clear();
            visualGraph.Clear();
        }

        private void cmdClearResult_Click(object sender, RoutedEventArgs e)
        {
            VisualEdge[] edges = visualGraph.Edges().ToArray();
            for (int i = 0; i < edges.Length; i++)
                edges[i].Line.Stroke = normalEdgeBrush;
        }

        private void lbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;

            if (selectedLabel != null)
                selectedLabel.Background = lblBackground;

            selectedLabel = (Label)sender;
            selectedLabel.Background = selectedLabelBackground;
        }

        private void lbl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Label selectedLabel = (Label)sender;
                Point mousePositionOnCanvas = e.GetPosition(graphCanvas);
                double offsetX = selectedLabel.ActualWidth / 2;
                double offsetY = selectedLabel.ActualHeight / 2;
                selectedLabel.SetValue(Canvas.LeftProperty, mousePositionOnCanvas.X - offsetX);
                selectedLabel.SetValue(Canvas.TopProperty, mousePositionOnCanvas.Y - offsetY);
            }
        }

        private void lbl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

        private void graphCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            foreach (WeightLabel w in weightLabels)
                w.RefreshCoordinate();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (selectedLabel == null)
                return;
            else if (e.Key != Key.Delete)
                return;
        }

        private void graphCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Hit
        }

        private void RenderNextMstEdge()
        {
            if (!remainingEdges.MoveNext())
            {
                remainingEdges = null;
                return;
            }
            VisualEdge edge = remainingEdges.Current;
            edge.Line.Stroke = mstEdgeStroke;
        }

        private bool ContainsSameEdge(int v, int w)
        {

        }
    }
}