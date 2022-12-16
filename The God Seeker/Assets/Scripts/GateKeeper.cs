using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateKeeper : MonoBehaviour
{
    public int length;
    public LineRenderer lineRenderer;
    public Vector3[] segmentPoses;
    private Vector3[] segmentV;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;

    public Transform[] body;

    float time, sinTime;

    private void Start()
    {
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
        BodyCreate();
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time > 1)
            Movement();
        if (time < 0)
        {
            time = 6;
        }

        BodyFollow();
    }

    void Movement()
    {
        sinTime += Time.deltaTime * 1.5f;
        if (sinTime >= 360)
        {
            sinTime = 0;
        }
        transform.position += transform.forward * 0.5f + transform.up * Mathf.Sin(sinTime) * 0.25f;
        transform.Rotate(Vector3.up * 2);

    }

    void BodyCreate()
    {
        segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            Vector3 targetPos = segmentPoses[i - 1] + Vector3.back * targetDist;

            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], smoothSpeed);
            body[i - 1].transform.position = segmentPoses[i];
        }

        lineRenderer.SetPositions(segmentPoses);
    }

    void BodyFollow()
    {
        segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * targetDist;

            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], smoothSpeed);
            body[i - 1].transform.position = segmentPoses[i];
        }

        lineRenderer.SetPositions(segmentPoses);
    }
}
