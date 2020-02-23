using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResettableTransform : MonoBehaviour
{

    Vector3 initialPosition;
    Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void Reset()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

}
