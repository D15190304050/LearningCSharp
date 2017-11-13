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
        /// <summary>
        /// The stirng literal that indicates the Prim's Algorithm.
        /// </summary>
        private const string Prim = "Prim's Algorithm";

        /// <summary>
        /// The string literal that indicates the Kruskal's Algorithm.
        /// </summary>
        private const string Kruskal = "Kruskal's Algorithm";

        /// <summary>
        /// True if some vertex is being dragged, false otherwise.
        /// </summary>
        private bool isDragging;

        /// <summary>
        /// The selected label, null if there is no label selected.
        /// </summary>
        private Label selectedLabel;

        /// <summary>
        /// The background color of visual vertex.
        /// </summary>
        private Brush lblBackground;

        /// <summary>
        /// The border color of visual vertex.
        /// </summary>
        private Brush lblBorder;

        /// <summary>
        /// The foreground color of visual vertex.
        /// </summary>
        private Brush lblForeground;

        /// <summary>
        /// The background color of selected visual vertex.
        /// </summary>
        private Brush selectedLabelBackground;

        /// <summary>
        /// The stroke color of odinary edges.
        /// </summary>
        private Brush normalEdgeBrush;

        /// <summary>
        /// The stroke color of edges in a MST (or a forest).
        /// </summary>
        private Brush mstEdgeStroke;

        /// <summary>
        /// The collection of all visual vertices.
        /// </summary>
        private LinkedList<Label> vertices;

        /// <summary>
        /// The collection of all visual weights.
        /// </summary>
        private LinkedList<WeightLabel> weightLabels;

        //private Dictionary<Line, VisualEdge> edgeMap;

        /// <summary>
        /// A coordinate converter used to compute the correct coordinate of each edge.
        /// </summary>
        private EdgeCoordinateConverter coordinateConverter;

        /// <summary>
        /// The visual graph used to compute the MST (or forest).
        /// </summary>
        private VisualEdgeWeightedGraph visualGraph;

        /// <summary>
        /// An enumerator to support iterations through all edges in a MST (or forest).
        /// </summary>
        private IEnumerator<VisualEdge> remainingEdges;

        /// <summary>
        /// Initializes the window for the visual MST computation.
        /// </summary>
        public VisualMstWindow()
        {
            InitializeComponent();

            isDragging = false;
            lblBackground = Brushes.LightGray;
            lblBorder = Brushes.DarkGray;
            lblForeground = Brushes.Black;
            selectedLabelBackground = Brushes.Orange;
            normalEdgeBrush = Brushes.Black;
            mstEdgeStroke = Brushes.MediumVioletRed;
            vertices = new LinkedList<Label>();
            weightLabels = new LinkedList<WeightLabel>();
            coordinateConverter = new EdgeCoordinateConverter();
            visualGraph = new VisualEdgeWeightedGraph();
            //edgeMap = new Dictionary<Line, VisualEdge>();
        }

        /// <summary>
        /// Adds a vertex to the window an to the graph.
        /// </summary>
        /// <param name="sender">The Button instance associated with this method.</param>
        /// <param name="e">The RoutedEventArgs that contains state information and event data associated with the Click event.</param>
        private void cmdAddVertex_Click(object sender, RoutedEventArgs e)
        {
            // Initialize a TextBlock to contains the weight with specified vertex name and font size.
            TextBlock txtVertexName = new TextBlock
            {
                Text = vertices.Count.ToString(),
                FontSize = 16,
            };

            // Initialize a label with specified content and some other properties.
            Label vertex = new Label
            {
                Content = txtVertexName,
                BorderBrush = lblBorder,
                BorderThickness = new Thickness(2),
                Background = lblBackground,
                Foreground = lblForeground,
                Padding = new Thickness(10)
            };

            // Set the initial coordinate of this weight label.
            vertex.SetValue(Canvas.TopProperty, 10.0);
            vertex.SetValue(Canvas.LeftProperty, 10.0);

            // Associate the mouse event with this weight label.
            vertex.MouseLeftButtonDown += lbl_MouseLeftButtonDown;
            vertex.MouseLeftButtonUp += lbl_MouseLeftButtonUp;
            vertex.MouseMove += lbl_MouseMove;

            // Add this weight label to canvas, vertices and visualGraph for sub-sequent processing.
            graphCanvas.Children.Add(vertex);
            vertices.AddLast(vertex);
            visualGraph.AddVertex();

            // Clear the result computed before because we have a different graph now.
            ClearResult();
        }

        /// <summary>
        /// Add an edge to the window and the graph.
        /// </summary>
        /// <remarks>
        /// This method will create an instance of VisualEdge using the infomation in the TextBox on the window.
        /// </remarks>
        /// <param name="sender">The Button instance associated with this method.</param>
        /// <param name="e">The RoutedEventArgs that contains state information and event data associated with the Click event.</param>
        private void cmdAddEdge_Click(object sender, RoutedEventArgs e)
        {
            #region Validation
            // Validate vertex 1.
            if ((!int.TryParse(txtVertex1.Text, out int v1)) ||
                v1 < 0)
            {
                MessageBox.Show("The name of a vertex in a graph must be a non-negative integer.");
                return;
            }

            // Validate vertex 2.
            if ((!int.TryParse(txtVertex2.Text, out int v2)) ||
                v2 < 0)
            {
                MessageBox.Show("The name of a vertex in a graph must be a non-negative integer.");
                return;
            }

            // Validate the weight of this edge.
            if (!double.TryParse(txtWeight.Text, out double weight))
            {
                MessageBox.Show("The value of weight must be a floating point value.");
                return;
            }

            // Report that can't add this edge if either end point of this edge doesn't exist.
            if ((!visualGraph.ContainsVertex(v1)) || (!visualGraph.ContainsVertex(v2)))
            {
                MessageBox.Show("Can't add such edge.");
                return;
            }
            #endregion

            // Determine whether an update to the existing edge is needed or not.
            if (ContainsSameEdge(v1, v2))
            {
                // Report that there exists an edge with same end points and get the decision from the user.
                MessageBoxResult msgResult = MessageBox.Show("Refresh the edge with same end point?", "Adding the exsiting edge.", MessageBoxButton.OKCancel);

                // Do nothing if update is not needed.
                // Find the edge and refresh its weight otherwise.
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

                    // Refresh.
                    edgeToRefresh.WeightLabel.Text = weight.ToString();
                    edgeToRefresh.Weight = weight;

                    // Clear the TextBox to make it easy to add next edge because the user doesn't need to clear the TextBoxes manually.
                    ClearTextBox();

                    // Clear the result computed before because we have a different graph now.
                    ClearResult();

                    // End this call.
                    return;
                }
            }

            // Get the label with the same name.
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

            // Do nothing if no such vertex-pair (technically this if will always get true because there is a same validation above,
            // and if the pocess reach here, it can pass this validation as well).
            if ((vertex1 == null) || (vertex2 == null))
                return;

            // Set binding for the X1 property of the visual edge.
            Binding bindingX1 = new Binding
            {
                Source = vertex1,
                Path = new PropertyPath("(Canvas.Left)"),
                Converter = coordinateConverter,
                ConverterParameter = vertex1.ActualWidth
            };

            // Set binding for the Y1 property of the visual edge.
            Binding bindingY1 = new Binding
            {
                Source = vertex1,
                Path = new PropertyPath("(Canvas.Top)"),
                Converter = coordinateConverter,
                ConverterParameter = vertex1.ActualHeight
            };

            // Set binding for the X2 property of the visual edge.
            Binding bindingX2 = new Binding
            {
                Source = vertex2,
                Path = new PropertyPath("(Canvas.Left)"),
                Converter = coordinateConverter,
                ConverterParameter = vertex2.ActualWidth
            };

            // Set binding for the Y2 property of the visual edge.
            Binding bindingY2 = new Binding
            {
                Source = vertex2,
                Path = new PropertyPath("(Canvas.Top)"),
                Converter = coordinateConverter,
                ConverterParameter = vertex2.ActualHeight
            };

            // Initialize a line as the visual edge with specified stroke.
            Line edge = new Line
            {
                StrokeThickness = 5,
                Stroke = normalEdgeBrush
            };             
            //edge.X1 = (double)vertexV1.GetValue(Canvas.LeftProperty) + vertexV1.ActualWidth / 2;
            //edge.Y1 = (double)vertexV1.GetValue(Canvas.TopProperty) + vertexV1.ActualHeight / 2;
            //edge.X2 = (double)vertexV2.GetValue(Canvas.LeftProperty) + vertexV2.ActualWidth / 2;
            //edge.Y2 = (double)vertexV2.GetValue(Canvas.TopProperty) + vertexV2.ActualHeight / 2;

            // Associate bindings with the visual edge.
            edge.SetBinding(Line.X1Property, bindingX1);
            edge.SetBinding(Line.Y1Property, bindingY1);
            edge.SetBinding(Line.X2Property, bindingX2);
            edge.SetBinding(Line.Y2Property, bindingY2);

            // Initialize the WeightLabel using specified infomation and add it to the canvas and weightLabels.
            WeightLabel edgeWeight = new WeightLabel(edge) { Text = weight.ToString() };
            edgeWeight.RefreshCoordinate();
            weightLabels.AddLast(edgeWeight);

            // Add the edge and its weight to the canvas.
            graphCanvas.Children.Add(edge);
            graphCanvas.Children.Add(edgeWeight);

            // Remove and re-add the 2 vertices of this edge so that the can cover the overlap between the edge created above and itself.
            // It's about the order to render them.
            graphCanvas.Children.Remove(vertex1);
            graphCanvas.Children.Remove(vertex2);
            graphCanvas.Children.Add(vertex1);
            graphCanvas.Children.Add(vertex2);

            // Create the instance of VisualEdge that represents the edge created above and add it to visualGraph.
            VisualEdge visualEdge = new VisualEdge(v1, v2, weight, edge, edgeWeight);
            visualGraph.AddEdge(visualEdge);
            //edgeMap.Add(edge, visualEdge);

            // Clear the TextBox to make it easy to add next edge because the user doesn't need to clear the TextBoxes manually.
            ClearTextBox();

            // Clear the result computed before because we have a different graph now.
            ClearResult();
        }

        /// <summary>
        /// Removes the selected vertex or selected edge.
        /// </summary>
        /// <param name="sender">The Button instance associated with this method.</param>
        /// <param name="e">The RoutedEventArgs that contains state information and event data associated with the Click event.</param>
        private void cmdRemove_Click(object sender, RoutedEventArgs e)
        {
            if (selectedLabel != null)
            {
                if (selectedLabel is Label)
                    RemoveSelectedVertex();
                selectedLabel = null;
            }

            //MessageBox.Show(visualGraph.ToString());
        }

        /// <summary>
        /// Remvoes the selected vertex.
        /// </summary>
        private void RemoveSelectedVertex()
        {
            string vertexName = ((TextBlock)selectedLabel.Content).Text;
            int v = int.Parse(vertexName);
            IEnumerable<VisualEdge> adjacents = visualGraph.Adjacent(v);
            foreach (VisualEdge edge in adjacents)
            {
                graphCanvas.Children.Remove(edge.Line);
                graphCanvas.Children.Remove(edge.WeightLabel);
                weightLabels.Remove(edge.WeightLabel);
                //edgeMap.Remove(edge.Line);
            }
            visualGraph.RemoveVertex(v);
            graphCanvas.Children.Remove(selectedLabel);
            selectedLabel = null;
        }

        /// <summary>
        /// Renders all edges of the MST (or forest) corresponding to the VisualEdgeWeightedGraph shown on the window.
        /// </summary>
        /// <param name="sender">The Button instance associated with this method.</param>
        /// <param name="e">The RoutedEventArgs that contains state information and event data associated with the Click event.</param>
        private void cmdShowMst_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<VisualEdge> mst = new VisualKruskalMst(visualGraph).Edges;
            VisualEdge[] edges = mst.ToArray();
            for (int i = 0; i < edges.Length; i++)
                edges[i].Line.Stroke = mstEdgeStroke;
        }

        /// <summary>
        /// Renders next edge of the MST (or forest) corresponding to the VisualEdgeWeightedGraph shown on the window. Do nothing if the entire MST (or forest) is rendered.
        /// </summary>
        /// <param name="sender">The Button instance associated with this method.</param>
        /// <param name="e">The RoutedEventArgs that contains state information and event data associated with the Click event.</param>
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

        /// <summary>
        /// Removes all elements from the canvas that contains the visualized graph.
        /// </summary>
        /// <param name="sender">The Button instance associated with this method.</param>
        /// <param name="e">The RoutedEventArgs that contains state information and event data associated with the Click event.</param>
        private void cmdClearCanvas_Click(object sender, RoutedEventArgs e)
        {
            graphCanvas.Children.Clear();
            vertices.Clear();
            weightLabels.Clear();
            visualGraph.Clear();
        }

        /// <summary>
        /// Removes all rendering of edges on MST (or forest).
        /// </summary>
        /// <param name="sender">The Button instance associated with this method.</param>
        /// <param name="e">The RoutedEventArgs that contains state information and event data associated with the Click event.</param>
        private void cmdClearResult_Click(object sender, RoutedEventArgs e) => ClearResult();

        /// <summary>
        /// Renders the selected visual vertex to indicate that it is the label the user is dragging.
        /// </summary>
        /// <param name="sender">The selected visual vertex.</param>
        /// <param name="e">The MouseButtonEventArgs that contains state information and event data associated with the MouseLeftButtonDown event.</param>
        private void lbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Make the mouse able to drag the visual vertex.
            isDragging = true;

            // Set the background color of last selected visual vertex to ordinary color.
            if (selectedLabel != null)
                selectedLabel.Background = lblBackground;

            // Set the background color of currrent selected visual vertex.
            selectedLabel = (Label)sender;
            selectedLabel.Background = selectedLabelBackground;
        }

        /// <summary>
        /// Drags the visual vertex.
        /// </summary>
        /// <param name="sender">The selected visual vertex.</param>
        /// <param name="e">The MouseButtonEventArgs that contains state information and event data associated with the MouseMove event.</param>
        private void lbl_MouseMove(object sender, MouseEventArgs e)
        {
            // Do nothing if unable to drag.
            if (isDragging)
            {
                // Get the selected visual vertex.
                Label selectedLabel = (Label)sender;

                // Get the coordinate of mouse.
                Point mousePositionOnCanvas = e.GetPosition(graphCanvas);

                // Compute the offset.
                double offsetX = selectedLabel.ActualWidth / 2;
                double offsetY = selectedLabel.ActualHeight / 2;

                // Reset the coordinate of the visual vertex.
                selectedLabel.SetValue(Canvas.LeftProperty, mousePositionOnCanvas.X - offsetX);
                selectedLabel.SetValue(Canvas.TopProperty, mousePositionOnCanvas.Y - offsetY);
            }
        }

        /// <summary>
        /// Cancels the dragging on a visual vertex.
        /// </summary>
        /// <param name="sender">The selected visual vertex.</param>
        /// <param name="e">The MouseButtonEventArgs that contains state information and event data associated with the MouseLeftButtonUp event.</param>
        private void lbl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) => isDragging = false;

        /// <summary>
        /// Refreshes coordinates of all weight labels.
        /// </summary>
        /// <param name="sender">The canvas that contains the visualized graph.</param>
        /// <param name="e">The MouseButtonEventArgs that contains state information and event data associated with the MouseLeftButtonUp event.</param>
        private void graphCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            foreach (WeightLabel w in weightLabels)
                w.RefreshCoordinate();
        }

        /// <summary>
        /// Renders the next edge on the MST (or forest) corresponding to the visualized graph on the window. Do nothing if the entire MST (or forest) is rendered.
        /// </summary>
        private void RenderNextMstEdge()
        {
            // Do nothing if there is no next edge (to render), which indicates the entire MST (or forest) is rendered.
            if (!remainingEdges.MoveNext())
            {
                // Set the edge enumerator to null to prevent extra computing.
                remainingEdges = null;
                return;
            }

            // Get the edge to render.
            VisualEdge edgeToRender = remainingEdges.Current;

            // Reset its stroke color.
            edgeToRender.Line.Stroke = mstEdgeStroke;
        }

        /// <summary>
        /// Returns true if there is an edge in the VisualEdgeWeightedGraph with the same end points, false otherwise.
        /// </summary>
        /// <param name="v">A vertex of the edge to add.</param>
        /// <param name="w">Another vertex of the edge to add.</param>
        /// <returns>True if there is an edge in the VisualEdgeWeightedGraph with the same end points, false otherwise.</returns>
        private bool ContainsSameEdge(int v, int w)
        {
            // Traverse through all edges incident to vertex v.
            foreach (VisualEdge e in visualGraph.Adjacent(v))
            {
                // Get the other end point of vertex v on this edge.
                int u = e.Other(v);

                // Return true if they have same end points.
                if (u == w)
                    return true;
            }

            // No such edge if reach here.
            return false;
        }

        /// <summary>
        /// Clears the result computed before.
        /// </summary>
        /// <param name="sender">The ComboBox whose selected item indicates which algorithm to run.</param>
        /// <param name="e">The SelectionChangedEventArgs that contains state information and event data associated with the SelectionChanged event.</param>
        private void cbAlgorithm_SelectionChanged(object sender, SelectionChangedEventArgs e) => ClearResult();

        /// <summary>
        /// Clear the result computed before.
        /// </summary>
        private void ClearResult()
        {
            // Set the storke color of all visual edges to ordinary color.
            VisualEdge[] edges = visualGraph.Edges().ToArray();
            for (int i = 0; i < edges.Length; i++)
                edges[i].Line.Stroke = normalEdgeBrush;

            // Set the edge enumerator to null so it will be re-computed when needed.
            // If there is no such assignment, then when uesrs click the "Show Next Step" Button, only an uncomplete MST (or forest) can they get.
            // Because this program will just render next edge returned by this enumerator if it is not null, and they may have several edges rendered before.
            remainingEdges = null;
        }

        /// <summary>
        /// Associates the SelectionChanged event with its handler after this window is loaded.
        /// </summary>
        /// <param name="sender">This window.</param>
        /// <param name="e">The RoutedEventArgs that contains state information and event data associated with the Loaded event.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbAlgorithm.SelectionChanged += (o, ev) => ClearResult();
        }

        /// <summary>
        /// Clears all TextBoxes.
        /// </summary>
        private void ClearTextBox()
        {
            txtVertex1.Text = "";
            txtVertex2.Text = "";
            txtWeight.Text = "";
        }
    }
}