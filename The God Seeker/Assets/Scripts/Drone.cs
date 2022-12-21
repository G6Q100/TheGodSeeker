using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drone : MonoBehaviour
{
    float bulletCD = 1;

    public GameObject droneShape, enemyBullet;

    Quaternion currentPos, lastPos;

    NavMeshAgent agent;
    public Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) >= 25)
            return;
        if (agent.velocity.magnitude <= 0.5f)
                Attack();
            else
                bulletCD = 0.5f;
            Movement();
    }

    void Movement()
    {
        lastPos = transform.rotation;

        Vector3 playerPos = new Vector3(player.position.x, 1, player.position.z);
        agent.SetDestination(playerPos);

        transform.LookAt(playerPos);

        currentPos = transform.rotation;

        droneShape.transform.rotation = Quaternion.Euler(Mathf.Clamp(lastPos.z - currentPos.z, -1, 1) * -30, 
            0, Mathf.Clamp(lastPos.x - currentPos.x, -1, 1) * 30) * transform.rotation;
    }

    void Attack()
    {
        bulletCD += Time.deltaTime;

        if (bulletCD >= 1.5f)
        {
            Instantiate(enemyBullet, transform.position + Vector3.down, transform.rotation);
            bulletCD = 0;
        }
    }

}
