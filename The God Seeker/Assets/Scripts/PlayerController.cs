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

    Vector3 playerMovement, direction;

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
            case "jump":
                Movement();
                Jump();
                break;
        }
    }

    void Attack()
    {
        attacking -= Time.deltaTime;
        if (attacking > 0.5f)
        {
            maxGravity = -10;

            playerMovement = transform.forward * 10 * Time.deltaTime;
            characterController.Move(playerMovement);
            return;
        }

        if (attacking > 0.2f)
        {
            //anim.SetBool("Dashing", false);
            maxGravity = -5;
            return;
        }

        if (attacking <= 0)
        {
            attacking = 0;
            mode = "normal";
        }
    }

    void Jump()
    {
        gravity -= 15f * Time.deltaTime;

        if (characterController.isGrounded)
        {
            jumpTime = 1;
            speed = 10;
            gravity = 0;
            mode = "normal";
            return;
        }

        if (gravity < maxGravity)
        {
            gravity = maxGravity;
        }
    }

    void Movement()
    {
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

        if (Input.GetKeyDown(KeyCode.J) && jumpTime > 0)
        {
            attacking = 0.6f;
            mode = "attack";
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) &&
            jumpTime == 1 && attacking <= 0)
        {
            jumpTime = 0;
            speed = 5;
            gravity = 10;
            mode = "jump";
        }
    }
}
