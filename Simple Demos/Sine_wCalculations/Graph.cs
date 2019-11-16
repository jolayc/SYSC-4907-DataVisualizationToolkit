using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

/*
 * Class for animated graph object
 */

public class Graph : MonoBehaviour
{
    public Transform PointPrefab;           // i.e., representation of a point in the graph
    public Transform[] Points;              // collection of points in graph
    public GraphFunctionName Function;      // Graphing function being used
    static GraphFunction[] Functions = {    // Graphing functions available
        Sine.SineFunction, Sine.Sine2DFunction, Sine.MultiSineFunction
    };

    // Graph labels for each axis
    [Tooltip("X Axis Label")]
    public string XLabel;
    //[Tooltip("Y-Axis label")]
    public string YLabel = "Time (s)";
    [Tooltip("Z-Axis Label")]
    public string ZLabel;
    [Tooltip("Title of Graph")]
    public string GraphTitle;
    //[Tooltip("Plot Scale")]
    //public float plotScale = 10;

    [Range(10, 100)]
    public int Resolution = 10;             // Default value is 10 data points

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        // Update the y position of each point every frame w.r.t. time
        float time = Time.time;
        GraphFunction func = Functions[(int)Function];
        for (int i = 0; i < Points.Length; i++)
        {
            Transform currentPoint = Points[i];
            Vector3 position = currentPoint.localPosition;
            position.y = func(position.x, position.z, time);
            currentPoint.localPosition = position;
        }
    }

    void Awake()
    {
        CalculateXZPoints();
    }

    void CalculateXPoints()
    {
        float step = 2f / Resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position;

        position.y = 0f;
        position.z = 0f;

        float max_x = 0f;

        Points = new Transform[Resolution];

        for (int i = 0; i < Resolution; i++)
        {
            Transform point = Instantiate(PointPrefab);
            position.x = (i + 0.5f) * step - 1f;
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform, false);
            Points[i] = point;
        }
    }

    float GetMax(float a, float b)
    {
        float max = (a > b) ? max = a : max = b;
        return max;
    }

    void CalculateXZPoints()
    {
        float step = 2f / Resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position;

        position.y = 0f;
        position.z = 0f;

        Points = new Transform[Resolution * Resolution];

        for (int i = 0, z = 0; z < Resolution; z++)
        {
            position.z = (z + 0.5f) * step - 1f;
            for (int x = 0; x < Resolution; x++, i++)
            {
                Transform point = Instantiate(PointPrefab);
                position.x = (x + 0.5f) * step - 1f;
                point.localPosition = position;
                point.localScale = scale;
                point.SetParent(transform, false);
                Points[i] = point;
            }
        }
    }

    // Draw X, Y and Z-axis labels of graph
    void DrawLabels()
    {

    }

    /*
     * Find a way to incorporate this function instead of using the magic
     * numbers used to calculate the X, Y, Z points
     */
    float normalize(float value, float max, float min)
    {
        // If values are zero or constant
        if (System.Math.Abs(max - min) < float.Epsilon)
        {
            return value;
        }
        else
        {
            return (value - min) / (max - min);
        }
    }
}
