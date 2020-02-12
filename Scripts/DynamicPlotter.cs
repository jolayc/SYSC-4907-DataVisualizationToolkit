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
        private GameObject TimeText;

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
            enabled = false;
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

        public void Init()
        {
            PlotScale = 1;
            Points = new List<Transform>();

            Debug.Log(Graph);

            SetMaxMinMid();
            DrawPlot();
            DrawTitle();
            DrawTime();

            enabled = true;
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

                if (Graph.isTimeGraph())
                {
                    UpdateTime(currentIndex);
                }

                pointFromGraph.currentPointIndex++;
            }
            else
            {
                pointFromGraph.currentPointIndex = 0;
            }
        }

        private void UpdateTime(int index)
        {
            List<string> timePoints = Graph.TimePoints;
            TimeText.GetComponent<TextMesh>().text = "Time: " + timePoints[index];
        }
            
        private void DrawTitle()
        {
            Vector3 graphExtent = PointHolder.GetComponent<BoxCollider>().size / 2;

            GameObject plotTitle = Instantiate(Text, new Vector3(
                                               NormalizeToRange(graphExtent.x * -1, graphExtent.x, GraphXMid, GraphXMax, GraphXMin),
                                               NormalizeToRange(graphExtent.y * -1, graphExtent.y, GraphYMid, GraphYMax, GraphYMin),
                                               NormalizeToRange(graphExtent.z * -1, graphExtent.z, GraphZMid, GraphZMax, GraphZMin)),
                                               Quaternion.identity);

            // Add title text
            plotTitle.transform.SetParent(PointHolder.transform);
            plotTitle.GetComponent<TextMesh>().text = PlotTitle;
            plotTitle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            plotTitle.transform.position = plotTitle.transform.position + new Vector3(0, graphExtent.y, 0);
        }

        private void DrawTime()
        {
            Vector3 graphExtent = PointHolder.GetComponent<BoxCollider>().size / 2;

            TimeText = Instantiate(Text, new Vector3(
                                               NormalizeToRange(graphExtent.x * -1, graphExtent.x, GraphXMid, GraphXMax, GraphXMin),
                                               NormalizeToRange(graphExtent.y * -1, graphExtent.y, GraphYMid, GraphYMax, GraphYMin),
                                               NormalizeToRange(graphExtent.z * -1, graphExtent.z, GraphZMid, GraphZMax, GraphZMin)),
                                               Quaternion.identity);

            // Add time text
            TimeText.transform.SetParent(PointHolder.transform);
            TimeText.GetComponent<TextMesh>().text = "";
            TimeText.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            TimeText.transform.position = TimeText.transform.position - new Vector3(0, graphExtent.y, 0);
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

        //private void NormalizeToRange(float a, float b, float normalized_value)
        //{
        //    return ((b - a) * normalized_value) + a;
        //}

        private float GetMiddle(float max, float min)
        {
            return (max + min) / 2;
        }
    }
}
