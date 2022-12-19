using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentRotation : MonoBehaviour
{
    public float speed;

    public Vector3 direction;
    public Transform target;
    [SerializeField] HP bossHp;
    Quaternion lookAt;

    void Update()
    {
        lookAt = target.rotation * Quaternion.Euler(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, speed * Time.deltaTime);
    }

    public void GetHit(int damage, int knockback)
    {
        bossHp.Damaged(damage, knockback);
    }
}
