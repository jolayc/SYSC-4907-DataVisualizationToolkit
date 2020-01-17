using System.Collections;
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
        public List<PlotPoint> PlotPoints = new List<PlotPoint>();

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
            Instantiate(PointPrefab);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void Awake()
        {

        }

        public void AddPlotPoint(PlotPoint point)
        {
            PlotPoints.Add(point);
        }
    }
}