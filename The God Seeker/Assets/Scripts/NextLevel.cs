using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public Animator trasition;

    public float trasitionTime = 1f;

    //[SerializeField]
    //string nextLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //SceneManager.LoadScene(nextLevel);
            LoadNext();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadNext()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public IEnumerator LoadLevel(int levelIndex)
    {
        trasition.SetTrigger("Start");

        yield return new WaitForSeconds(trasitionTime);

        SceneManager.LoadScene(levelIndex);

    }
}
