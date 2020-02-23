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
    public IngredientType[] Content { get { return content; } }

    public event Action<IngredientType[]> ContentChanged;


    // Start is called before the first frame update
    void Start()
    {
        layerPot = LayerMask.NameToLayer("pot");
        hasFood = false;

        PlateUI plateUI = Instantiate<PlateUI>(plateUIPrefab);
        plateUI.Plate = this;

        content = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EmptyAndResetPlate() 
    {
        // reset plate content
        foodObject.SetActive(false);
        content = null;
        ContentChanged?.Invoke(content);
        // if object is grabbed, force release before resetting, otherwise grab system will keep controlling its transform
        OVRCookedGrabbable g = GetComponent<OVRCookedGrabbable>();
        if (g.isGrabbed)
        {
            g.grabbedBy.ForceRelease(g);
        }

        // reset plate to original position
        GetComponent<ResettableTransform>().Reset();
        
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
        //Debug.Log($"Attempt food transfer from {pot.name}");
        
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
        content = null;

        // trigger plate content changed event for UI
        ContentChanged?.Invoke(content);
    }

    public bool IsEmpty()
    {
        return content == null;
    }
}
