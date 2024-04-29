using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource[] audioSources;
    private AudioSource bgSource;
    private AudioSource sfxSource;

    public AudioClip menuScreenBg;
    public AudioClip zonaBg;

    //public AudioMixer mixer;

    //private static float crossfadeDuration = 1f;
    //private static WaitForSeconds crossfadeDelay = new WaitForSeconds(crossfadeDuration);

    public static AudioPlayer instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        audioSources = GetComponents<AudioSource>();
        if (audioSources.Length < 2) return;

        bgSource = audioSources[0];
        sfxSource = audioSources[1];

        //bgSource.clip = menuScreenBg;
        //bgSource.Play();
        //bgSource.loop = true;
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        bgSource?.Stop();
        if (next.name == "Zona")
        {         
            ChangeAudio(zonaBg);
            //StartCoroutine("CrossfadeBgAudio");
        }
        else if (next.name == "MenuScreen")
        {
            ChangeAudio(menuScreenBg);
        }
    }

    private void ChangeAudio(AudioClip bg)
    {
        bgSource.clip = bg;
        bgSource.Play();
        bgSource.loop = true;
    }

    private void PlayAudioEffect()
    {

    }
}