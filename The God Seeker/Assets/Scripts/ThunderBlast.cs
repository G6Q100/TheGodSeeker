using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBlast : MonoBehaviour
{
    float animSpace, tempSpace = 3.2f;
    [SerializeField] GameObject lightning;

    private void Update()
    {
        animSpace += Time.deltaTime;
        if (animSpace < 3)
        {
            gameObject.transform.localScale = Vector3.one * 10f * Mathf.Sin(animSpace/3);
            return;
        }
        if (animSpace < 10f)
        {
            if(animSpace > tempSpace)
            {
                tempSpace += 0.2f;
                Instantiate(lightning, transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
            }
            return;
        }

        if (animSpace >= 12)
            Destroy(gameObject);

        gameObject.transform.localScale = Vector3.one * 10f * Mathf.Sin(( 12 - animSpace) / 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player2")
            return;
        if (other.gameObject.tag == "Enemy")
            return;
        if (other.gameObject.tag == "Segment")
            return;
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HP>().Damaged(1, 0);
        }
    }
}
