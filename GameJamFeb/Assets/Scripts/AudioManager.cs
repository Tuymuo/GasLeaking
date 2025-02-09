using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [Header("------ Audio Sourse ---------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    [Header("------ Audio Clip ---------")]
    public AudioClip mainmotive;


    private void Start()
    {
        musicSource.clip = mainmotive;
        musicSource.Play();
        Debug.Log("Playing Music: " + musicSource.clip.name);
    }
}
