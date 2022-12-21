using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyThunder : MonoBehaviour
{
    float dropSpace = Random.Range(0.35f,0.5f);
    int randX, randZ;
    [SerializeField] GameObject lightning;
    private void Update()
    {
        dropSpace -= Time.deltaTime;
        if (dropSpace < 0)
        {
            dropSpace = Random.Range(0.35f, 0.5f);
            randX = Random.Range(-25, 56);
            randZ = Random.Range(-20, 40);
            Vector3 spawnPoint = new Vector3(randX, 80, randZ);
            Instantiate(lightning, spawnPoint, Quaternion.Euler(-90, 0, 0));
        }
    }
}
