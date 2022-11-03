using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class SoundManager : MonoBehaviour
{
    private bool muteBackgroundMusic = false;
    private bool muteSoudFX = false;

    public static SoundManager instance;
    private AudioSource audioSource;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void ToggleBackgroundMusic()
    {
        muteBackgroundMusic = !muteBackgroundMusic;

        if (muteBackgroundMusic)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.Play();
        }
    }

    public void ToggleSoundFX()
    {
        muteSoudFX = !muteSoudFX;
        GameEvents.ToggleSoundFXMethod();
    }

    public bool IsBackgroundMusicMuted()
    {
        return muteBackgroundMusic;
    }

    public bool IsSoundFXMuted()
    {
        return muteSoudFX;
    }

    public void SilienceBackgroundMusic(bool silience)
    {
        if (muteBackgroundMusic == false)
        {
            if (silience)
                audioSource.volume = 0f;
            else
                audioSource.volume = 0.045f;
        }
    }
}
