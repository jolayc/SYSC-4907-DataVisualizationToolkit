using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace TimeSeriesExtension
{
    public class TimeSeriesPlotter : MonoBehaviour
    {
        public GameObject Text;
        public GameObject PointHolder;
        public Transform PointPrefab;

        public TextAsset DataFile1;  // Remove if CSV parsing is handled by an outside source
        public TextAsset DataFile2;

        public TimeSeriesGraph Graph;
        private List<Transform> Points;

        // Labels
        public string PlotTitle;
        public string XAxisName;
        public string YAxisName;
        public string ZAxisName;

        // Resources
        public Material HandleMaterial;
        public Material HandleGrabbedMaterial;
        public GameObject RotationHandle;
        public GameObject ScaleHandle;
        public TrailRenderer TrailRenderer;
        public float PlotScale;

        // from Graph
        private float GraphXMax, GraphXMid, GraphXMin;
        private float GraphYMax, GraphYMid, GraphYMin;
        private float GraphZMax, GraphZMid, GraphZMin;


        // Start is called before the first frame update
        void Start()
        {
            // Lock scene rendering to 60 fps
            Application.targetFrameRate = 60;
        }

        private void Awake()
        {
            PlotScale = 10;
            Points = new List<Transform>();          

            CreateTimeSeriesGraphUsingCSV(); // Replace once combined with UI and CSV parsing components

            // Grab max, min and mid points of graph
            GraphXMax = Graph.XMax;
            GraphXMid = Graph.XMid;
            GraphXMin = Graph.XMin;

            GraphYMax = Graph.YMax;
            GraphYMid = Graph.YMid;
            GraphYMin = Graph.YMin;

            GraphZMax = Graph.ZMax;
            GraphZMid = Graph.ZMid;
            GraphZMin = Graph.ZMin;

            // Instantiate prefabs for each plot point in Graph
            DrawPlot();

            // Draw plot labels and other information
            DrawLabels();

            PointHolder.transform.position = Vector3.zero; // Center the plot to the middle of the screen

            // Initialize plot manipulation controls
            //InitializeInteraction();

            // LogValues(); // for debugging
        }

        void Update()
        {
            if (Time.frameCount % 5 == 0)
            {
                for (int i = 0; i < Points.Count; i++)
                {
                    UpdatePoint(Points[i], i);
                }
            }
        }

        /**
         * Log various Graph values for debugging purposes
         */ 
        private void LogValues()
        {
            Debug.Log(Graph.LogMax());
            Debug.Log(Graph.LogMin());
            Debug.Log(Graph.LogMid());
        }

        private void DrawPlot()
        {
            // Render plot points at center (0, 0, 0)
            int numberOfPoints = Graph.PlotPoints.Count;

            for (int i = 0; i < numberOfPoints; i++)
            {
                Transform current_point = Instantiate(PointPrefab);
                current_point.GetComponent<Renderer>().material.color = Random.ColorHSV(0.0f, 1.0f); // assign random color to point
                current_point.SetParent(PointHolder.transform);
                current_point.localPosition = Vector3.zero;
                current_point.localScale = new Vector3(0.03f, 0.03f, 0.03f) * PlotScale;
                Points.Add(current_point);
            }

            // Center the pivot of object to center of plot
            PointHolder.transform.position = new Vector3(Normalize(GraphXMid, GraphXMax, GraphXMin),
                                                         Normalize(GraphYMid, GraphYMax, GraphYMin),
                                                         Normalize(GraphZMid, GraphZMax, GraphZMin))
                                                         * PlotScale;
        }

        private void DrawLabels()
        {
            GameObject plotTitle = Instantiate(Text, new Vector3(Normalize(GraphXMid, GraphXMax, GraphXMin),
                                                                Normalize(GraphYMax, GraphYMax, GraphYMin),
                                                                Normalize(GraphZMid, GraphZMax, GraphZMin))
                                                                * PlotScale, Quaternion.identity);

            // Add title
            plotTitle.transform.SetParent(PointHolder.transform);
            plotTitle.GetComponent<TextMesh>().text = PlotTitle;

            plotTitle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * PlotScale;
            plotTitle.transform.position = plotTitle.transform.position + new Vector3(0, plotTitle.GetComponent<Renderer>().bounds.size.y / 2, 0);

            // Add x-axis
            GameObject xlabel;
            xlabel = Instantiate(Text, new Vector3(Normalize(GraphXMid, GraphXMax, GraphXMin),
                                                   Normalize(GraphYMid, GraphYMax, GraphYMin),
                                                   Normalize(GraphZMid, GraphZMax, GraphZMin))
                                                   * PlotScale, Quaternion.Euler(90, 0, 0));

            xlabel.transform.SetParent(PointHolder.transform);
            xlabel.GetComponent<TextMesh>().text = XAxisName;
            xlabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * PlotScale;

            // Add y-axis
            GameObject ylabel = Instantiate(Text, new Vector3(Normalize(GraphXMid, GraphXMax, GraphXMin),
                                                   Normalize(GraphYMid, GraphYMax, GraphYMin),
                                                   Normalize(GraphZMid, GraphZMax, GraphZMin))
                                                   * PlotScale, Quaternion.Euler(0, 0, 90));

            ylabel.transform.SetParent(PointHolder.transform);
            ylabel.GetComponent<TextMesh>().text = YAxisName;
            ylabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * PlotScale;


            // Add z-axis
            GameObject zlabel = Instantiate(Text, new Vector3(Normalize(GraphXMid, GraphXMax, GraphXMin),
                                                   Normalize(GraphYMid, GraphYMax, GraphYMin),
                                                   Normalize(GraphZMid, GraphZMax, GraphZMin))
                                                   * PlotScale, Quaternion.Euler(90, 90, 0));

            zlabel.transform.SetParent(PointHolder.transform);
            zlabel.GetComponent<TextMesh>().text = ZAxisName;
            zlabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * PlotScale;
        }

        /*
         * Temporary method to use CSV parsing when creating graph
         */
        private void CreateTimeSeriesGraphUsingCSV()
        {
            var csvString = DataFile1.ToString(); // Convert CSV to String for easier use
            DataParser parser = new DataParser(csvString);

            // Create PlotPoint object using data from CSV
            List<float> x_values = parser.GetListFromColumn(0); // Grab values from first column
            List<float> y_values = parser.GetListFromColumn(1);
            List<float> z_values = parser.GetListFromColumn(2);
            PlotPoint new_point = new PlotPoint(x_values, y_values, z_values);

            // Create empty Graph object and give it plot points
            Graph = new TimeSeriesGraph();
            Graph.AddPlotPoint(new_point);

            csvString = DataFile2.ToString();
            parser = new DataParser(csvString);

            x_values = parser.GetListFromColumn(0);
            y_values = parser.GetListFromColumn(1);
            z_values = parser.GetListFromColumn(2);
            new_point = new PlotPoint(x_values, y_values, z_values);

            // Create empty Graph object and give it plot points
            Graph.AddPlotPoint(new_point);
        }

        private void UpdatePoint(Transform point, int index)
        {
            Vector3 position;

            // Grab the corresponding point from graph using given index
            PlotPoint pointFromGraph = Graph.PlotPoints[index];

            float xmax, xmin, ymax, ymin, zmax, zmin;

            xmax = Graph.XMax;
            ymax = Graph.YMax;
            zmax = Graph.ZMax;
            xmin = Graph.XMin;
            ymin = Graph.YMin;
            zmin = Graph.ZMin;

            int currentIndex = pointFromGraph.currentPointIndex;

            if (currentIndex < pointFromGraph.XPoints.Count)
            {
                // Update position vector
                position.x = Normalize(pointFromGraph.XPoints[currentIndex], xmax, xmin);
                position.y = Normalize(pointFromGraph.YPoints[currentIndex], ymax, ymin);
                position.z = Normalize(pointFromGraph.ZPoints[currentIndex], zmax, zmin);

                //  Render point with updated position
                point.localPosition = position * PlotScale;

                // Update index
                pointFromGraph.currentPointIndex++;
            }
            else
            {
                pointFromGraph.currentPointIndex = 0;
            }
        }

        private void InitializeInteraction()
        {
            BoxCollider boxCollider = PointHolder.AddComponent<BoxCollider>();

            // TO-DO: determine size of boxCollider

            PointHolder.AddComponent<BoundingBox>();
            PointHolder.GetComponent<BoundingBox>().WireframeEdgeRadius = PointHolder.GetComponent<BoundingBox>().WireframeEdgeRadius * PlotScale;
            PointHolder.GetComponent<BoundingBox>().WireframeMaterial.color = Color.white;
            PointHolder.AddComponent<ManipulationHandler>();

            // Scale handle sizes
            PointHolder.GetComponent<BoundingBox>().ScaleHandleSize = PointHolder.GetComponent<BoundingBox>().ScaleHandleSize * PlotScale;
            PointHolder.GetComponent<BoundingBox>().RotationHandleSize = PointHolder.GetComponent<BoundingBox>().RotationHandleSize * PlotScale;

            // TO-DO: add optional prefab models
        }

        private float Normalize(float value, float max, float min)
        {
            // If values are all zero or constant
            if (max - min == 0)
            {
                return value;
            }
            else
            {
                return (value - min) / (max - min);
            }
        }
    }
}