using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTable : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("plate"))
        {
            Debug.Log("Delivery table detected plate!");
            Plate plate = other.GetComponentInParent<Plate>();
            gameManager.DeliverPlate(plate);
        }

    }

}
