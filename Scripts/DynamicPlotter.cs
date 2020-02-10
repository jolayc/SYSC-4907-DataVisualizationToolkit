using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

namespace TimeSeriesExtension
{
    public class DynamicPlotter : MonoBehaviour
    {
        // for DynamicPlotter
        public GameObject Text;
        public GameObject PointHolder;

        public Transform PointPrefab;

        public TextAsset DataFile;

        public TimeSeriesGraph Graph;

        private List<Transform> Points;

        // Labels
        public string PlotTitle;
        public string XAxisName, YAxisName, ZAxisName;

        // Graph resources
        public Material HandleMaterial;
        public Material HandleGrabbedMaterial;
        public GameObject RotationHandle;
        public GameObject ScaleHandle;
        public float PlotScale;

        // from Graph
        private float GraphXMax, GraphXMid, GraphXMin;
        private float GraphYMax, GraphYMid, GraphYMin;
        private float GraphZMax, GraphZMid, GraphZMin;

        // Start is called before the first frame update
        void Start()
        {
            // Lock scene rendering to 60 fps
            //Application.targetFrameRate = 60;
        }

        private void Awake()
        {
            PlotScale = 1;
            Points = new List<Transform>();

            CreateGraph();
            SetMaxMinMid();
            DebugMaxMidMin();
            DrawPlot();
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.frameCount % 2 == 0)
            {
                for (int i = 0; i < Points.Count; i++)
                {
                    UpdatePoint(Points[i], i);
                }
            }
        }

        private void DebugPlot()
        {
            Vector3 max_bounds = PointHolder.GetComponent<BoxCollider>().bounds.max;
            Vector3 min_bounds = PointHolder.GetComponent<BoxCollider>().bounds.min;
            Vector3 center = PointHolder.GetComponent<BoxCollider>().size;

            Debug.Log("max bounds: " + max_bounds);
            Debug.Log("min bounds: " + min_bounds);
            Debug.Log("center: " + center);

            Debug.Log(NormalizeInRange(-0.5f, 0.5f, 100f, 0f, 100f));
        }

        private void DebugDrone()
        {
            Debug.Log("X,Y,Z max: " + Graph.XMax + "," + Graph.YMax + "," + Graph.ZMax);
            Debug.Log("X,Y,Z mid: " + Graph.XMid + "," + Graph.YMid + "," + Graph.ZMid);
            Debug.Log("X,Y,Z min: " + Graph.XMin + "," + Graph.YMin + "," + Graph.ZMin);
        }

        private void SetMaxMinMid()
        {
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
        }

        private void CreateGraph()
        {
            Graph = new TimeSeriesGraph();

            string csvString = DataFile.ToString();
            DataParser parser = new DataParser(csvString);

            List<float> x1 = parser.GetListFromColumn(0);
            List<float> y1 = parser.GetListFromColumn(1);
            List<float> z1 = parser.GetListFromColumn(2);
            List<float> x2 = parser.GetListFromColumn(3);
            List<float> y2 = parser.GetListFromColumn(4);
            List<float> z2 = parser.GetListFromColumn(5);
            List<float> x3 = parser.GetListFromColumn(6);
            List<float> y3 = parser.GetListFromColumn(7);
            List<float> z3 = parser.GetListFromColumn(8);

            PlotPoint new_point = new PlotPoint(x1, y1, z1);
            Graph.AddPlotPoint(new_point);

            new_point = new PlotPoint(x2, y2, z2);
            Graph.AddPlotPoint(new_point);

            new_point = new PlotPoint(x3, y3, z3);
            Graph.AddPlotPoint(new_point);
        }

        private void CreateDronePlot()
        {
            Graph = new TimeSeriesGraph();

            string csvString = DataFile.ToString();
            DataParser parser = new DataParser(csvString);

            List<float> x_values = parser.GetListFromColumn(3); // longitude
            List<float> y_values = parser.GetListFromColumn(2); // latitude
            List<float> z_values = parser.GetListFromColumn(4); // altitude

            PlotPoint new_point = new PlotPoint(x_values, y_values, z_values);
            Graph.AddPlotPoint(new_point);
        }

        private void DrawPlot()
        {
            int numberOfPoints = Graph.PlotPoints.Count;

            PointHolder.transform.position = Vector3.zero; // Center pivot to origin

            // Configure MRTK components of Graph, e.g. BoundingBox and ManipulationHandler
            PointHolder.AddComponent<BoxCollider>();
            PointHolder.AddComponent<BoundingBox>();
            PointHolder.GetComponent<BoundingBox>().WireframeMaterial.color = Color.white;
            PointHolder.AddComponent<ManipulationHandler>();

            for (int i = 0; i < numberOfPoints; i++)
            {
                Transform current_point = Instantiate(PointPrefab);
                current_point.GetComponent<Renderer>().material.color = Random.ColorHSV(0.0f, 1.0f);
                current_point.SetParent(PointHolder.transform);
                current_point.localPosition = PointHolder.GetComponent<BoxCollider>().center;
                current_point.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                Points.Add(current_point);
            }
        }

