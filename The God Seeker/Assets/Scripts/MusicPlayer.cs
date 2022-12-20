using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource AudioSource;

    public Slider volumeSlider;

    private float MusicVolume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource.Play();
        MusicVolume = PlayerPrefs.GetFloat("volume");
        AudioSource.volume = MusicVolume;
        volumeSlider.value = MusicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        AudioSource.volume = MusicVolume;
        PlayerPrefs.SetFloat("volume", MusicVolume);
    }

    public void VolumeUpdater(float volume)
    {
        MusicVolume = volume;
    }
}
