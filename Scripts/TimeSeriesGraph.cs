using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeSeriesExtension
{
    public class TimeSeriesGraph
    {
        public List<PlotPoint> PlotPoints { get; set; }

        public float XMax { get; set; }
        public float YMax { get; set; }
        public float ZMax;

        public float XMin;
        public float YMin;
        public float ZMin;

        /*
         * Constructor used when no initial data is provided
         */ 
        public TimeSeriesGraph()
        {
            PlotPoints = new List<PlotPoint>();
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

            CalculateMaxPoints();
            CalculateMinPoints();
        }

        private void CalculateMaxPoints()
        {
            float xmax, ymax, zmax;

            xmax = PlotPoints[0].XMax;
            ymax = PlotPoints[0].YMax;
            zmax = PlotPoints[0].ZMax;

            foreach (var point in PlotPoints)
            {
                float current_x = point.XMax;
                float current_y = point.YMax;
                float current_z = point.ZMax;

                if (current_x > xmax) xmax = current_x;
                if (current_y > ymax) ymax = current_y;
                if (current_y > zmax) zmax = current_z;
            }

            XMax = xmax;
            YMax = ymax;
            ZMax = zmax;
        }

        private void CalculateMinPoints()
        {
            float xmin, ymin, zmin;

            xmin = PlotPoints[0].XMin;
            ymin = PlotPoints[0].YMin;
            zmin = PlotPoints[0].ZMin;

            foreach (var point in PlotPoints)
            {
                float current_x = point.XMin;
                float current_y = point.YMin;
                float current_z = point.ZMin;

                if (current_x < xmin) xmin = current_x;
                if (current_y < ymin) ymin = current_y;
                if (current_z < zmin) zmin = current_z;
            }

            XMin = xmin;
            YMin = ymin;
            ZMin = zmin;
        }

        public string LogMax()
        {
            return "(XMax, YMax, ZMax): " + XMax + "," + YMax + "," + ZMax;
        }

        public string LogMin()
        {
            return "(XMin, YMin, ZMin): " + XMin + "," + YMin + "," + ZMin;
        }
    }
}