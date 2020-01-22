using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace TimeSeriesExtension
{
    public class TimeSeriesPlotter : MonoBehaviour
    {
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

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Awake()
        {
            PlotScale = 10;
            Points = new List<Transform>();          

            CreateTimeSeriesGraphUsingCSV(); // Replace once combined with UI and CSV parsing components

            // Instantiate prefabs for each plot point in Graph
            for (int i = 0; i < Graph.PlotPoints.Count; i++)
            {
                Transform point = Instantiate(PointPrefab);
                point.localScale = new Vector3(0.03f, 0.03f, 0.03f) * PlotScale;
                point.GetComponent<Renderer>().material.color = Random.ColorHSV(0.0f, 1.0f);
                point.SetParent(PointHolder.transform);
                point.localPosition = Vector3.zero;
                Points.Add(point);
            }

            // Center the plot
            PointHolder.transform.position = new Vector3(0, 0, 0);

            // Initialize plot manipulation controls
            //InitializeInteraction();

            Debug.Log(Graph.LogMax());
            Debug.Log(Graph.LogMin());
        }

        void Update()
        {
            if (Time.frameCount % 10 == 0)
            {
                for (int i = 0; i < Points.Count; i++)
                {
                    UpdatePoint(Points[i], i);
                }
            }
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

            // TO-DO determine size of boxCollider

            PointHolder.AddComponent<BoundingBox>();
            PointHolder.AddComponent<ManipulationHandler>();


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

        private float FindMiddle(float max, float min)
        {
            return (max + min) / 2;
        }
    }

}