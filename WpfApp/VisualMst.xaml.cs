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
    public partial class VisualMst : Window
    {
        private bool isDragging;
        private Vector clickOffset;
        private DrawingVisual selectedVisual;
        private Brush drawingBrush;
        private Pen drawingPen;
        private Brush labelForeground;
        private Size selectedLabelSize;
        private int vertexCount;

        private LinkedList<Label> vertices;
        private CoordinateConverter coordinateConverter;

        public VisualMst()
        {
            InitializeComponent();

            vertexCount = 0;
            isDragging = false;
            drawingBrush = Brushes.LightGray;
            drawingPen = new Pen(Brushes.DarkGray, 2);
            labelForeground = Brushes.Black;
            vertices = new LinkedList<Label>();
            coordinateConverter = new CoordinateConverter();
        }

        private void cmdAddVertex_Click(object sender, RoutedEventArgs e)
        {
            TextBlock txtVertexName = new TextBlock
            {
                Text = vertexCount.ToString(),
                FontSize = 16,
            };
            Label vertex = new Label
            {
                Content = txtVertexName,
                BorderBrush = Brushes.DarkGray,
                BorderThickness = new Thickness(2),
                Background = Brushes.LightGray,
                Foreground = Brushes.Black,
                Padding = new Thickness(10)
            };
            vertex.SetValue(Canvas.TopProperty, 10.0);
            vertex.SetValue(Canvas.LeftProperty, 10.0);
            vertex.MouseLeftButtonDown += lbl_MouseLeftButtonDown;
            vertex.MouseLeftButtonUp += lbl_MouseLeftButtonUp;
            vertex.MouseMove += lbl_MouseMove;
            graphCanvas.Children.Add(vertex);
            vertexCount++;
            vertices.AddLast(vertex);
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

            Binding bindingX1 = new Binding();
            bindingX1.Source = vertexV1;
            bindingX1.Path = new PropertyPath("(Canvas.LeftProperty)");
            bindingX1.Converter = coordinateConverter;
            bindingX1.ConverterParameter = vertexV1.ActualWidth;

            Line edge = new Line();
            //edge.X1 = (double)vertexV1.GetValue(Canvas.LeftProperty) + vertexV1.ActualWidth / 2;
            edge.SetBinding(Line.X1Property, bindingX1);
            edge.Y1 = (double)vertexV1.GetValue(Canvas.TopProperty) + vertexV1.ActualHeight / 2;
            edge.X2 = (double)vertexV2.GetValue(Canvas.LeftProperty) + vertexV2.ActualWidth / 2;
            edge.Y2 = (double)vertexV2.GetValue(Canvas.TopProperty) + vertexV2.ActualHeight / 2;
            edge.StrokeThickness = 5;
            edge.Stroke = Brushes.Black;

            TextBlock edgeWeight = new TextBlock { Text = weight.ToString() };
            edgeWeight.SetValue(Canvas.TopProperty, (edge.Y1 + edge.Y2) / 2 - 30.0);
            edgeWeight.SetValue(Canvas.LeftProperty, (edge.X1 + edge.X2) / 2);

            graphCanvas.Children.Add(edge);
            graphCanvas.Children.Add(edgeWeight);
            graphCanvas.Children.Remove(vertexV1);
            graphCanvas.Children.Remove(vertexV2);
            graphCanvas.Children.Add(vertexV1);
            graphCanvas.Children.Add(vertexV2);
        }

        private void cmdRemoveVertex_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdShowMst_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdShowNextStep_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdClearCanvas_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdClearResult_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
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
    }
}
