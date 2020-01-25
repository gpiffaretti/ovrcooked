using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField]
    PlateUI plateUIPrefab;

    int layerPot;

    [SerializeField]
    GameObject foodObject;

    [SerializeField]
    bool hasFood;

    [SerializeField]
    IngredientType[] content;

    public event Action<IngredientType[]> ContentChanged;

    // Start is called before the first frame update
    void Start()
    {
        layerPot = LayerMask.NameToLayer("pot");
        hasFood = false;

        PlateUI plateUI = Instantiate<PlateUI>(plateUIPrefab);
        plateUI.Plate = this;
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
        
        // check empty plate and pot food ready
        if (hasFood || !pot.IsFoodReady()) return;

        // check pot is tilted over plate (dot product with 'up' vectors)
        bool potTilted = Vector3.Dot(pot.transform.up, Vector3.up) < 0f;
        if (!potTilted) return;

        // create content in plate
        this.content = pot.GetPotContent();
        foodObject.SetActive(true);
        foodObject.transform.localScale = Vector3.zero;
        foodObject.transform.DOScale(Vector3.one, 0.3f);

        // reset pot
        pot.Reset();

        // trigger plate content changed event for UI
        ContentChanged?.Invoke(content);
    }

    public void Reset()
    {
        foodObject.SetActive(false);
        hasFood = false;
        IngredientType[] content = null;

        // trigger plate content changed event for UI
        ContentChanged?.Invoke(content);
    }
}
