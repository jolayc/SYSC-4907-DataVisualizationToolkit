using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Animated Graph object

public class Graph : MonoBehaviour
{
    public Transform PointPrefab;       // i.e., representation of a point in the graph
    public Transform[] Points;          // collection of points in graph
    //public GraphFunctionName Function;  // Graphing function being used

    [Tooltip("X Axis Label")]
    public string XLabel;
    //[Tooltip("Y-Axis label")]
    public string YLabel = "Time (s)";
    [Tooltip("Z-Axis Label")]
    public string ZLabel;
    [Tooltip("Title of Graph")]
    public string GraphTitle;

    [Range(10, 100)]
    public int Resolution = 10;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        // Update the y position of each point every frame w.r.t. time
        float time = Time.time;

    }

    void Awake()
    {
        CalculateXPoints();
        DrawLabels();
    }

    void CalculateXPoints()
    {
        float step = 2f / Resolution;
    }

    // Draw X, Y and Z-axis labels of graph
    void DrawLabels()
    {
        
    }
}
