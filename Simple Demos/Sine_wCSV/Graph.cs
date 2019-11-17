using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

namespace SineWCSV
{
    public class Graph : MonoBehaviour
    {
        // Graph assets
        public Transform PointPrefab;
        private Transform Point;
        public TextAsset DataFile;
        private Transform[] Points;

        // Graph resources
        private List<float> XPoints;
        private List<float> YPoints;
        private List<float> ZPoints;

        private float XMax;
        private float YMax;
        private float ZMax;

        private float XMin;
        private float YMin;
        private float ZMin;

        // Labels
        [Tooltip("Plot title")]
        public string PlotTitle;
        [Tooltip("X-axis label")]
        public string XAxisName;
        [Tooltip("Y-axis label")]
        public string YAxisName;
        [Tooltip("Z-axis label")]
        public string ZAxisName;

        private int index;
        private TrailRenderer trender;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            ////Render points w.r.t.time
            Vector3 position;
            position.z = 0f;

            if (Time.frameCount % 30 == 0)
            {
                if (Point.position.x >= XMax)
                {
                    index = 0;
                    trender.Clear();
                }
                else
                {
                    index++;
                }
                position.x = XPoints[index];
                position.y = YPoints[index];
                Point.localPosition = position;
                Debug.Log(index);
            }
        }

        private void Awake()
        {
            index = 0;

            GetDataPointsFromCSV();
            CalcMaxValues();
            CalcMinValues();
            //LogValues();

            Point = Instantiate(PointPrefab);

            trender = Point.GetComponent<TrailRenderer>();

            //PopulatePoints();
        }

        private void GetDataPointsFromCSV()
        {
            XPoints = new List<float>();
            YPoints = new List<float>();
            //ZPoints = new List<float>();

            var dataString = DataFile.ToString();
            Debug.Log("Data string" + dataString);

            using (var reader = new StringReader(dataString))
            {
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
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

        private void CalcMaxValues()
        {
            XMax = XPoints.Select(System.Math.Abs).Max();
            YMax = YPoints.Select(System.Math.Abs).Max();
        }

        private void CalcMinValues()
        {
            XMin = XPoints.Min();
            YMin = YPoints.Min();
        }

        private void LogValues()
        {
            Debug.Log("X Max:" + XMax);
            Debug.Log("Y Max:" + YMax);
            Debug.Log("X Min:" + XMin);
            Debug.Log("Y Min:" + YMin);
            foreach (var x in XPoints)
            {
                Debug.Log("X:" + x);
            }
            foreach (var y in YPoints)
            {
                Debug.Log("Y:" + y);
            }
        }
    }
}
