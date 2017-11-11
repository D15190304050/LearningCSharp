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

            if ((!visualGraph.ContainsVertex(v1)) || (!visualGraph.ContainsVertex(v2)))
                return;

            if (ContainsSameEdge(v1, v2))
            {
                MessageBoxResult msgResult = MessageBox.Show("Refresh the edge with same end point?", "Adding the exsiting edge.", MessageBoxButton.OKCancel);
                if (msgResult != MessageBoxResult.OK)
                    return;
                else
                {
                    VisualEdge edgeToRefresh = null;
                    foreach (VisualEdge ve in visualGraph.Adjacent(v1))
                    {
                        if (ve.Other(v1) == v2)
                        {
                            edgeToRefresh = ve;
                            break;
                        }
                    }

                    edgeToRefresh.WeightLabel.Text = weight.ToString();
                    edgeToRefresh.Weight = weight;
                    ClearTextBox();
                    return;
                }
            }

            Label vertex1 = null;
            Label vertex2 = null;
            foreach (Label lbl in vertices)
            {
                string vertexName = ((TextBlock)lbl.Content).Text;
                if (vertexName == txtVertex1.Text)
                    vertex1 = lbl;
                else if (vertexName == txtVertex2.Text)
                    vertex2 = lbl;
            }

            if ((vertex1 == null) || (vertex2 == null))
                return;

            Binding bindingX1 = new Binding
            {
                Source = vertex1,
                Path = new PropertyPath("(Canvas.Left)"),
                Converter = coordinateConverter,
                ConverterParameter = vertex1.ActualWidth
            };

            Binding bindingY1 = new Binding
            {
                Source = vertex1,
                Path = new PropertyPath("(Canvas.Top)"),
                Converter = coordinateConverter,
                ConverterParameter = vertex1.ActualHeight
            };

            Binding bindingX2 = new Binding
            {
                Source = vertex2,
                Path = new PropertyPath("(Canvas.Left)"),
                Converter = coordinateConverter,
                ConverterParameter = vertex2.ActualWidth
            };

            Binding bindingY2 = new Binding
            {
                Source = vertex2,
                Path = new PropertyPath("(Canvas.Top)"),
                Converter = coordinateConverter,
                ConverterParameter = vertex2.ActualHeight
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
            graphCanvas.Children.Remove(vertex1);
            graphCanvas.Children.Remove(vertex2);
            graphCanvas.Children.Add(vertex1);
            graphCanvas.Children.Add(vertex2);

            VisualEdge visualEdge = new VisualEdge(v1, v2, weight, edge, edgeWeight);
            visualGraph.AddEdge(visualEdge);

            ClearTextBox();
        }

        private void cmdRemoveVertex_Click(object sender, RoutedEventArgs e)
        {
            if (selectedLabel != null)
            {
                string vertexName = ((TextBlock)selectedLabel.Content).Text;
                int v = int.Parse(vertexName);
                IEnumerable<VisualEdge> adjacents = visualGraph.Adjacent(v);
                foreach (VisualEdge edge in adjacents)
                {
                    graphCanvas.Children.Remove(edge.Line);
                    graphCanvas.Children.Remove(edge.WeightLabel);
                    weightLabels.Remove(edge.WeightLabel);
                }
                visualGraph.RemoveVertex(v);
                graphCanvas.Children.Remove(selectedLabel);
                selectedLabel = null;
            }

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

        private void cmdClearResult_Click(object sender, RoutedEventArgs e) => ClearResult();
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
            foreach (VisualEdge e in visualGraph.Adjacent(v))
            {
                int u = e.Other(v);
                if (u == w)
                    return true;
            }
            return false;
        }

        private void cbAlgorithm_SelectionChanged(object sender, SelectionChangedEventArgs e) => ClearResult();

        private void ClearResult()
        {
            VisualEdge[] edges = visualGraph.Edges().ToArray();
            for (int i = 0; i < edges.Length; i++)
                edges[i].Line.Stroke = normalEdgeBrush;
            remainingEdges = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbAlgorithm.SelectionChanged += (o, ev) => ClearResult();
        }

        private void ClearTextBox()
        {
            txtVertex1.Text = "";
            txtVertex2.Text = "";
            txtWeight.Text = "";
        }
    }
}