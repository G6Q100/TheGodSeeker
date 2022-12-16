using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    private float existTime = 50;

    public Rigidbody rb;

    public GameObject bulletHitEffect;

    private void FixedUpdate()
    {
        existTime -= Time.deltaTime;
        rb.velocity = (transform.rotation * Vector3.forward * speed);

        if (GetComponent<ParticleSystem>().isStopped || existTime <= 0)
            Destroy(gameObject, 2f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player2")
        {
            Physics.IgnoreCollision(GetComponent<SphereCollider>(), collision.collider);
        }
        else
        {
            Destroy(Instantiate(bulletHitEffect, transform.position, Quaternion.identity), 1);
            GetComponent<ParticleSystem>().Stop();
        }
    }
}
