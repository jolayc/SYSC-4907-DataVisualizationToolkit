using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

/// <summary>
/// TO-DO:
/// - Rename to Time-Dependent-Graph.cs
/// - Allow dynamic (?) support for 2D and 3D plotting
/// - Implement support for 2D and 3D plotting
/// - Implement support for plotting multiple unique points on the graph
/// - Change trail-render to render shadow (?) of point movement instead of the path it takes
/// - Remove GetDataFromCSV and create separate class for it
/// </summary>

public class Graph : MonoBehaviour
{
    // Public
    public Transform PointPrefab;   // Prefab to represent a data point
    public TextAsset CSVData;       // CSV Data asset

    // Private
    private Transform[] Points;     // Collection of points to graph
    private GameObject PointHolder; // Used as Parent object to hold each point of plot
    private TrailRenderer Trail;

    // Graph resources
    private List<float> XPoints, YPoints, ZPoints;

    // Maximum and minimum values of data to plot
    private float XMax, YMax, ZMax;
    private float XMin, YMin, ZMin;

    // Labels
    [Tooltip("Plot Title")]
    public string PlotLabel;
    [Tooltip("X-axis label")]
    public string XLabel;
    [Tooltip("Y-axis label")]
    public string YLabel;
    [Tooltip("Z-axis label")]
    public string ZLabel;

    // Other
    readonly int n = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % n == 0) // Adjust how often you want to update each points position
        {
            Vector3 position;

        }
    }

    private void Awake()
    {
        // Set up initial resources
        GetDataFromCSV();
        SetMaxPoints();
        SetMinPoints();
    }

    private void SetMaxPoints()
    {
        XMax = XPoints.Select(System.Math.Abs).Max();
        YMax = YPoints.Select(System.Math.Abs).Max();
        //ZMax = ZPoints.Select(System.Math.Abs).Max();
    }

    private void SetMinPoints()
    {
        XMin = XPoints.Min();
        YMin = YPoints.Min();
        //ZMin = ZPoints.Min();
    }

    // Ensure values are within range (-1, 1)
    private float Normalize(float value, float max, float min)
    {
        // If values are all constant or zero
        if (System.Math.Abs(max - min) < float.Epsilon)
        {
            return value;
        }
        else
        {
            return (value - min) / (max - min);
        }
    }

    private void GetDataFromCSV()
    {
        XPoints = new List<float>();
        YPoints = new List<float>();
        //ZPoints = new List<float>();

        var dataString = CSVData.ToString();    // Convert .csv text asset to string object
        Debug.Log("Data string" + dataString);

        // Parse through dataString and append corresponding points to appropriate list
        using (var reader = new StringReader(dataString))
        {
            while (true)
            {
                var line = reader.ReadLine();
                if (line != null)
                {
                    // Comma denotes a separate column
                    var values = line.Split(',');
                    var x = float.Parse(values[0]);
                    var y = float.Parse(values[1]);
                    XPoints.Add(x);
                    YPoints.Add(y);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
