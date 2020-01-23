using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{

    int layerPot; 

    // Start is called before the first frame update
    void Start()
    {
        layerPot = LayerMask.NameToLayer("pot");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        // can be improved, we're getting this event for each collider in the pot
        if (other.gameObject.layer == layerPot)
        {
            AttemptFoodTransfer(other.gameObject.GetComponentInParent<Pot>());   
        }
    }

    private void AttemptFoodTransfer(Pot pot)
    {
        Debug.Log($"Attempt food transfer from {pot.name}");

        // check food ready

        // check pot is tilted over plate (dot product with 'up' vectors)

        // reset pot

        // create content in plate

        // trigger plate content changed event for UI
    }
}
