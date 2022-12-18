using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialChange : MonoBehaviour
{
    [SerializeField]
    Text player1Text, player2Text;

    [SerializeField]
    string[] p1Text, p2Text;

    [SerializeField]
    int mode;

    [SerializeField]
    GameObject[] bridge;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || (mode == 2 && other.tag == "Player2"))
        {
            player1Text.text = "";
            player2Text.text = "";
            for (int i = 0; i < p1Text.Length; i++)
            {
                if(i < p1Text.Length - 1)
                {
                    player1Text.text += p1Text[i] + "\n";
                    player2Text.text = p2Text[i] + "\n";
                }
                else
                {
                    player1Text.text += p1Text[i];
                    player2Text.text += p2Text[i];
                }
            }

            if(mode == 1)
            {
                bridge[0].SetActive(false);
                bridge[1].SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
