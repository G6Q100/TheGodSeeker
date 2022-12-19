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

    [SerializeField] Transform player;

    [SerializeField] float RotationSpeed = 1;

    [SerializeField] float CircleRadius = 1;

    [SerializeField] float ElevationOffset = 0;

    private Vector3 positionOffset;
    private float angle;

    int circling = 0;
    [SerializeField]
    float idleTime, changeTime = -3f;

    int mode = 0, lastMode = 0;
    float attackSpace = 0;
    Vector3 savePos, DefaultRrightArmPos, DefaultLeftArmPos, rightArmPos, leftArmPos, playerPos;
    Quaternion saveRot;
    [SerializeField] GameObject rightArm, leftArm;

    int fire;
    [SerializeField] GameObject energyBall;

    private void Start()
    {
        DefaultRrightArmPos = rightArm.transform.localPosition;
        DefaultLeftArmPos = leftArm.transform.localPosition;
        rightArmPos = DefaultRrightArmPos;
        leftArmPos = DefaultLeftArmPos;

        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];

        changeTime = Random.Range(-2f, 2f);

        BodyCreate();
    }

    void LateUpdate()
    {
        BodyFollow();
        ChangeMode();

        changeTime += Time.deltaTime;
        switch (mode)
        {
            case 0:
                Follow();
                break;
            case 1:
                ClawAttack();
                break;
            case 2:
                FireAttack();
                break;
        }
    }

    void ChangeMode()
    {
        if (changeTime <= 9f)
        {
            mode = 0;
            changeTime += Time.deltaTime;
            Follow();
            return;
        }
        if (mode == 0)
        {
            mode = lastMode;
            attackSpace = 0;
            if (mode < 2)
                mode++;
            else
                mode = 1;
            lastMode = mode;

            if (mode == 2)
                savePos = transform.position - transform.forward * 12;
        }
    }

    void Follow()
    {
        rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed * 3);
        leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos, 
            smoothSpeed * 3);

        if (Vector3.Distance(transform.position, player.position) > 12)
        {
            if (circling == 0)
            {
                circling = 1;
                CircleRadius = Vector3.Distance(transform.position, player.position);
                RotationSpeed = 1f;
            }
            CircleRadius -= Time.deltaTime / 2;
        }
        else if (Vector3.Distance(transform.position, player.position) > 9)
        {
            CircleRadius -= Time.deltaTime / 2;
        }
        CircleMove();
        if (circling == 1)
        {
            circling = 0;
            idleTime = 0;
            RotationSpeed = 1f;
        }
    }

    void ClawAttack()
    {
        attackSpace += Time.deltaTime;

        transform.LookAt(player.position + Vector3.up * 3);
        if (attackSpace < 0.6f)
        {
            savePos = transform.position - transform.forward * 16 + transform.up;
            saveRot = transform.rotation;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);
            return;
        }
        if (attackSpace < 0.9f)
        {
            transform.rotation = saveRot;
            savePos = player.position - transform.forward * 12 + transform.right * 12 + transform.up * 3;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 8);
            transform.LookAt(player.position + Vector3.up * 3);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);
            return;
        }

        if (attackSpace < 1.2f)
        {
            transform.rotation = saveRot;
            savePos = player.position - transform.forward * 10 - transform.right * 12 + transform.up * 3;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 8);
            transform.LookAt(player.position + Vector3.up * 3);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);
            return;
        }

        if(attackSpace < 1.5f)
        {
            playerPos = player.transform.position + transform.up * 1;
        }

        if (attackSpace < 1.8f)
        {
            transform.rotation = saveRot;
            savePos = player.position - transform.forward * 8 + transform.right * 12 + transform.up * 5;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 5);
            transform.LookAt(player.position + Vector3.up * 3);

            rightArmPos = new Vector3(0.05f, -rightArmPos.y, -rightArmPos.z);
            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);

            savePos = player.position - transform.forward * 6.5f - transform.right * 3 + transform.up * 3;
            return;
        }

        if (attackSpace < 3f)
        {
            transform.rotation = saveRot;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 5);
            transform.LookAt(player.position + Vector3.up * 3);
            rightArm.GetComponent<Rigidbody>().detectCollisions = true;

            rightArm.transform.position = Vector3.Lerp(rightArm.transform.position, playerPos,
                smoothSpeed * 10);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);
            return;
        }

        rightArmPos = DefaultRrightArmPos;
        rightArm.GetComponent<Rigidbody>().detectCollisions = false;
        changeTime = Random.Range(-2f,2f);
    }


    void FireAttack()
    {
        attackSpace += Time.deltaTime;

        transform.LookAt(player.position + Vector3.up * 3);
        if (attackSpace < 0.6f)
        {
            fire = 0;

            saveRot = transform.rotation;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);
            return;
        }

        if (attackSpace < 0.8f)
        {
            if (fire == 0)
            {
                fire = 1;
                Instantiate(energyBall, transform.position + transform.forward, transform.rotation);
            }
            return;
        }
        if (attackSpace < 1.35f)
        {
            fire = 0;
            circling = 1;
            CircleRadius = 15;
            RotationSpeed = 5;
            CircleMove();
            return;
        }

        if (attackSpace < 2.5f)
        {
            if(fire == 0 && attackSpace > 1.85f)
            {
                fire = 1;
                Instantiate(energyBall, transform.position + transform.forward, transform.rotation);
                Debug.Log("Fire");
            }
            return;
        }

        if (attackSpace < 3.1f)
        {
            fire = 0;
            circling = 1;
            CircleRadius = 15;
            RotationSpeed = 5;
            CircleMove();
            return;
        }

        if (attackSpace < 4f)
        {
            if(fire == 0 && attackSpace > 3.6f)
            {
                fire = 1;
                Instantiate(energyBall, transform.position + transform.forward, transform.rotation);
            }
            return;
        }

        fire = 0;
        changeTime = Random.Range(-2f, 2f);
    }


    void CircleMove()
    {
        positionOffset.Set(
         Mathf.Cos(angle) * CircleRadius,
         ElevationOffset,
         Mathf.Sin(angle) * CircleRadius
     );
        if(circling == 0)
        {
            idleTime += Time.deltaTime;
            if(idleTime >= 180)
            {
                idleTime = 0;
            }
            CircleRadius = 8f + Mathf.Sin(idleTime / 2) * 1f;
            ElevationOffset = 5.5f + Mathf.Sin(idleTime / 2) * 1.5f;
            RotationSpeed = 1 + Mathf.Sin(idleTime / 2) * 0.2f;
        }

        transform.position = Vector3.Lerp(transform.position, player.position + positionOffset, smoothSpeed * 3);
        angle += Time.deltaTime * RotationSpeed;
        transform.LookAt(player.position + Vector3.up * 3);
    }

    void BodyCreate()
    {
        segmentPoses[0] = targetDir.position;

        float size = body[0].localScale.x;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            Vector3 targetPos = segmentPoses[i - 1] - Vector3.back * (targetDist - i * 0.1f);
            body[i - 1].localScale = Vector3.one * size;
            size -= 5;
            segmentPoses[i] = targetPos;
            body[i - 1].transform.position = segmentPoses[i];
        }

        lineRenderer.SetPositions(segmentPoses);
    }

    void BodyFollow()
    {
        segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * 
                (targetDist - i * 0.05f) - targetDir.forward * targetDist * 0.6f;

            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], smoothSpeed);
            body[i - 1].transform.position = segmentPoses[i];
        }

        lineRenderer.SetPositions(segmentPoses);
    }
}
