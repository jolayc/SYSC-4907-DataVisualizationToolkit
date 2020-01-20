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

        private TimeSeriesGraph Graph;
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
                //point.SetParent(PointHolder.transform);
                point.localPosition = Vector3.zero;
                Points.Add(point);
            }

            // Initialize plot manipulation controls
            //InitializeInteraction();
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

            int currentIndex = pointFromGraph.currentPointIndex;

            if (currentIndex < pointFromGraph.XPoints.Count)
            {
                // Update position vector
                position.x = pointFromGraph.XPoints[currentIndex];
                position.y = pointFromGraph.YPoints[currentIndex];
                position.z = pointFromGraph.ZPoints[currentIndex];

                //  Render point with updated position
                point.localPosition = position;

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

            PointHolder.AddComponent<BoundingBox>();
            PointHolder.AddComponent<ManipulationHandler>();

            //Scale handle sizes
            PointHolder.GetComponent<BoundingBox>().ScaleHandleSize = PointHolder.GetComponent<BoundingBox>().ScaleHandleSize * PlotScale;
            PointHolder.GetComponent<BoundingBox>().RotationHandleSize = PointHolder.GetComponent<BoundingBox>().RotationHandleSize * PlotScale;


            //Optional handle prefab Models
            PointHolder.GetComponent<BoundingBox>().HandleGrabbedMaterial = HandleGrabbedMaterial;
            PointHolder.GetComponent<BoundingBox>().HandleMaterial = HandleMaterial;
            PointHolder.GetComponent<BoundingBox>().ScaleHandlePrefab = ScaleHandle;
            PointHolder.GetComponent<BoundingBox>().RotationHandleSlatePrefab = RotationHandle;

            // Optional appBar
            // TO-DO
        }
    }

}