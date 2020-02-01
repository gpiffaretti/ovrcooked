using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Trashcan : MonoBehaviour
{
    Animator animator;

    AudioSource audioSource;

    [SerializeField]
    AudioClip trashedElementSound;
    [SerializeField]
    AudioClip trashOpenSound;
    [SerializeField]
    AudioClip trashCloseSound;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenTrash() 
    {
        //Debug.Log("open lid");
        animator.ResetTrigger("open");
        animator.ResetTrigger("close");
        animator.SetTrigger("open");
        audioSource.PlayOneShot(trashOpenSound);
    }

    private void CloseTrash() 
    {
        //Debug.Log("close lid");
        animator.ResetTrigger("open");
        animator.ResetTrigger("close");
        animator.SetTrigger("close");
        audioSource.PlayOneShot(trashCloseSound);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("potBase")
            || other.gameObject.layer == LayerMask.NameToLayer("plate"))
        {
            OpenTrash();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("potBase")
            || other.gameObject.layer == LayerMask.NameToLayer("plate"))
        {
            CloseTrash();
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
        bool plateTilted = Vector3.Dot(plate.transform.up, Vector3.up) < 0f;
        if (!plateTilted || plate.IsEmpty()) return;

        // reset plate
        plate.Reset();
        audioSource.PlayOneShot(trashedElementSound);
    }

    private void AttemptTrashContent(Pot pot)
    {
        //Debug.Log($"Attempt trash pot {pot.name}");

        // check pot is tilted over plate (dot product with 'up' vectors)
        bool potTilted = Vector3.Dot(pot.transform.up, Vector3.up) < 0f;
        if (!potTilted || pot.IsEmpty()) return;

        // reset pot
        pot.Reset();
        audioSource.PlayOneShot(trashedElementSound);

    }
}
