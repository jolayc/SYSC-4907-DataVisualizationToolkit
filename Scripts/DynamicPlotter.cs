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
            //Application.targetFrameRate = 60;
        }

        private void Awake()
        {
            Points = new List<Transform>();

            CreateGraph();
            SetMaxMinMid();
            DrawPlot();
            InitializeInteraction();
        }

        // Update is called once per frame
        void Update()
        {
            //for (int i = 0; i < Points.Count; i++)
            //{
            //    UpdatePoint(Points[i], i);
            //}
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

            for (int i = 0; i < numberOfPoints; i++)
            {
                Transform current_point = Instantiate(PointPrefab);
                current_point.GetComponent<Renderer>().material.color = Random.ColorHSV(0.0f, 1.0f);
                current_point.SetParent(PointHolder.transform);
                current_point.localPosition = Vector3.zero;
                current_point.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                Points.Add(current_point);
            }

            PointHolder.transform.position = new Vector3(Normalize(GraphXMid, GraphXMax, GraphXMin),
                                                         Normalize(GraphYMid, GraphYMax, GraphYMin),
                                                         Normalize(GraphZMid, GraphZMax, GraphZMin));
        }

        private void UpdatePoint(Transform point, int index)
        {

        }

        private float Normalize(float value, float max, float min)
        {
            if (max - min == 0)
            {
                return value;
            }
            else
            {
                float result = (value - min) / max - min;
                Debug.Log(result);
                return result;
            }
        }

        private float GetMiddle(float max, float min)
        {
            return (max + min) / 2;
        }

        private void InitializeInteraction()
        {
            BoxCollider boxCollider = PointHolder.AddComponent<BoxCollider>();

            PointHolder.AddComponent<BoundingBox>();
            PointHolder.GetComponent<BoundingBox>().WireframeMaterial.color = Color.white;
            PointHolder.AddComponent<ManipulationHandler>();
        }
    }
}
