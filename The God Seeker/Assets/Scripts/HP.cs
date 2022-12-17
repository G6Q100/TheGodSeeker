using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HP : MonoBehaviour
{
    [SerializeField]
    int healthPoint, hpMode;

    float damageTime;

    [SerializeField]
    GameObject normal, damaged, enemyHitEffect;

    [SerializeField]
    GameObject player;
    [SerializeField]
    Material normalM, damagedM;
    [SerializeField]
    Slider hpBar;

    private void Update()
    {
        switch (hpMode)
        {
            case 1:
                if (damageTime > 0)
                {
                    damageTime -= Time.deltaTime;
                    normal.SetActive(false);
                    damaged.SetActive(true);
                }
                else
                {
                    normal.SetActive(true);
                    damaged.SetActive(false);
                }
                break;
            case 2:
                if (damageTime > 0)
                {
                    damageTime -= Time.deltaTime;
                    player.GetComponent<SkinnedMeshRenderer>().material = damagedM;
                }
                else
                {
                    player.GetComponent<SkinnedMeshRenderer>().material = normalM;
                }
                break;
        }
    }

    public void Damaged(int hp)
    {
        switch (hpMode)
        {
            case 1:
                healthPoint -= hp;
                damageTime = 0.1f;
                transform.position -= transform.forward;
                if (healthPoint <= 0)
                {
                    Destroy(Instantiate(enemyHitEffect, transform.position, Quaternion.identity), 1);
                    Destroy(gameObject);
                }
                break;
            case 2:
                healthPoint -= hp;
                hpBar.value = Mathf.Clamp(healthPoint, 0, 20);
                damageTime = 0.1f;
                if (healthPoint <= 0)
                {
                    gameObject.GetComponent<PlayerController>().enabled = false;
                    player.SetActive(false);
                    StartCoroutine(Restart());
                }
                break;
        }

        IEnumerator Restart()
        {
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("Level1");
        }
    }


}
