using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeSeriesExtension
{
    public class TimeSeriesPlotter : MonoBehaviour
    {
        public Transform PointPrefab;
        public TextAsset DataFile;  // Remove if CSV parsing is handled by an outside source

        private TimeSeriesGraph Graph;
        private List<Transform> Points;

        // Labels
        public string PlotTitle;
        public string XAxisName;
        public string YAxisName;
        public string ZAxisName;

        // Resources
        public GameObject HandleMaterial;
        public GameObject HandleGrabbedMaterial;
        public GameObject RotationHandle;
        public GameObject ScaleHandle;
        public TrailRenderer TrailRenderer;

        // Misc.
        private int Index;
        //private int pointIndex;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Awake()
        {
            Index = 0;
            Points = new List<Transform>();
            //pointIndex = 0;
            CreateTimeSeriesGraphUsingCSV(); // This method will be removed once combined with UI

            Vector3 zero_position = Vector3.zero;

            // Instantiate prefabs for each plot point in Graph
            for (int i = 0; i < Graph.PlotPoints.Count; i++)
            {
                Transform point = Instantiate(PointPrefab);
                point.localPosition = zero_position;
                Points.Add(point);
            }
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
            var csvString = DataFile.ToString(); // Convert CSV to String for easier use
            DataParser parser = new DataParser(csvString);

            // Create PlotPoint object using data from CSV
            List<float> x_values = parser.GetListFromColumn(0); // Grab values from first column
            List<float> y_values = parser.GetListFromColumn(1);
            List<float> z_values = parser.GetListFromColumn(2);
            PlotPoint new_point = new PlotPoint(x_values, y_values, z_values);

            // Create empty Graph object and give it plot points
            Graph = new TimeSeriesGraph();
            Graph.AddPlotPoint(new_point);
        }

        private void UpdatePoint(Transform point, int index)
        {
            Vector3 position;

            // Grab the corresponding point from graph using given index
            PlotPoint pointFromGraph = Graph.PlotPoints[index];

            // Update position vector
            position.x = 0.0f;
            position.y = 0.0f;
            position.z = 0.0f;

            //  Render point with updated position
            point.localPosition = position;
        }

        private void UpdatePoint()
        {
            Vector3 position;

            var current_point = Points[0];

            if (Index < 10)
            {
                position.x = Graph.PlotPoints[0].XPoints[Index];
                position.y = Graph.PlotPoints[0].YPoints[Index];
                position.z = Graph.PlotPoints[0].ZPoints[Index];

                current_point.localPosition = position;

                Index++;
            }
            else
            {
                Index = 0;
            }
        }
    }

}