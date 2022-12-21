using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HP : MonoBehaviour
{
    public int healthPoint, hpMode;

    float damageTime, iFrame;

    [SerializeField]
    GameObject normal, damaged, enemyHitEffect, tutorial, tutorial2, boss, cutscene;

    [SerializeField]
    SkinnedMeshRenderer player;

    [SerializeField]
    SkinnedMeshRenderer[] bodyPart;
    [SerializeField]
    MeshRenderer[] modelPart;

    [SerializeField]
    public Material normalM, damagedM;
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
                hpBar.value = Mathf.Clamp(healthPoint, 0, 20);
                if (damageTime > 0)
                {
                    damageTime -= Time.deltaTime;
                    player.material = damagedM;
                }
                else
                {
                    player.material = normalM;
                }

                if (iFrame > 0)
                {
                    iFrame -= Time.deltaTime;
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
            case 5:
                if (damageTime > 0)
                {
                    damageTime -= Time.deltaTime;
                    foreach (MeshRenderer skin in modelPart)
                    {
                        skin.material = damagedM;
                    }
                }
                else
                {
                    foreach (MeshRenderer skin in modelPart)
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
                if (iFrame <= 0)
                {
                    iFrame = 0.5f;
                    healthPoint -= hp;
                    hpBar.value = Mathf.Clamp(healthPoint, 0, 20);
                    damageTime = 0.1f;
                    if (healthPoint <= 0)
                    {
                        gameObject.GetComponent<PlayerController>().enabled = false;
                        gameObject.GetComponent<Animator>().enabled = false;
                        gameObject.GetComponent<Ragdoll>().ActivateRagdoll();
                        StartCoroutine(Restart());
                    }
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
            case 5:
                healthPoint -= hp;
                hpBar.value = Mathf.Clamp(healthPoint, 0, 300);
                damageTime = 0.1f;
                if (healthPoint <= 0)
                {
                    Destroy(Instantiate(enemyHitEffect, transform.position, Quaternion.identity), 1);
                    cutscene.GetComponent<NextLevel>().LoadNext();
                    Destroy(boss);
                }
                break;
        }

        IEnumerator Restart()
        {
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


}
