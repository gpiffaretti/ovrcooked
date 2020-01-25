using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashcan : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("potBase")
            || other.gameObject.layer == LayerMask.NameToLayer("plate"))
        {
            Debug.Log("open lid");
            animator.ResetTrigger("open");
            animator.ResetTrigger("close");
            animator.SetTrigger("open");
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("potBase")
            || other.gameObject.layer == LayerMask.NameToLayer("plate"))
        {
            Debug.Log("close lid");
            animator.ResetTrigger("open");
            animator.ResetTrigger("close");
            animator.SetTrigger("close");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // can be improved, we're getting this event for each collider in the pot

        // POLYMORHPISM FOR POT AND PLATE?
        if (other.gameObject.layer == LayerMask.NameToLayer("potBase"))
        {
            AttemptTrashContent(other.gameObject.GetComponentInParent<Pot>());
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("plate"))
        {
            AttemptTrashContent(other.gameObject.GetComponentInParent<Plate>());
        }
    }

    private void AttemptTrashContent(Plate plate)
    {
        //Debug.Log($"Attempt trash plate {plate.name}");

        // check pot is tilted over plate (dot product with 'up' vectors)
        bool potTilted = Vector3.Dot(plate.transform.up, Vector3.up) < 0f;
        if (!potTilted) return;

        // reset plate
        plate.Reset();
    }

    private void AttemptTrashContent(Pot pot)
    {
        //Debug.Log($"Attempt trash pot {pot.name}");

        // check pot is tilted over plate (dot product with 'up' vectors)
        bool potTilted = Vector3.Dot(pot.transform.up, Vector3.up) < 0f;
        if (!potTilted) return;

        // reset pot
        pot.Reset();

    }
}
