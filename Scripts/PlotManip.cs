using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
//using UnityEditor;

public class PlotManip : MonoBehaviour
{
    public GameObject Holder;
    public Transform PointPrefab;
    public Transform Point;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        float xmax = 25.0f;
        float ymax = 25.0f;
        float zmax = 25.0f;

        float xmin = -1.0f;
        float ymin = -1.0f;
        float zmin = 0.0f;

        float xmid = GetMiddle(xmax, xmin);
        float ymid = GetMiddle(ymax, ymin);
        float zmid = GetMiddle(zmax, zmin);

        Holder.transform.position = new Vector3(Normalize(xmid, xmax, xmin),
                                                Normalize(ymid, ymax, ymin),
                                                Normalize(zmid, zmax, zmin));

        Point = Instantiate(PointPrefab, Vector3.zero, Quaternion.identity);

        Point.SetParent(Holder.transform);

        InitializeInteraction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeInteraction()
    {
        BoxCollider boxCollider = Holder.AddComponent<BoxCollider>();

        Holder.AddComponent<BoundingBox>();
        Holder.GetComponent<BoundingBox>().WireframeMaterial.color = Color.white;
        Holder.AddComponent<ManipulationHandler>();
    }

    private void DebugNormalization()
    {
        float max = 24.0f;
        float min = 0.0f;

        for (int i = 0; i < 24; i++)
        {
            Debug.Log(Normalize(i, max, min));
        }
    }

    private float Normalize(float value, float max, float min)
    {
        if (max - min == 0)
        {
            return value;
        }
        else
        {
            return (value - min) / (max - min);
        }
    }

    private float GetMiddle(float max, float min)
    {
        return (max + min) / 2;
    }
}
