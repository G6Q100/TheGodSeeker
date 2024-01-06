using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource AudioSource;

    public Slider volumeSlider;

    private float MusicVolume = 1f, tempVolume = 1;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource.Play();
        MusicVolume = PlayerPrefs.GetFloat("volume") * tempVolume;
        AudioSource.volume = MusicVolume;
        volumeSlider.value = MusicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            AudioSource.volume = MusicVolume / 2;
            PlayerPrefs.SetFloat("volume", MusicVolume / 2);
            return;
        }    


        AudioSource.volume = MusicVolume;
        PlayerPrefs.SetFloat("volume", MusicVolume);
    }

    public void VolumeUpdater(float volume)
    {
        MusicVolume = volume;
    }
}
