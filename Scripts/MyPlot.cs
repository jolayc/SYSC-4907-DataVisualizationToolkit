﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class used for testing DynamicPlotter
 */
 
namespace TimeSeriesExtension
{
    public class MyPlot : MonoBehaviour
    {
        public TextAsset SineData;
        public TextAsset DroneData;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Awake()
        {
            TimeSeriesGraph sineGraph = CreateSineGraph();
            TimeSeriesGraph droneGraph = CreateDroneGraph();

            DynamicPlotter sinePlotter = GetPlotter(sineGraph, "Sine Graph");
            DynamicPlotter dronePlotter = GetPlotter(droneGraph, "Drone Graph");
        }

        // Update is called once per frame
        void Update()
        {

        }

        private DynamicPlotter GetPlotter(TimeSeriesGraph graph, string name)
        {
            GameObject plot = new GameObject();

            DynamicPlotter plotter = plot.AddComponent<DynamicPlotter>();

            plotter.Graph = graph;
            Debug.Log("Graph is assigned");
            plotter.PointHolder = plot;

            Transform dataPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            plotter.PointPrefab = dataPoint;
            Destroy(dataPoint.gameObject);

            plotter.Text = Resources.Load("Assets/DataVisualizationToolkit/Plot Objects/text3D.prefab") as GameObject;

            plotter.PlotTitle = name;

            plotter.Init();

            return plotter;
        }

        private TimeSeriesGraph CreateSineGraph()
        {
            TimeSeriesGraph graph = new TimeSeriesGraph();

            string sineString = SineData.ToString();
            DataParser parser = new DataParser(sineString);

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
            graph.AddPlotPoint(new_point);

            new_point = new PlotPoint(x2, y2, z2);
            graph.AddPlotPoint(new_point);

            new_point = new PlotPoint(x3, y3, z3);
            graph.AddPlotPoint(new_point);

            return graph;
        }

        private TimeSeriesGraph CreateDroneGraph()
        {
            TimeSeriesGraph graph = new TimeSeriesGraph();

            string droneString = DroneData.ToString();
            DataParser parser = new DataParser(droneString);

            List<float> x_values = parser.GetListFromColumn(3); //long
            List<float> y_values = parser.GetListFromColumn(2); //lat
            List<float> z_values = parser.GetListFromColumn(4); //alt

            PlotPoint new_point = new PlotPoint(x_values, y_values, z_values);
            graph.AddPlotPoint(new_point);

            return graph;
        }
    }
}

