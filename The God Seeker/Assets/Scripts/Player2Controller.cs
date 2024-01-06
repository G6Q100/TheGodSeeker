using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player2Controller : MonoBehaviour
{
    public GameObject player1, popUp, popUp2, healing;

    NavMeshAgent agent;

    float teleportCD, dashCD;

    public int hitTime;

    public Slider teleportSlider, hitTimeSlider;

    Vector3 lastStand;

    [SerializeField] Transform attackRange;
    [SerializeField] GameObject dashArea;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Movement();
        Dash();
        Teleport();
    }

    void Movement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            agent.enabled = true;
            lastStand = transform.position;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity) && agent.enabled)
            {
                if (hit.point.z <= -16)
                    agent.SetDestination(new Vector3(hit.point.x, hit.point.y + 2, -16));
                else
                    agent.SetDestination(new Vector3(hit.point.x, hit.point.y + 2, hit.point.z));
            }
        }

        if (transform.position.x >= player1.transform.position.x + 32.5f ||
            transform.position.x <= player1.transform.position.x - 32.5f ||
            transform.position.z >= player1.transform.position.z + 32.5f ||
            transform.position.z <= player1.transform.position.z - 18.5f)
        {
            transform.position = player1.transform.position + Vector3.up;
            popUp.transform.position = transform.position + Vector3.up;
            popUp.SetActive(true);
            agent.SetDestination(transform.position);
            popUp.GetComponent<ParticleSystem>().Play();
        }
    }

    void Teleport()
    {
        if (teleportCD < 2)
            teleportCD += Time.deltaTime;

        teleportSlider.value = Mathf.Clamp(teleportCD, 0, 2);

        if (Input.GetKeyDown(KeyCode.Keypad1) && teleportCD >= 2)
        {
            lastStand = transform.position;

            Vector3 playerPos = player1.transform.position;

            Vector3 teleportedPos = transform.position;

            player1.GetComponent<PlayerController>().Teleported(teleportedPos + Vector3.up);
            transform.position = playerPos;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, Mathf.Infinity))
            {
                agent.SetDestination(transform.position - new Vector3(0, hit.distance + 2, 0));
            }

            popUp.transform.position = transform.position;
            popUp.SetActive(true);
            popUp.GetComponent<ParticleSystem>().Play();

            popUp2.transform.position = playerPos;
            popUp2.SetActive(true);
            popUp2.GetComponent<ParticleSystem>().Play();

            teleportCD = 0;
        }
    }

    void Dash()
    {
        if (Input.GetMouseButtonDown(1) && dashCD <= 0)
        {
            dashCD = 0.3f;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                agent.enabled = false;
                transform.LookAt(hit.point);
                transform.position = transform.position + transform.forward * 6;
                Destroy(Instantiate(dashArea, attackRange.transform.position, attackRange.transform.rotation), 0.3f);

                agent.enabled = true;
                agent.SetDestination(transform.position);
            }
        }

        if(dashCD > 0)
        {
            agent.enabled = true;
            dashCD -= Time.deltaTime;
        }
    }

    public void SpwanHeal()
    {
        Instantiate(healing, transform.position + transform.up, Quaternion.Euler(90, 0 , 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Environment")
        {
            gameObject.transform.position = lastStand;
            agent.SetDestination(transform.position);
        }
    }
}