using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeSeriesExtension
{
    public class TimeSeriesGraph
    {
        public List<PlotPoint> PlotPoints { get; set; }

        private float XMax;
        private float YMax;
        private float ZMax;

        private float XMin;
        private float YMin;
        private float ZMin;

        /*
         * Constructor used when no initial data is provided
         */ 
        public TimeSeriesGraph()
        {
            PlotPoints = new List<PlotPoint>();
            //CalculateMaxPoints();
            //CalculateMinPoints();
        }

        /*
         * Constructor used when initial data is provided
         */ 
        public TimeSeriesGraph(List<PlotPoint> points)
        {
            PlotPoints = points;
        }

        public void AddPlotPoint(PlotPoint point)
        {
            PlotPoints.Add(point);
        }

        private void CalculateMaxPoints()
        {
            
        }

        private void CalculateMinPoints()
        {

        }
    }
}