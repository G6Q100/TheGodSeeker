using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField]
    int damage, knockback;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<HP>().Damaged(damage, knockback);
            Destroy(gameObject);
            return;
        }
        if (other.gameObject.tag == "Segment")
        {
            other.gameObject.GetComponent<SegmentRotation>().GetHit(damage, 0);
            Destroy(gameObject);
        }
    }
}
