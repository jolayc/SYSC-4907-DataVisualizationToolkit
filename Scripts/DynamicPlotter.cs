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
            DrawPlot();
            DebugPlot();
            DrawTitle();
        }

        // Update is called once per frame
        void Update()
        {
            //if (Time.frameCount % 2 == 0)
            //{
                for (int i = 0; i < Points.Count; i++)
                {
                    UpdatePoint(Points[i], i);
                }
          //  }
        }

        private void DebugPlot()
        {
            // TO-DO
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
                current_point.SetParent(PointHolder.GetComponent<BoxCollider>().transform);
                current_point.localPosition = PointHolder.GetComponent<BoxCollider>().center;
                current_point.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                Points.Add(current_point);
            }
        }

        private void UpdatePoint(Transform point, int index)
        {
            Vector3 updated_position;

            PlotPoint pointFromGraph = Graph.PlotPoints[index];
            int currentIndex = pointFromGraph.currentPointIndex;

            // Get bounds of BoxCollider to help with normalizing plot point
            Vector3 max_range = PointHolder.GetComponent<BoxCollider>().size / 2;
            Vector3 min_range = PointHolder.GetComponent<BoxCollider>().size / 2 * -1;

            //Debug.Log("max range: " + max_range + " min range: " + min_range);

            if (currentIndex < pointFromGraph.XPoints.Count)
            {
                updated_position.x = NormalizeToRange(min_range.x, max_range.x, pointFromGraph.XPoints[currentIndex], GraphXMax, GraphXMin);
                updated_position.y = NormalizeToRange(min_range.y, max_range.y, pointFromGraph.YPoints[currentIndex], GraphYMax, GraphYMin);
                updated_position.z = NormalizeToRange(min_range.z, max_range.z, pointFromGraph.ZPoints[currentIndex], GraphZMax, GraphZMin);

                point.localPosition = updated_position;

                pointFromGraph.currentPointIndex++;

            }
            else
            {
                pointFromGraph.currentPointIndex = 0;
            }
        }
            
        private void DrawTitle()
        {
            Vector3 graphExtent = PointHolder.GetComponent<BoxCollider>().size / 2;

            GameObject plotTitle = Instantiate(Text, new Vector3(
                                               NormalizeToRange(graphExtent.x * -1, graphExtent.x, GraphXMid, GraphXMax, GraphXMin),
                                               NormalizeToRange(graphExtent.y * -1, graphExtent.y, GraphYMid, GraphYMax, GraphYMin),
                                               NormalizeToRange(graphExtent.z * -1, graphExtent.z, GraphZMid, GraphZMax, GraphZMin)),
                                               Quaternion.identity);

            // Add title
            plotTitle.transform.SetParent(PointHolder.transform);
            plotTitle.GetComponent<TextMesh>().text = PlotTitle;
            plotTitle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            plotTitle.transform.position = plotTitle.transform.position + new Vector3(0, graphExtent.y, 0);
        }

        private void DrawTime()
        {
            // To-do
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

        private float NormalizeToRange(float a, float b, float value, float min, float max)
        {
            // Range [a, b]
            float normalized_value = Normalize(value, max, min);
            return ((b - a) * normalized_value) + a;
        }


        private float GetMiddle(float max, float min)
        {
            return (max + min) / 2;
        }
    }
}
