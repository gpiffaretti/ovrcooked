using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        
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
        fireCollider.enabled = intensity > 0f;

        // enable/disable particle system or adjust particle size based on intensity
        ParticleSystem.EmissionModule emission = fireParticles.emission;
        emission.enabled = intensity > 0f;
        float startSize = particleStartSizeCurve.Evaluate(intensity);

        ParticleSystem.MainModule mainModule = fireParticles.main;
        mainModule.startSize = startSize;
        
    }
}
