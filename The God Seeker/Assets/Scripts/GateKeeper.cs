using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateKeeper : MonoBehaviour
{
    public int length;
    public LineRenderer lineRenderer;
    public Vector3[] segmentPoses;
    private Vector3[] segmentV;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;

    public Transform[] body, lightningEffect;

    [SerializeField] Transform player;

    [SerializeField] float RotationSpeed = 1;

    [SerializeField] float CircleRadius = 1;

    [SerializeField] float ElevationOffset = 0;

    private Vector3 positionOffset;
    private float angle;

    int circling = 0;
    [SerializeField]
    float idleTime, changeTime = -3f;

    int mode = 0, lastMode = 0, phase = 1;
    float attackSpace = 0, tempSpace = 0;
    Vector3 savePos, DefaultRrightArmPos, DefaultLeftArmPos, rightArmPos, leftArmPos, playerPos;
    Quaternion saveRot;

    [SerializeField] GameObject rightArm, leftArm, attackHead;
    [SerializeField] GameObject warning;

    int fire;
    [SerializeField] GameObject energyBall, centerPoint, thunderBall, thunderShock, lightning,
        lightningDrop, music;
    [SerializeField] AudioClip bg2;
    [SerializeField] Animator dirLight, hpBar;
    [SerializeField] Material thunderDragon;

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
            case -1:
                Phase2Change();
                break;
            case 0:
                Follow();
                break;
            case 1:
                if (phase == 3)
                    ThunderClawAttack();
                else
                    ClawAttack();
                break;
            case 2:
                if (phase == 3)
                    RipidFireAttack();
                else
                    FireAttack();
                break;
            case 3:
                if (phase == 1)
                    ClawAttack();
                else
                    DashAttack();
                break;
            case 4:
                if (phase == 3)
                    ThunderClawAttack();
                else
                    ComboAttack();
                break;
            case 5:
                DashAttack();
                break;
            case 6:
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
            if(phase == 1)
            {
                if (mode < 4)
                    mode++;
                else
                    mode = 1;
                lastMode = mode;
            }
            else if (phase == 2)
            {
                if (mode < 5)
                    mode++;
                else
                    mode = 1;
                lastMode = mode;
            }
            else
            {
                if (mode < 6)
                    mode++;
                else
                    mode = 1;
                lastMode = mode;
            }

            if(GetComponent<HP>().healthPoint <= 150 && phase == 1)
            {
                phase = 2;
                mode = 5;
                lastMode = mode;
            }
            if (GetComponent<HP>().healthPoint <= 100 && phase == 2)
            {
                phase = 3;
                lastMode = 5;
                mode = -1;

                GetComponent<BoxCollider>().enabled = false;
                attackHead.GetComponent<BoxCollider>().enabled = false;
                for (int i = 0; i < body.Length - 1; i++)
                {
                    body[i].GetComponent<BoxCollider>().enabled = false;
                }

                hpBar.SetTrigger("FadeOut");
                CameraController.instance.player = centerPoint;
            }

            if (mode == 2 || mode == 5 || (mode == 3 && phase != 1))
            {
                savePos = transform.position - transform.forward * 12;
                return;
            }

            if ((mode == 4 && phase != 3) || (mode == 1 && phase == 3))
            {
                savePos = transform.position - transform.forward * 15;
                return;
            }

            if(mode == 6)
            {
                GetComponent<BoxCollider>().enabled = false;
                attackHead.GetComponent<BoxCollider>().enabled = false;
                for (int i = 0; i < body.Length - 1; i++)
                {
                    body[i].GetComponent<BoxCollider>().enabled = false;
                }
                return;
            }
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
                Vector3.Distance(transform.position, player.position);
            }
        }
        if (CircleRadius <= 8f)
        {
            circling = 0;
        }
        else
        {
            CircleRadius -= Time.deltaTime * 5;
            ElevationOffset = 6f + Mathf.Sin(idleTime * 1.2f) * 2f;
            RotationSpeed = 1 + Mathf.Sin(idleTime * 1.2f) * 0.3f;
        }

        CircleMove(player.position);
        transform.LookAt(player.position + Vector3.up * 3);
    }

    void ClawAttack()
    {
        attackSpace += Time.deltaTime;

        transform.LookAt(player.position + Vector3.up * 3);
        if (attackSpace < 0.6f)
        {
            savePos = transform.position - transform.forward * 2 + transform.up;
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
            savePos.y = 6;
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
            savePos.y = 6;
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
            savePos.y = 9;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 5);
            transform.LookAt(player.position + Vector3.up * 3);

            rightArmPos = new Vector3(0.05f, -rightArmPos.y, -rightArmPos.z);
            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);

            savePos = player.position - transform.forward * 6.5f - transform.right * 3 + transform.up * 3;
            savePos.y = 6;
            fire = 0;
            return;
        }

        if (attackSpace < 3f)
        {
            if(fire == 0)
            {
                fire = 1;
                CameraController.instance.shake = 0.5f;
            }

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

        fire = 0;
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
                transform.LookAt(player.position + Vector3.up * 1.5f);
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
                transform.LookAt(player.position + Vector3.up * 1.5f);
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
            if(fire == 0 && attackSpace > 3.5f)
            {
                fire = 1;
                transform.LookAt(player.position + Vector3.up * 1.5f);
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
        if (attackSpace < 2.3f)
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

        if (attackSpace < 2.6f)
        {
            transform.rotation = saveRot;
            savePos = player.position - transform.forward * 12 - transform.right * 12 + transform.up * 3;
            savePos.y = 6;
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
            transform.rotation = saveRot;
            savePos = player.position - transform.forward * 10 + transform.right * 12 + transform.up * 3;
            savePos.y = 6;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 8);
            transform.LookAt(player.position + Vector3.up * 3);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);
            return;
        }

        if (attackSpace < 3.2f)
        {
            playerPos = player.transform.position + transform.up * 1;
        }

        if (attackSpace < 3.5f)
        {
            transform.rotation = saveRot;
            savePos = player.position - transform.forward * 8 - transform.right * 12 + transform.up * 5;
            savePos.y = 8;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 5);
            transform.LookAt(player.position + Vector3.up * 3);

            leftArmPos = new Vector3(-0.05f, -leftArmPos.y, -leftArmPos.z);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
            smoothSpeed * 3);
            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
                smoothSpeed * 3);

            savePos = player.position - transform.forward * 6.5f + transform.right * 3 + transform.up * 3;
            savePos.y = 6;
            fire = 0;
            return;
        }

        if (attackSpace < 4.8f)
        {
            if (fire == 0)
            {
                fire = 1;
                CameraController.instance.shake = 0.5f;
            }

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

        fire = 0;
        leftArmPos = DefaultLeftArmPos;
        leftArm.GetComponent<Rigidbody>().detectCollisions = false;
        changeTime = Random.Range(-2f, 2f);
    }

    void DashAttack()
    {
        attackSpace += Time.deltaTime;

        if (attackSpace < 0.5f)
        {
            transform.LookAt(player.position + Vector3.up * 3);
            saveRot = transform.rotation;

            transform.position = Vector3.Lerp(transform.position, new Vector3(savePos.x, 3.5f, savePos.z), smoothSpeed / 3);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);

            if (attackSpace < 0.1f)
                warning.SetActive(false);
            else
                warning.SetActive(true);
            return;
        }

        if (attackSpace < 1f)
        {
            if (fire == 0)
            {
                fire = 1;
                savePos = transform.position + transform.forward * (Vector3.Distance(transform.position, player.position) * 3 + 2);
                saveRot = transform.rotation;

                warning.SetActive(false); 
                GetComponent<BoxCollider>().enabled = false;
                attackHead.GetComponent<BoxCollider>().enabled = true;
                for (int i = 0; i < body.Length - 1; i++)
                {
                    body[i].GetComponent<BoxCollider>().enabled = false;
                }
            }
            return;
        }
        if (attackSpace < 1.5f)
        {

            transform.position = Vector3.Lerp(transform.position, new Vector3(savePos.x, transform.position.y, savePos.z), smoothSpeed / 2);
            transform.LookAt(player.position + Vector3.up * 3);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed);

            if (attackSpace < 1.1f)
                warning.SetActive(false);
            else
                warning.SetActive(true);
            fire = 0;
            return;
        }

        if (attackSpace < 2f)
        {
            if(fire == 0)
            {
                fire = 1;
                savePos = transform.position + transform.forward * (Vector3.Distance(transform.position, player.position) * 3 + 2);
                saveRot = transform.rotation;

                warning.SetActive(false);
            }
            return;
        }

        if (attackSpace < 2.5f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(savePos.x, transform.position.y, savePos.z), smoothSpeed / 2);
            transform.LookAt(player.position + Vector3.up * 3);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed);

            if (attackSpace < 2.1f)
                warning.SetActive(false);
            else
                warning.SetActive(true);
            fire = 0;
            return;
        }

        if (attackSpace < 3f)
        {
            if (fire == 0)
            {
                fire = 1;
                savePos = transform.position + transform.forward * (Vector3.Distance(transform.position, player.position) * 3 + 2);
                saveRot = transform.rotation;

                warning.SetActive(false);
            }
            return;
        }

        if (attackSpace < 3.5f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(savePos.x, transform.position.y, savePos.z), smoothSpeed / 2);
            transform.LookAt(player.position + Vector3.up * 3);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed);
            return;
        }
        transform.position += Vector3.up * 3;

        fire = 0;
        GetComponent<BoxCollider>().enabled = true;
        attackHead.GetComponent<BoxCollider>().enabled = false;
        for (int i = 0; i < body.Length - 1; i++)
        {
            body[i].GetComponent<BoxCollider>().enabled = true;
        }
        changeTime = Random.Range(-2f, 2f);
    }

    void Phase2Change()
    {
        attackSpace += Time.deltaTime;

        if (attackSpace < 2f)
        {
            music.GetComponent<AudioSource>().volume = music.GetComponent<AudioSource>().volume - attackSpace /2;
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

        if (attackSpace < 5f)
        {
            music.GetComponent<AudioSource>().volume = 0;
            if (attackSpace > 4 && fire == 0)
            {
                fire = 1;
                dirLight.SetBool("Thunder", true);
                CameraController.instance.player = player.gameObject;
            }

            transform.LookAt(centerPoint.transform.position + Vector3.up * 70);
            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
             smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);

            if (circling == 0)
            {
                circling = 1;
                CircleRadius = 80;
                RotationSpeed = 1f;
                ElevationOffset = 30;
            }
            CircleMove(centerPoint.transform.position + Vector3.up * 60);
            return;
        }

        fire = 0;

        music.GetComponent<AudioSource>().Stop();
        music.GetComponent<AudioSource>().clip = bg2;
        music.GetComponent<AudioSource>().volume = 1;
        music.GetComponent<AudioSource>().Play();
        hpBar.SetTrigger("FadeIn");
        lightningDrop.SetActive(true);
        CameraController.instance.player = player.gameObject;
        foreach (Transform effect in lightningEffect)
        {
            effect.gameObject.SetActive(true);
        }

        GetComponent<BoxCollider>().enabled = true;
        attackHead.GetComponent<BoxCollider>().enabled = false;
        for (int i = 0; i < body.Length - 1; i++)
        {
            body[i].GetComponent<BoxCollider>().enabled = true;
        }

        GetComponent<HP>().normalM = thunderDragon;
        changeTime = Random.Range(-2f, 2f);
    }

    void ThunderClawAttack()
    {
        attackSpace += Time.deltaTime;

        transform.LookAt(player.position + Vector3.up * 3);
        if (attackSpace < 0.6f)
        {
            savePos = transform.position - transform.forward * 3 + transform.up;
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
            savePos.y = 6;
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
            savePos.y = 6;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 8);
            transform.LookAt(player.position + Vector3.up * 3);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);
            return;
        }

        if (attackSpace < 1.5f)
        {
            playerPos = player.transform.position + transform.up * 1;
        }

        if (attackSpace < 1.8f)
        {
            transform.rotation = saveRot;
            savePos = player.position - transform.forward * 8 + transform.right * 12 + transform.up * 5;
            savePos.y = 9;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 5);
            transform.LookAt(player.position + Vector3.up * 3);

            rightArmPos = new Vector3(0.05f, -rightArmPos.y, -rightArmPos.z);
            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);

            savePos = player.position - transform.forward * 6.5f - transform.right * 3 + transform.up * 3;
            savePos.y = 6;
            fire = 0;
            return;
        }

        if (attackSpace < 2.5f)
        {
            if (fire == 0)
            {
                fire = 1;
                CameraController.instance.shake = 0.5f;
            }

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
        if (attackSpace < 2.8f)
        {
            fire = 0;
            leftArmPos = DefaultLeftArmPos;
            rightArmPos = DefaultRrightArmPos;
            rightArm.GetComponent<Rigidbody>().detectCollisions = false;
            transform.LookAt(player.position + Vector3.up * 3);
            return;
        }

        if (attackSpace <= 3.3f)
        {
            leftArmPos = new Vector3(-0.05f, -leftArmPos.y, -leftArmPos.z);
            rightArmPos = new Vector3(0.05f, -rightArmPos.y, -rightArmPos.z);
            transform.rotation = saveRot;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 5);
            rightArm.transform.position = Vector3.Lerp(rightArm.transform.position, playerPos,
                smoothSpeed * 10);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);
            return;
        }

        if (attackSpace < 3.9f)
        {
            if (fire == 0)
            {
                fire = 1;
                Vector3 spawnPoint = new Vector3(transform.position.x, 0.1f, transform.position.z);
                Instantiate(lightning, spawnPoint, Quaternion.Euler(0, 0, 0));
                Instantiate(lightning, spawnPoint, Quaternion.Euler(0, 45, 0));
                Instantiate(lightning, spawnPoint, Quaternion.Euler(0, 90, 0));
                Instantiate(lightning, spawnPoint, Quaternion.Euler(0, 135, 0));
                Instantiate(lightning, spawnPoint, Quaternion.Euler(0, 180, 0));
                Instantiate(lightning, spawnPoint, Quaternion.Euler(0, 225, 0));
                Instantiate(lightning, spawnPoint, Quaternion.Euler(0, 270, 0));
                Instantiate(lightning, spawnPoint, Quaternion.Euler(0, 315, 0));
                return;
            }

            transform.rotation = saveRot;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 5);
            rightArm.transform.position = Vector3.Lerp(rightArm.transform.position, playerPos,
                smoothSpeed * 10);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);
            return;
        }
        if (attackSpace < 5f)
        {
            transform.rotation = saveRot;
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 5);

            leftArmPos = new Vector3(-0.05f, -leftArmPos.y, -leftArmPos.z);
            rightArmPos = new Vector3(0.05f, -rightArmPos.y, -rightArmPos.z);
            rightArm.transform.position = Vector3.Lerp(rightArm.transform.position, playerPos,
                smoothSpeed * 10);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);

            return;
        }

        leftArmPos = DefaultLeftArmPos;
        rightArmPos = DefaultRrightArmPos;
        fire = 0;
        changeTime = Random.Range(-2f, 2f);
    }

    void RipidFireAttack()
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

        if (attackSpace < 0.7f)
        {
            if (fire == 0)
            {
                fire = 1;
                transform.LookAt(player.position + Vector3.up * 1.5f);
                Instantiate(energyBall, transform.position + transform.forward, transform.rotation);
            }

            tempSpace = 0;
            return;
        }
        if(attackSpace < 3.5f)
        {
            if (attackSpace < 0.9f + tempSpace)
            {
                fire = 0;
                circling = 1;
                CircleRadius = 15;
                RotationSpeed = 5;
                CircleMove(player.position);
                return;
            }

            if (attackSpace < 1.1f + tempSpace)
            {
                if (fire == 0 && attackSpace > 1f + tempSpace)
                {
                    fire = 1;
                    transform.LookAt(player.position + Vector3.up * 1.5f);
                    Instantiate(energyBall, transform.position + transform.forward, transform.rotation);
                }
                return;
            }
            tempSpace += 0.5f;
            return;
        }

        if (attackSpace < 4f)
        {
            tempSpace = 0;
            if (fire == 0 && attackSpace > 3.5f)
            {
                fire = 1;
                transform.LookAt(player.position + Vector3.up * 1.5f);
                Instantiate(energyBall, transform.position + transform.forward, transform.rotation);
            }
            return;
        }

        fire = 0;
        changeTime = Random.Range(-2f, 2f);
    }

    void ThunderBlastAttack()
    {
        attackSpace += Time.deltaTime;

        if (attackSpace < 3f)
        {
            if (attackSpace > 1 && fire == 0)
            {
                fire = 1;
                Instantiate(thunderBall, centerPoint.transform.position + Vector3.up * 2, Quaternion.identity);
            }

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

        if (attackSpace < 13f)
        {
            transform.LookAt(centerPoint.transform.position + Vector3.up * 70);
            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
             smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);

            if (circling == 0)
            {
                circling = 1;
                CircleRadius = 80;
                RotationSpeed = 1f;
                ElevationOffset = 65;
            }
            CircleMove(centerPoint.transform.position + Vector3.up * 70);
            return;
        }

        if (attackSpace < 13.5f)
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

            savePos = centerPoint.transform.position + Vector3.up * 2;

            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 10);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
             smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);

            return;
        }

        if (attackSpace < 15f)
        {
            if (attackSpace < 14.5f)
            {
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                warning.SetActive(true);
                fire = 0;
            }
            else
            {
                if (fire == 0)
                {
                    fire = 1;
                    savePos = transform.position - transform.forward * 5 + transform.up;
                }
                warning.SetActive(false);
            }

            saveRot = transform.rotation;

            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 10);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
            smoothSpeed);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed);
            return;
        }
        if (attackSpace < 18f)
        {
            if (fire == 0)
            {
                fire = 1;
                savePos = transform.position + transform.forward * 5 + transform.up;
            }

            thunderShock.SetActive(true);
            transform.position = Vector3.Lerp(transform.position, savePos, smoothSpeed * 10);

            var targetRotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1.2f);

            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, rightArmPos,
             smoothSpeed * 3);
            leftArm.transform.localPosition = Vector3.Lerp(leftArm.transform.localPosition, leftArmPos,
                smoothSpeed * 3);

            CameraController.instance.shake = 0.5f;
            return;
        }
        transform.position += Vector3.up * 3;

        GetComponent<BoxCollider>().enabled = true;
        attackHead.GetComponent<BoxCollider>().enabled = false;
        for (int i = 0; i < body.Length - 1; i++)
        {
            body[i].GetComponent<BoxCollider>().enabled = true;
        }

        thunderShock.SetActive(false);
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
            CircleRadius = 5.5f + Mathf.Sin(idleTime * 1.2f) * 2f;
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
