using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        
        SFXSlider.minValue = 0;
        SFXSlider.maxValue = 1;
        SFXSlider.wholeNumbers = true;

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume();
        }
        else
        {
            SFXSlider.value = 1f; 
            SetSFXVolume();
        }
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;

        if (volume == 0)
        {
            myMixer.SetFloat("SFX", -80f);
        }
        else
        {
            myMixer.SetFloat("SFX", 0f); 
        }

        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        SFXSlider.value = savedVolume;
        SetSFXVolume();
    }
}
