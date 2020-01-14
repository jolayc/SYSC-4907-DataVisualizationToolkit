using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Data structure that the DataPlotter will refer to
 * to plot and update a point in its graph
 */

public class PlotPoint
{
    private Transform Point { get; set; }
    private List<float> XPoints { get; set; }
    private List<float> YPoints { get; set; }
    private List<float> ZPoints { get; set; }

    private float XMax;
    private float XMin;
    private float YMax;

    private float YMin;
    private float ZMax;
    private float ZMin;

    public PlotPoint(Transform point, List<float> xpoints, List<float> ypoints, List<float> zpoints)
    {
        Point = point;
        XPoints = xpoints;
        YPoints = ypoints;
        ZPoints = zpoints;

        CalculateMaxPoints();
        CalculateMinPoints();
    }

    private void CalculateMaxPoints()
    {
        XMax = XPoints.Select(System.Math.Abs).Max();
        YMax = YPoints.Select(System.Math.Abs).Max();
        ZMax = ZPoints.Select(System.Math.Abs).Max();
    }

    private void CalculateMinPoints()
    {
        XMin = XPoints.Min();
        YMin = YPoints.Min();
        ZMin = ZPoints.Min();
    }
}
