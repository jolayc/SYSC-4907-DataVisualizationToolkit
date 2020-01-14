using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeSeriesExtension
{
    public class ApplicationManager : MonoBehaviour
    {
        public GameObject Graph;
        public TextAsset DataFile;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Awake()
        {
            Debug.Log("Test");
            // Create and empty graph and add it to the scene
            //Instantiate(Graph);

            // Parse through CSV
            var csvString = DataFile.ToString(); // Convert CSV to String for easier use
            DataParser parser = new DataParser(csvString);

            List<float> values = parser.GetListFromColumn(1); // Grab values from first column

            //LogValues(values);
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
    }

}