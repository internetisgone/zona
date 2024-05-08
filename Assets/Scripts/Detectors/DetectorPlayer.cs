using System;
using UnityEngine;
using UnityEngine.Events;
public class DetectorPlayer : Detector
{
    public AudioClip beep;

    private Player Owner;
    private AudioSource audioSource;
    private GameObject detectorObj;
    private GameObject detectorLight;

    private float cachedProximity;

    private static float maxPitch = 1.5f;
    private static float minPitch = 1f;

    //public EventVoid ArtifactIsCollectible;
    //public EventVoid ArtifactNoLongerCollectible;
    public EventBool ArtifactIsCollectible;
    public EventFloat ArtifactProximityUpdated;
    public EventBool DetectorEquipped;
  
    GameObject firstPersonCamera;

    void Awake()
    {
        SetDetectorType(DetectorType.Echo);
        Owner = GetComponent<Player>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = beep;
    }

    void Start()
    {
        firstPersonCamera = transform.GetChild(1).gameObject;
        detectorObj = firstPersonCamera.transform.GetChild(0).GetChild(0).gameObject;
        if (DetectorType == DetectorType.Echo)
        {
            detectorLight = detectorObj.transform.GetChild(0).gameObject;
        }

        DetectorEquipped.RaiseEvent(Owner.StalkerData.DetectorEquipped);
    }

    void Update()
    {
        if (IsDetected)
        {
            CheckIfCollectible();
        }
    }

    private void OnEnable()
    {
        ArtifactProximityUpdated.OnEventRaised += OnProximityUpdated;
        DetectorEquipped.OnEventRaised += ActivateDetector;
    }

    private void OnDisable()
    {
        ArtifactProximityUpdated.OnEventRaised -= OnProximityUpdated;
        DetectorEquipped.OnEventRaised += ActivateDetector;
    }

    // search for artifacts within the detector's range
    public override void Detect()
    {
        Collider[] artifactColliders = Physics.OverlapSphere(transform.position, DetectorData.DetectionRange, ArtifactLayerMask);
        if (artifactColliders.Length > 0)
        {
            //Debug.LogFormat("{0} detected {1} artifacts using {2}", Owner.Name, artifactColliders.Length, DetectorData.DetectorType.ToString());
            IsDetected = true;
            float minDistance = DetectorData.DetectionRange;
            foreach (Collider collider in artifactColliders)
            {
                float distance = (transform.position - collider.transform.position).magnitude;

                // get shortest distance
                if (distance < minDistance) minDistance = distance;

                // check if within visibility range
                if (distance < DetectorData.VisibilityRange)
                {
                    Artifact artifact = collider.gameObject.GetComponent<Artifact>();
                    artifact.ToggleVisibility(true);
                }

                // Debug.LogFormat("{0} is within {1} meters", collider.gameObject.name, distance); 
            }
            ArtifactProximityUpdated.RaiseEvent(minDistance);
            cachedProximity = minDistance;
        }
        else
        {
            IsDetected = false;
            ArtifactProximityUpdated.RaiseEvent(0);
            cachedProximity = 0;
        }
    }

    // possible to collect visible artifacts when detector is not equipped 
    private void DetectVisible()
    {
        Collider[] artifactColliders = Physics.OverlapSphere(transform.position, Owner.StalkerData.CollectionRange, ArtifactLayerMask);
        IsDetected = false;

        if (artifactColliders.Length > 0)
        {
            foreach (Collider collider in artifactColliders)
            {
                Artifact artifact = collider.gameObject.GetComponent<Artifact>();
                if (artifact.IsVisible)
                {
                    IsDetected = true;
                }
            }
        }
    }

    // check if any of the detected artifacts is within arm's reach
    private void CheckIfCollectible()
    {
        Vector3 origin, direction;
        RaycastHit hit;

        origin = firstPersonCamera.transform.position;
        direction = firstPersonCamera.transform.forward;

        if (Physics.Raycast(origin, direction, out hit, Owner.StalkerData.CollectionRange, ArtifactLayerMask))
        // todo use non alloc
        {
            ArtifactIsCollectible.RaiseEvent(true);
            GameObject artifact = hit.collider.gameObject;
            artifact.GetComponent<Artifact>().PlayAnimation();
            if (Input.GetKey(KeyCode.F) && Owner.StalkerData.MovementEnabled)
            {
                Owner.CollectArtifact(artifact);
                ArtifactProximityUpdated.RaiseEvent(0);
            }
        }
        else
        {
            ArtifactIsCollectible.RaiseEvent(false);
        }
    }

    private void ActivateDetector(bool isActive)
    {
        Owner.StalkerData.DetectorEquipped = isActive;

        if (isActive)
        {
            CancelInvoke("DetectVisible");
            detectorObj.SetActive(true);
            InvokeRepeating("Detect", 1f, DetectorData.Interval);
            // todo
            // use shorter interval eg 0.2 once detected for smoother beep frequency transition
        }
        else
        {
            CancelInvoke("Detect");
            StopBeepAndBlink();
            IsDetected = false;
            detectorObj.SetActive(false);
            ArtifactIsCollectible.RaiseEvent(false);
            // ClearProximityDisplay();
            InvokeRepeating("DetectVisible", 0f, DetectorData.Interval);
        }
    }

    private void OnProximityUpdated(float proximity)
    {
        if (proximity == 0f)
        {
            StopBeepAndBlink();
        }
        else
        {
            if (CloseEnough(cachedProximity, proximity, 0.01f)) return;

            StopBeepAndBlink();

            audioSource.pitch = CalcBeepPitch(proximity);
            float interval = 10 * beep.length * proximity / DetectorData.DetectionRange;

            InvokeRepeating("PlayBeep", 0, interval);
            InvokeRepeating("BlinkLight", 0, interval / 2);
        }
    }

    private void PlayBeep()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(beep);
        }
    }

    private float CalcBeepPitch(float proximity)
    {
        // linear relationship between proximity and pitch, with a negative coef
        float proximityNormalized = proximity / DetectorData.DetectionRange;
        float pitch = maxPitch + proximityNormalized * (minPitch - maxPitch);
        return pitch;
    }

    private void BlinkLight()
    {
        detectorLight.SetActive(!detectorLight.activeSelf);
    }

    private void StopBeepAndBlink()
    {
        CancelInvoke("PlayBeep");
        CancelInvoke("BlinkLight");
        audioSource.Stop();
        if (DetectorType == DetectorType.Echo)
        {
            detectorLight.SetActive(false);

        }
    }

    //private void ClearProximityDisplay()
    //{
    //    ArtifactProximityUpdated.RaiseEvent(0);
    //}

    // todo put in utils
    private bool CloseEnough(float x, float y, float tolerance)
    {
        return Math.Abs(x - y) <= tolerance;
    }

# if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // draw detection radius
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, DetectorData.DetectionRange);
        }
    }
# endif
}
