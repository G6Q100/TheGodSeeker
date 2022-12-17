using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class SpiderController : MonoBehaviour
{
    float attackCD = 1;

    public GameObject droneShape;

    NavMeshAgent agent;
    public Transform player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.velocity.magnitude <= 0.5f)
            Attack();
        else
            attackCD = 0.5f;
        Movement();
    }

    void Movement()
    {
        Vector3 playerPos = new Vector3(player.position.x, 1, player.position.z);
        agent.SetDestination(playerPos);

        transform.LookAt(playerPos);
    }

    void Attack()
    {
        attackCD += Time.deltaTime;
    }
}
