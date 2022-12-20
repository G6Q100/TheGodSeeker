using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossFight : MonoBehaviour
{
    [SerializeField] GameObject bossHp, boss, music;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            bossHp.SetActive(true);
            boss.SetActive(true);
            boss.transform.position = other.transform.position + Vector3.right * 35;
            music.SetActive(true);
            Destroy(gameObject);
        }
    }
}
