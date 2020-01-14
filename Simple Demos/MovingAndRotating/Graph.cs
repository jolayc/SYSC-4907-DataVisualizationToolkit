using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

namespace MovingAndRotating
{
    public class Graph : MonoBehaviour
	{
        // Unity / MR Assets
        public Transform PointPrefab;
        public TextAsset Data;

        private Transform Point;
        private TrailRenderer Trail;

        // Graph resources
        // (X, Y, Z, orientation)
        private List<float> XPoints, YPoints, ZPoints, RPoints;

        private float XMax, YMax, ZMax;
        private float XMin, YMin, ZMin;

        // Labels

        private void Awake()
        {
            
        }

        private void GetMaxValues()
        {

        }

        private void GetMinValues()
        {

        }
    }
}
