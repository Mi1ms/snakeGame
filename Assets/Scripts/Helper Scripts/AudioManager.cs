using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioClip pickUp_sound, death_sound;

    void Start()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if(instance == null) {
            instance = this;
        }
        
    }

    public void Play_PickUpSound() {
        AudioSource.PlayClipAtPoint(pickUp_sound, transform.position);
    }

    public void Play_DeathSound() {
        AudioSource.PlayClipAtPoint(death_sound, transform.position);
    }
}
