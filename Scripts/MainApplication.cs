using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeSeriesExtension
{
    public class MainApplication : MonoBehaviour
    {
        public TimeSeriesGraph Graph;
        public TextAsset DataFile;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Awake()
        {
            // Create and empty graph and add it to the scene

            // Parse through CSV
            var csvString = DataFile.ToString(); // Convert CSV to String for easier use
            DataParser parser = new DataParser(csvString);

            List<float> x_values = parser.GetListFromColumn(0); // Grab values from first column
            List<float> y_values = parser.GetListFromColumn(1);
            List<float> z_values = ZValues();

            //LogValues(values);
            Instantiate(Graph);
            PlotPoint new_point = new PlotPoint(Graph.PointPrefab, x_values, y_values, z_values);
            //Graph.AddPlotPoint(new_point);

            //Graph.ShowValues();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /**
         * Function used for logging values
         */
        private void LogValues(List<float> values)
        {
            foreach (var value in values)
            {
                Debug.Log(value);
            }
        }
        private List<float> ZValues()
        {
            List<float> values = new List<float>();
            for (int i = 0; i < 10; i++)
            {
                values.Add(0.0f);
            }
            return values;
        }
    }

}