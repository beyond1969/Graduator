using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSplineTest : MonoBehaviour {

    public List<Vector3> curvePoints = new List<Vector3>();

    public float len = 10;
    private float t;
    private int segment;
    private float segmentInterval;

    // Use this for initialization
    void Start () {
        curvePoints.Add(new Vector3(0f, 0f, -2f));
        curvePoints.Add(new Vector3(0f, 0f, -2f));
        curvePoints.Add(new Vector3(0f, 0f, -2f));

        curvePoints.Add(new Vector3(0f, 2.9f, -1.3f));
        curvePoints.Add(new Vector3(0f, 1.0f, 0.9f));

        curvePoints.Add(new Vector3(0f, 0f, 2f));
        curvePoints.Add(new Vector3(0f, 0f, 2f));
        curvePoints.Add(new Vector3(0f, 0f, 2f));
        

        t = 0f;
        segment = 0;
        segmentInterval = len / (float)curvePoints.Count;
    }
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;
        if (t >= segmentInterval)
        {
            t = 0f;
            segment++;
        }
        if(segment >= curvePoints.Count - 3)
        {
            segment = 0;
        }
        BSplineMove();
	}

    private void BSplineMove()
    {
        float delta = t / segmentInterval;
        float t_sqr = Mathf.Pow(delta, 2);
        float t_cub = Mathf.Pow(delta, 3);

        float B0 = (1f / 6f) * Mathf.Pow(1f - delta, 3f);
        float B1 = (1f / 6f) * (3 * t_cub - 6 * t_sqr + 4);
        float B2 = (1f / 6f) * (-3 * t_cub + 3 * t_sqr + 3 * delta + 1);
        float B3 = (1f / 6f) * t_cub;

        transform.position = (B0 * curvePoints[segment]
                             +B1 * curvePoints[segment+1]
                             +B2 * curvePoints[segment+2]
                             +B3 * curvePoints[segment+3]);
    }
}
