using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource audioSource;
    //public AudioMixer mixer;

    //private static float crossfadeDuration = 1f;
    //private static WaitForSeconds crossfadeDelay = new WaitForSeconds(crossfadeDuration);

    private void Awake()
    {
        if (clips.Length == 0)
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        audioSource.Play();
        audioSource.loop = true;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        if (next.name == "Zona")
        {
            audioSource.Stop();
            ChangeAudio();
            //StartCoroutine("CrossfadeBgAudio");
        }
    }

    private void ChangeAudio()
    {
        audioSource.clip = clips[1];
        audioSource.Play();
        audioSource.loop = true;
    }
}