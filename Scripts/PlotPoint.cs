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
    public Transform PointPrefab { get; set; } // Asset used to represent a point

    public List<float> XPoints { get; set; }
    public List<float> YPoints { get; set; }
    public List<float> ZPoints { get; set; }

    public float XMax;
    public float XMin;
    public float YMax;

    public float YMin;
    public float ZMax;
    public float ZMin;

    public PlotPoint(Transform point, List<float> xpoints, List<float> ypoints, List<float> zpoints)
    {
        PointPrefab = point;
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
