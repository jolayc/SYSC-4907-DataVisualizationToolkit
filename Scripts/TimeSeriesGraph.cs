﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeSeriesExtension
{
    public class TimeSeriesGraph : MonoBehaviour
    {
        public Transform PointPrefab; // Asset used to represent a single point in the plot

        // Labels
        public string PlotTitle;
        public string XAxisName;
        public string YAxisName;
        public string ZAxisName;

        // Plot points
        public List<PlotPoint> PlotPoints;

        // Plot members
        public GameObject AppBar;
        public GameObject RotationHandle;
        public GameObject ScaleHandle;
        public GameObject Text;
        public Material HandleMaterial;
        public Material HandleGrabbedMaterial;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void Awake()
        {
            Instantiate(PointPrefab);
            PlotPoints = new List<PlotPoint>();
        }

        public void AddPlotPoint(PlotPoint point)
        {
            PlotPoints.Add(point);
        }

        public void ShowValues()
        {
            foreach(PlotPoint point in PlotPoints)
            {
                point.PrintValues();
            }
        }
    }
}