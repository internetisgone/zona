using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voicelines : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    public AudioClip[] voicelines;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (voicelines.Length > 0)
            {
                int rand = UnityEngine.Random.Range(0, voicelines.Length);
                audioSource.PlayOneShot(voicelines[rand]);
            }
        }
    }
}
