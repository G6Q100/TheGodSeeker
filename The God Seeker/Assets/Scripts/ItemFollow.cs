using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFollow : MonoBehaviour
{
    [SerializeField] GameObject hand;
    void LateUpdate()
    {
        transform.SetParent(hand.transform);
        transform.localPosition = new Vector3(0.15f, 0.2f, 0);
        transform.localRotation = Quaternion.Euler(0, 20, 180);
        transform.localScale = new Vector3(16, 20, 15);
        transform.SetParent(transform);
    }
}
