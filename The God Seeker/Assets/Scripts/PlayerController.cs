using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    Animator anim;
    int speed = 12, maxGravity = -5;

    [SerializeField] float gravity = 0, dashing = 0, attacking = 0;
    [SerializeField]int jumpTime = 1;

    Vector3 playerMovement, direction;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        if (attacking <= 0.2f)
            Dash();
        if (dashing <= 0.2f)
            Attack();
        Jump();
        Movement();
    }

    void Dash()
    {
        if (dashing > 0.2f)
        {
            dashing -= Time.deltaTime;
            maxGravity = -10;

            characterController.height = 2;
            characterController.center = Vector3.down;

            playerMovement = transform.forward * 30 * Time.deltaTime;
            characterController.Move(playerMovement);
            return;
        }

        if (dashing > 0)
        {
            anim.SetBool("Dashing", false);
            dashing -= Time.deltaTime;
            maxGravity = -5;

            characterController.center = Vector3.zero;
            characterController.height = 4;
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && jumpTime > 0)
        {
            anim.SetBool("Dashing", true);
            dashing = 0.5f;
            return;
        }


        if (dashing <= 0)
        {
            dashing = 0;
        }
    }

    void Attack()
    {
        if (attacking > 0.5f)
        {
            attacking -= Time.deltaTime;
            maxGravity = -10;

            playerMovement = transform.forward * 30 * Time.deltaTime;
            characterController.Move(playerMovement);
            return;
        }

        if (attacking > 0.2f)
        {
            //anim.SetBool("Dashing", false);
            attacking -= Time.deltaTime;
            maxGravity = -5;

            return;
        }

        if (Input.GetKeyDown(KeyCode.J) && jumpTime > 0)
        {
            //anim.SetBool("Dashing", true);
            attacking = 0.6f;
            return;
        }


        if (attacking <= 0)
        {
            attacking = 0;
        }
    }

    void Jump()
    {
        gravity -= 15f * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) &&
            jumpTime == 1 && dashing <= 0.2f && attacking <= 0.2f)
        {
            jumpTime = 0;
            speed = 5;
            gravity = 10;
            return;
        }

        if (characterController.isGrounded)
        {
            jumpTime = 1;
            speed = 10;
            gravity = 0;
            return;
        }

        if (gravity < maxGravity)
        {
            gravity = maxGravity;
        }
    }

    void Movement()
    {
        if(dashing <= 0.2f && attacking <= 0.2f)
        {
            float v = Input.GetAxis("Horizontal") * speed;
            float h = Input.GetAxis("Vertical") * speed;

            playerMovement = new Vector3(v, gravity, h) * Time.deltaTime;
        }
        else
        {
            playerMovement = new Vector3(0, gravity, 0) * Time.deltaTime;
        }

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
    }
}
