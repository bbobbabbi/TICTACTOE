using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    [SerializeField] private List<AudioSource> Sounds;
     

    public void SoundOff()
    {
        AudioListener.volume = 0;
    }
    public void SoundOn()
    {
        AudioListener.volume = 1;
    }

    public void PlayAudioClip(int AudioNumber) {
        if (!Sounds[AudioNumber].IsUnityNull())
        {
            Sounds[AudioNumber].Play();
        }
    }
}
