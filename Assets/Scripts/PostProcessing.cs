using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessing : MonoBehaviour
{
    Volume volume;
    FilmGrain filmGrain;

    private void Awake()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out filmGrain);
    }

    private void Update()
    {

    }
}
