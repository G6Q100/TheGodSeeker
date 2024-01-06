using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField]
    int damage, knockback, mode = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<HP>().Damaged(damage, knockback);
            if (mode == 2)
            {
                Player2Controller player2 = GameObject.Find("Player2").GetComponent<Player2Controller>();
                player2.hitTime++;
                player2.hitTimeSlider.value = player2.hitTime;

                if (player2.hitTime >= 20)
                {
                    player2.hitTime = 0;
                    player2.SpwanHeal();
                }
            }
            return;
        }
        if (other.gameObject.tag == "Segment")
        {
            if (mode == 2)
            {
                Player2Controller player2 = GameObject.Find("Player2").GetComponent<Player2Controller>();
                player2.hitTime++;
                player2.hitTimeSlider.value = player2.hitTime;

                if (player2.hitTime >= 20)
                {
                    player2.hitTime = 0;
                    player2.SpwanHeal();
                }
            }
        }
    }
}
