using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;

    public static SoundManager instance;

    private void OnEnable()
    {       
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlaySound("Theme");
        }
        else
        {
            StopSound("Theme");
        }        
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }          
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;

            sound.source.loop = sound.loop;
        }
    }

    public void PlaySound(string name)
    {
        Sound sound = Array.Find(sounds, so => so.name == name);
        if (sound == null)
        {
            Debug.LogError("Clip name " + name + " not available");
            return;
        }

        sound.source.Play();
    }

    public void PlaySoundOnce(string name)
    {
        Sound sound = Array.Find(sounds, so => so.name == name);
        if (sound == null)
        {
            Debug.LogError("Clip name " + name + " not available");
            return;
        }

        sound.source.PlayOneShot(sound.clip);
    }

    public void StopSound(string name)
    {
        Sound sound = Array.Find(sounds, so => so.name == name);
        if (sound == null)
        {
            Debug.LogError("Clip name " + name + " not available");
            return;
        }

        sound.source.volume = sound.volume * (1.0f + UnityEngine.Random.Range(-sound.volume / 2.0f, sound.volume / 2.0f));
        sound.source.pitch = sound.volume * (1.0f + UnityEngine.Random.Range(-sound.pitch / 2.0f, sound.pitch / 2.0f));

        sound.source.Stop();
    }
}
