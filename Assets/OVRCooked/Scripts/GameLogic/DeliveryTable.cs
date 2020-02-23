using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTable : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    AudioSource audioSource;
    [SerializeField] AudioClip clipSuccessfulDelivery;
    [SerializeField] AudioClip clipFailedDelivery;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("plate"))
        {
            Debug.Log("Delivery table detected plate!");
            Plate plate = other.GetComponentInParent<Plate>();
            bool success = gameManager.DeliverPlate(plate);

            audioSource.PlayOneShot(success? clipSuccessfulDelivery : clipFailedDelivery);
        }

    }

}
