using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeSeriesExtension
{
    public class TimeSeriesPlotter : MonoBehaviour
    {
        public Transform PointPrefab;
        public TextAsset DataFile;

        private TimeSeriesGraph Graph;

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

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Awake()
        {
            CreateTimeSeriesGraphWithCSV(); // This method will be removed once combined with UI

            // Initialize point prefab for each point in graph
            foreach (PlotPoint point in Graph.PlotPoints)
            {
                Instantiate(point.PointPrefab);
            }
        }

        /*
         * Temporary method to use CSV parsing when creating graph
         */ 
        private void CreateTimeSeriesGraphWithCSV()
        {
            var csvString = DataFile.ToString(); // Convert CSV to String for easier use
            DataParser parser = new DataParser(csvString);

            // Create PlotPoint object using data from CSV
            List<float> x_values = parser.GetListFromColumn(0); // Grab values from first column
            List<float> y_values = parser.GetListFromColumn(1);
            List<float> z_values = parser.GetListFromColumn(2);
            PlotPoint new_point = new PlotPoint(PointPrefab, x_values, y_values, z_values);

            // Create empty Graph object and give it plot points
            Graph = new TimeSeriesGraph();
            Graph.AddPlotPoint(new_point);
        }

        // Update is called once per frame
        void Update()
        {

        }

        ///
        /* FOR DEBUGGING */
        //
        private void LogValues(List<float> values)
        {
            foreach (var value in values)
            {
                Debug.Log(value);
            }
        }

        public void PrintValues(List<float> list, string label)
        {
            foreach (var elem in list)
            {
                Debug.Log(label + ": " + elem);
            }
        }
    }

}