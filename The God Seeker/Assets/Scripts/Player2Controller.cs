using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player2Controller : MonoBehaviour
{
    public GameObject player1, popUp;
    int speed = 20;

    NavMeshAgent agent;

    [SerializeField] float gravity = 0;
    Vector3 playerMovement;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                agent.SetDestination(hit.point);
            }
        }

        if(transform.position.x >= player1.transform.position.x + 18.5f ||
            transform.position.x <= player1.transform.position.x - 18.5f || 
            transform.position.z >= player1.transform.position.z + 14f ||
            transform.position.z <= player1.transform.position.z - 14f)
        {
            transform.position = player1.transform.position;
            popUp.transform.position = transform.position;
            popUp.SetActive(true);
            popUp.GetComponent<ParticleSystem>().Play();
        }
    }
}