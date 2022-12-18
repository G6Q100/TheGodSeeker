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
    GameObject normal, damaged, enemyHitEffect, tutorial, tutorial2;

    [SerializeField]
    SkinnedMeshRenderer player;

    [SerializeField]
    SkinnedMeshRenderer[] bodyPart;

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
                    player.material = damagedM;
                }
                else
                {
                    player.material = normalM;
                }
                break;
            case 3:
                if (damageTime > 0)
                {
                    damageTime -= Time.deltaTime;
                    foreach (SkinnedMeshRenderer skin in bodyPart)
                    {
                        skin.material = damagedM;
                    }
                }
                else
                {
                    foreach (SkinnedMeshRenderer skin in bodyPart)
                    {
                        skin.material = normalM;
                    }
                }
                break;
            case 4:
                if (damageTime > 0)
                {
                    damageTime -= Time.deltaTime;
                    foreach (SkinnedMeshRenderer skin in bodyPart)
                    {
                        skin.material = damagedM;
                    }
                }
                else
                {
                    foreach (SkinnedMeshRenderer skin in bodyPart)
                    {
                        skin.material = normalM;
                    }
                }
                break;
        }
    }

    public void Damaged(int hp, int knockback)
    {
        switch (hpMode)
        {
            case 1:
                healthPoint -= hp;
                damageTime = 0.1f;
                transform.position -= transform.forward * knockback;
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
                    player.gameObject.SetActive(false);
                    StartCoroutine(Restart());
                }
                break;
            case 3:
                healthPoint -= hp;
                damageTime = 0.1f;
                transform.position -= transform.forward * knockback;
                if (healthPoint <= 0)
                {
                    Destroy(Instantiate(enemyHitEffect, transform.position, Quaternion.identity), 1);
                    Destroy(gameObject);
                }
                break;
            case 4:
                healthPoint -= hp;
                damageTime = 0.1f;
                transform.position -= transform.forward * knockback;
                if (healthPoint <= 0)
                {
                    Destroy(Instantiate(enemyHitEffect, transform.position, Quaternion.identity), 1);
                    tutorial.SetActive(true);
                    tutorial2.SetActive(true);
                    Destroy(gameObject);
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
