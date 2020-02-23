using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("plate") || other.gameObject.layer == LayerMask.NameToLayer("pot"))
        {
            other.GetComponentInParent<ResettableTransform>().Reset();
        }
    }
}
