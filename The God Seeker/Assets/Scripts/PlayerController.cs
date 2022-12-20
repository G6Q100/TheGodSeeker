using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    Animator anim;
    int speed = 10, maxGravity = -5;

    [SerializeField] float gravity = 0, attacking = 0;
    [SerializeField]int jumpTime = 1;

    string mode = "normal";

    Vector3 playerMovement, direction, lastStand;

    [SerializeField] ParticleSystem slowFalling;

    [SerializeField] Transform attackRange;
    [SerializeField] GameObject sword;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        switch (mode)
        {
            case "normal":
                Movement();
                break;
            case "attack":
                Attack();
                break;
        }
        if(transform.position.y <= -5)
        {
            transform.position = lastStand;
            GetComponent<HP>().Damaged(2, 0);
        }
    }

    void Attack()
    {
        attacking -= Time.deltaTime;
        if (attacking > 0.3f)
        {
            maxGravity = -10;
            playerMovement = transform.forward * 10 * Time.deltaTime;
            characterController.Move(playerMovement);
            return;
        }

        if (attacking > 0.1f)
        {
            maxGravity = -5;
            return;
        }

        if (attacking <= 0)
        {
            attacking = 0;
            mode = "normal";
        }
    }

    void Movement()
    {
        gravity -= 20f * Time.deltaTime;

        float v = Input.GetAxis("Horizontal") * speed;
        float h = Input.GetAxis("Vertical") * speed;

        playerMovement = new Vector3(v, gravity, h) * Time.deltaTime;

        characterController.Move(playerMovement);

        direction = characterController.velocity;

        if (direction.magnitude > .5f)
        {
            direction.y = 0;
            anim.SetBool("Running", true);
            transform.forward = direction;
        }
        else
        {
            anim.SetBool("Running", false);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, -16, 160));

        // Jump
        if (characterController.isGrounded)
        {
            gravity = 0;
            jumpTime = 1;
            speed = 10;

            anim.SetBool("Jump", false);
            var emission = slowFalling.emission;
            emission.rateOverTime = 0;
        }
        else
        {
            if(jumpTime > 0)
            {
                lastStand = transform.position;
            }
            jumpTime = 0;
            speed = 5;

            anim.SetBool("Jump", true);
            var emission = slowFalling.emission;
            emission.rateOverTime = 5;
        }

        if (Input.GetKeyDown(KeyCode.J) && jumpTime > 0)
        {
            attacking = 0.4f;
            anim.SetTrigger("Attack");
            Destroy(Instantiate(sword, attackRange.transform.position, attackRange.transform.rotation), 0.3f);
            mode = "attack";
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) &&
            jumpTime == 1 && attacking <= 0)
        {
            gravity = 10;
        }

        if (gravity < maxGravity)
        {
            gravity = maxGravity;
        }
    }

    [HideInInspector]
    public void Teleported(Vector3 pos)
    {
        characterController.enabled = false;
        transform.position = pos;
        characterController.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player2")
        {
            Physics.IgnoreCollision(characterController, collision.collider);
        }
    }
}
