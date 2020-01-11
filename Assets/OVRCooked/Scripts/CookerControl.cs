using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookerControl : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    float controlRotation;

    [SerializeField]
    [HideInInspector]
    bool isOn;

    public CookingFire fire;

    // Control can be rotated from 0 to 270
    public float minRotationAngle = 0f;
    public float maxRotationAngle = 270f;

    public float minIntensityAngle = 45;
    public float maxIntensityAngle = 180;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        controlRotation = rot.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        controlRotation = rot.z;

        // hack to prevent cooker control to rotate in other axis other than Z
        transform.localRotation = Quaternion.Euler(0, 0, controlRotation);

        // set new fire intensity
        float intensity = GetNormalizedIntensity();
        isOn = intensity > 0f;
        fire.SetIntensity(intensity);

    }

    private float GetNormalizedIntensity() 
    {
        if (controlRotation > maxIntensityAngle || controlRotation < minIntensityAngle) return 0f;

        // linear interpolation formula
        float t = (controlRotation - minIntensityAngle) / (maxIntensityAngle - minIntensityAngle);

        return t;
    }
}
