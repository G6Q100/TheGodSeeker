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
    [SerializeField] GameObject energyBall, centerPoint;

    private void Start()
    {
        DefaultRrightArmPos = rightArm.transform.localPosition;
        DefaultLeftArmPos = leftArm.transform.localPosition;
        rightArmPos = DefaultRrightArmPos;
        leftArmPos = DefaultLeftArmPos;

        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];

        changeTime = -6;

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
            case 3:
                ClawAttack();
                break;
            case 4:
                ComboAttack();
                break;
            case 5:
                ThunderBlastAttack();
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
            if (mode < 5)
                mode++;
            else
                mode = 1;
            lastMode = mode;

            if (mode == 2)
                savePos = transform.position - transform.forward * 12;

            if (mode == 4)
                savePos = transform.position - transform.forward * 15;
        }
    }

    void Follow()
    {
        rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed * 3);
        leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos, 
            smoothSpeed * 3);

        if (Vector3.Distance(transform.position, player.position) > 11)
        {
            if (circling == 0)
            {
                circling = 1;
                CircleRadius = Vector3.Distance(transform.position, player.position);
                RotationSpeed = 1f;
            }
            CircleRadius -= Time.deltaTime * 10;
        }
        else if (Vector3.Distance(transform.position, player.position) > 8)
        {
            CircleRadius -= Time.deltaTime * 10;
        }
        CircleMove(player.position);
        transform.LookAt(player.position + Vector3.up * 3);
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
                transform.LookAt(player.position + Vector3.up);
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
            CircleMove(player.position);
            return;
        }

        if (attackSpace < 2.5f)
        {
            if(fire == 0 && attackSpace > 1.85f)
            {
                fire = 1;
                transform.LookAt(player.position + Vector3.up);
                Instantiate(energyBall, transform.position + transform.forward, transform.rotation);
            }
            return;
        }

        if (attackSpace < 3.1f)
        {
            fire = 0;
            circling = 1;
            CircleRadius = 15;
            RotationSpeed = 5;
            CircleMove(player.position);
            return;
        }

        if (attackSpace < 4f)
        {
            if(fire == 0 && attackSpace > 3.6f)
            {
                fire = 1;
                transform.LookAt(player.position + Vector3.up);
                Instantiate(energyBall, transform.position + transform.forward, transform.rotation);
            }
            return;
        }

        fire = 0;
        changeTime = Random.Range(-2f, 2f);
    }

    void ComboAttack()
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
                transform.LookAt(player.position + Vector3.up);
                Instantiate(energyBall, transform.position + transform.forward, transform.rotation);
            }
            return;
        }

        if (attackSpace < 1.35f)
        {
            fire = 0;
            circling = 1;
            CircleRadius = 18;
            RotationSpeed = 5;
            CircleMove(player.position);
            return;
        }

        if (attackSpace < 2f)
        {
            if (fire == 0 && attackSpace > 1.85f)
            {
                fire = 1;
                transform.LookAt(player.position + Vector3.up);
                Instantiate(energyBall, transform.position + transform.forward, transform.rotation);
            }
            return;
        }

        fire = 0;

        if (attackSpace < 2.3f)
        {
            transform.rotation = saveRot;
            savePos = player.position - transform.forward * 12 - transform.right * 12 + transform.up * 3;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 8);
            transform.LookAt(player.position + Vector3.up * 3);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);
            return;
        }

        if (attackSpace < 2.6f)
        {
            transform.rotation = saveRot;
            savePos = player.position - transform.forward * 10 + transform.right * 12 + transform.up * 3;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 8);
            transform.LookAt(player.position + Vector3.up * 3);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);
            return;
        }

        if (attackSpace < 2.9f)
        {
            playerPos = player.transform.position + transform.up * 1;
        }

        if (attackSpace < 3.2f)
        {
            transform.rotation = saveRot;
            savePos = player.position - transform.forward * 8 - transform.right * 12 + transform.up * 5;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 5);
            transform.LookAt(player.position + Vector3.up * 3);

            leftArmPos = new Vector3(0.05f, -leftArmPos.y, -leftArmPos.z);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
            smoothSpeed * 3);
            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
                smoothSpeed * 3);

            savePos = player.position - transform.forward * 6.5f + transform.right * 3 + transform.up * 3;
            return;
        }

        if (attackSpace < 4.4f)
        {
            transform.rotation = saveRot;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 5);
            transform.LookAt(player.position + Vector3.up * 3);
            leftArm.GetComponent<Rigidbody>().detectCollisions = true;

            leftArm.transform.position = Vector3.Lerp(leftArm.transform.position, playerPos,
                smoothSpeed * 10);
            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);
            return;
        }

        leftArmPos = DefaultLeftArmPos;
        leftArm.GetComponent<Rigidbody>().detectCollisions = false;
        changeTime = Random.Range(-2f, 2f);
    }

    void ThunderBlastAttack()
    {
        attackSpace += Time.deltaTime;

        if (attackSpace < 3f)
        {
            transform.LookAt(centerPoint.transform.position + Vector3.up * 3);
            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
             smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);

            if (circling == 0)
            {
                circling = 1;
                CircleRadius = 15;
                RotationSpeed = 5f;
                ElevationOffset = 5;
            }
            CircleMove(centerPoint.transform.position);
            return;
        }
        if (attackSpace < 9f)
        {
            transform.LookAt(centerPoint.transform.position + Vector3.up * 30);
            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
             smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);

            if (circling == 0)
            {
                circling = 1;
                CircleRadius = 65;
                RotationSpeed = 1f;
                ElevationOffset = 45;
            }
            CircleMove(centerPoint.transform.position + Vector3.up * 30);
            return;
        }

        changeTime = Random.Range(-2f, 2f);
    }

    void CircleMove(Vector3 target)
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
            CircleRadius = 6f + Mathf.Sin(idleTime * 1.2f) * 1f;
            ElevationOffset = 6f + Mathf.Sin(idleTime * 1.2f) * 2f;
            RotationSpeed = 1 + Mathf.Sin(idleTime * 1.2f) * 0.3f;
        }

        transform.position = Vector3.Lerp(transform.position, target + positionOffset, smoothSpeed * 3);
        angle += Time.deltaTime * RotationSpeed;
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
                (targetDist - i * 0.035f) - targetDir.forward * targetDist * 0.6f;

            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], smoothSpeed);
            body[i - 1].transform.position = segmentPoses[i];
        }

        lineRenderer.SetPositions(segmentPoses);
    }
}
