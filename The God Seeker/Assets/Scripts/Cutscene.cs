using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    public Animator trasition;

    public float trasitionTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CutsceneWait());
    }
    public IEnumerator CutsceneWait()
    {
        yield return new WaitForSeconds(9);

        trasition.SetTrigger("Start");

        yield return new WaitForSeconds(trasitionTime);

        SceneManager.LoadScene(0);

    }
}
