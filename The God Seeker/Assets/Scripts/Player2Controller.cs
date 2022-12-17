using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player2Controller : MonoBehaviour
{
    public GameObject player1, popUp, popUp2;

    NavMeshAgent agent;

    float teleportCD;

    public Slider teleportSlider;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        Movement();
        Teleport();
    }

    void Movement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.point.z <= -16)
                    agent.SetDestination(new Vector3(hit.point.x, hit.point.y, -16));
                else
                    agent.SetDestination(hit.point);
            }
        }

        if (transform.position.x >= player1.transform.position.x + 26.5f ||
            transform.position.x <= player1.transform.position.x - 26.5f ||
            transform.position.z >= player1.transform.position.z + 22.5f ||
            transform.position.z <= player1.transform.position.z - 18.5f)
        {
            transform.position = player1.transform.position;
            popUp.transform.position = transform.position;
            popUp.SetActive(true);
            agent.SetDestination(transform.position);
            popUp.GetComponent<ParticleSystem>().Play();
        }
    }

    void Teleport()
    {
        if (teleportCD < 3)
            teleportCD += Time.deltaTime;

        teleportSlider.value = Mathf.Clamp(teleportCD, 0, 3);

        if (Input.GetKeyDown(KeyCode.Keypad1) && teleportCD >= 3)
        {
            Vector3 playerPos = player1.transform.position;

            Vector3 teleportedPos = transform.position;

            player1.GetComponent<PlayerController>().Teleported(teleportedPos);
            transform.position = playerPos;

            popUp.transform.position = transform.position;
            popUp.SetActive(true);
            agent.SetDestination(transform.position);
            popUp.GetComponent<ParticleSystem>().Play();

            popUp2.transform.position = playerPos;
            popUp2.SetActive(true);
            popUp2.GetComponent<ParticleSystem>().Play();

            teleportCD = 0;
        }
    }
}