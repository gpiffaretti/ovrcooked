using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CookingFire : MonoBehaviour
{

    [SerializeField]
    [HideInInspector]
    private bool isOn;
    
    /// <summary>
    /// Normalized fire intensity
    /// </summary>
    [SerializeField]
    [HideInInspector]
    private float intensity;

    [SerializeField]
    Collider fireCollider;

    [SerializeField]
    ParticleSystem fireParticles;

    [SerializeField]
    AnimationCurve particleStartSizeCurve;

    [SerializeField]
    AudioClip stoveOnSound;
    [SerializeField]
    AudioClip stoveOffSound;

    private AudioSource audioSource;

    private Pot currentPot;

    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sets fire intensity
    /// </summary>
    public void SetIntensity(float normalizedIntensity) 
    {
        intensity = normalizedIntensity;

        // hack to manually execute logic of OnTriggerExit when turning off trigger
        if (isOn && intensity == 0f) // just turned off
        {
            OnTriggerDisabled();
            audioSource.PlayOneShot(stoveOffSound);
        }

        if (!isOn && intensity > 0f) // just turned on
        {
            audioSource.PlayOneShot(stoveOnSound);
        }

        isOn = intensity > 0f;
        fireCollider.enabled = isOn;
        
        // enable/disable particle system or adjust particle size based on intensity
        ParticleSystem.EmissionModule emission = fireParticles.emission;
        emission.enabled = intensity > 0f;
        float startSize = particleStartSizeCurve.Evaluate(intensity);

        ParticleSystem.MainModule mainModule = fireParticles.main;
        mainModule.startSize = startSize;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("potBase"))
        {
            currentPot = other.gameObject.GetComponentInParent<Pot>();
            currentPot.ToggleFire(true);
            //Debug.Log("pot-fire trigger enter");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("potBase"))
        {
            Debug.Log("pot-fire trigger exit");
            if (currentPot != null)
            {
                currentPot.ToggleFire(false);
                currentPot = null;
            }
            
        }
    }

    void OnTriggerDisabled()
    {
        // we need to check if there's a pot when disabling, because triggers won't
        // send the OnTriggerExit message
        if (currentPot != null) 
        {
            currentPot.ToggleFire(false);
        }
    }
}