        private void DrawTitle()
        {
            // Configure plot title
            GameObject plotTitle = Instantiate(Text, new Vector3(Normalize(GraphXMid, GraphXMax, GraphXMin),
                                                                 Normalize(GraphYMid, GraphYMax, GraphYMin),
                                                                 Normalize(GraphZMid, GraphZMax, GraphZMin)),
                                                                 Quaternion.identity);
            // Render title text
            plotTitle.transform.SetParent(PointHolder.transform);
            plotTitle.GetComponent<TextMesh>().text = PlotTitle;
            plotTitle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            plotTitle.transform.position = plotTitle.transform.position + new Vector3(0, Normalize(GraphYMax, GraphYMax, GraphYMin), 0);
        }

        private void DrawTime()
        {
            // To-do
        }

        // THIS BETTER WORK
        private void UpdatePoint(Transform point, int index)
        {
            Vector3 updated_position;

            PlotPoint pointFromGraph = Graph.PlotPoints[index];
            int currentIndex = pointFromGraph.currentPointIndex;

            // Bounds of PointHolder to use when normalizing the updated_position of a point
            Vector3 max_range = PointHolder.GetComponent<BoxCollider>().bounds.max;
            Vector3 min_range = PointHolder.GetComponent<BoxCollider>().bounds.min;

            if (currentIndex < pointFromGraph.XPoints.Count)
            {
                updated_position.x = NormalizeInRange(min_range.x, max_range.x, pointFromGraph.XPoints[currentIndex], GraphXMax, GraphXMin);
                updated_position.y = NormalizeInRange(min_range.y, max_range.y, pointFromGraph.YPoints[currentIndex], GraphYMax, GraphYMin);
                updated_position.z = NormalizeInRange(min_range.z, max_range.z, pointFromGraph.ZPoints[currentIndex], GraphZMax, GraphZMin);

                // Update point position in local space (to PointHolder)
                point.localPosition = updated_position;

                // Update current plot point's index
                pointFromGraph.currentPointIndex++;
            }

            else
            {
                pointFromGraph.currentPointIndex = 0;
            }
        }

        private float Normalize(float value, float max, float min)
        {
            if (max - min == 0)
            {
                return value;
            }
            else
            {
                float result = (value - min) / (max - min);
                return result;
            }
        }

        private float NormalizeInRange(float a, float b, float value, float min, float max)
        {
            // Normalize a value between range [a, b]
            float normalized = (value - min) / (max - min);
            return ((b - a) * normalized) + a;
        }

        private float GetMiddle(float max, float min)
        {
            return (max + min) / 2;
        }

        private void InitializeInteraction()
        {
            BoxCollider boxCollider = PointHolder.AddComponent<BoxCollider>();
            PointHolder.transform.gameObject.GetComponent<BoxCollider>().size = new Vector3(Normalize(GraphXMid, GraphXMax, GraphXMin),
                                                                                            Normalize(GraphYMid, GraphYMax, GraphYMin),
                                                                                            Normalize(GraphZMid, GraphZMax, GraphZMin)) * PlotScale;

            PointHolder.AddComponent<BoundingBox>();
            PointHolder.GetComponent<BoundingBox>().WireframeMaterial.color = Color.white;
            PointHolder.AddComponent<ManipulationHandler>();
        }

        // Debug functions
        private void DebugMaxMidMin()
        {
            Debug.Log("X-Max, X-Mid, X-Min: " + GraphXMax + "," + GraphXMid + "," + GraphXMin);
            Debug.Log("Y-Max, Y-Mid, Y-Min: " + GraphYMax + "," + GraphYMid + "," + GraphYMin);
            Debug.Log("Z-Max, Z-Mid, Z-Min: " + GraphZMax + "," + GraphZMid + "," + GraphZMin);

        }

        private void DebugPositions()
        {
            Vector3 position;
            PlotPoint current_point = Graph.PlotPoints[0];
            int index = current_point.currentPointIndex;

            position.x = Normalize(current_point.XPoints[0], GraphXMax, GraphXMin);
            position.y = Normalize(current_point.YPoints[0], GraphXMax, GraphXMin);
            position.z = Normalize(current_point.ZPoints[0], GraphZMax, GraphZMin);

            Debug.Log(position);

        }
    }
}
