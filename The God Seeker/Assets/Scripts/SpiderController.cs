using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderController : MonoBehaviour
{
    float attackCD = 1;
    int attacked = 0;

    Quaternion currentPos, lastPos;

    NavMeshAgent agent;
    public Transform player;

    [SerializeField]
    ParticleSystem preFire, fire;
    [SerializeField]
    GameObject attackRange;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) >= 30)
            return;
        if (agent.velocity.magnitude <= 0.1f)
                Attack();
            else
            {
                attackCD = 2f;
                preFire.gameObject.SetActive(false);
                fire.gameObject.SetActive(false);
                attackRange.SetActive(false);
            }
            if (attackCD > 1.5f)
                Movement();
    }

    void Movement()
    {
        lastPos = transform.rotation;

        Vector3 playerPos = new Vector3(player.position.x, 1, player.position.z);
        agent.SetDestination(playerPos);

        transform.LookAt(playerPos);

        currentPos = transform.rotation;
    }

    void Attack()
    {
        attackCD -= Time.deltaTime;
        if(attackCD > 1.5f)
        {
            transform.LookAt((transform.position + player.position) / 3);
            preFire.gameObject.SetActive(true);
            return;
        }
        if (attackCD > 1.3f)
            return;
        if (attackCD > 1f)
        {
            if(attacked == 0)
            {
                attacked = 1; 
                attackRange.SetActive(true);
            }
            preFire.gameObject.SetActive(false);
            fire.gameObject.SetActive(true);
            ParticleSystem.MainModule fireSetting = fire.main;
            fireSetting.startSize = 1f * (attackCD - 1) / 0.3f;
            return;
        }
        fire.gameObject.SetActive(false);
        attackRange.gameObject.SetActive(false);
        transform.LookAt((transform.position + player.position) / 3);

        if (attackCD <= 0)
        {
            attacked = 0;
            attackCD = 2;
        }
    }
}
