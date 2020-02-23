using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class PlateUI : MonoBehaviour
{

    private Plate plate;
    public Plate Plate
    {
        get { return plate; }
        set
        {
            plate = value;
            plate.ContentChanged += OnContentChangedHandler;
            Inititialize();
        }
    }

    private CanvasGroup canvasGroup;


    // values used to lerp UI alpha based on vertical ortientation => Vector3.Dot(potUp, worldUp)
    const float MaxVerticalOrientation = 0.7f;
    const float MinVerticalOrientation = 0.3f;

    [SerializeField]
    Vector3 positionOffset;

    private bool hasFood;

    [SerializeField]
    Image[] ingredientIcons;


    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePositionAndRotation();
    }

    private void UpdatePositionAndRotation()
    {
        // POSITION
        transform.position = plate.transform.position + plate.transform.TransformDirection(positionOffset);

        // ROTATION
        Vector3 dir = transform.position - Camera.main.transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = rotation;


        float upwardsOrientation = Vector3.Dot(plate.transform.up, Vector3.up);
        //Debug.Log($"Upwards orientation:  {upwardsOrientation}");
        float lerpValue = (upwardsOrientation - MinVerticalOrientation) / (MaxVerticalOrientation - MinVerticalOrientation);
        float alphaLerpValue = Mathf.Clamp01(lerpValue);

        canvasGroup.alpha = alphaLerpValue;
    }

    private void Inititialize()
    {
        for (int i = 0; i < ingredientIcons.Length; i++)
        {
            ingredientIcons[i].gameObject.SetActive(false);
        }

    }

    private void OnContentChangedHandler(IngredientType[] ingredients)
    {
        for (int i = 0; i < ingredientIcons.Length; i++)
        {
            if (ingredients == null)
            {
                ingredientIcons[i].gameObject.SetActive(false);
            }
            else 
            {
                Sprite icon = GameUIManager.Instance.GetIngredientIcon(ingredients[i]);
                ingredientIcons[i].gameObject.SetActive(true);
                ingredientIcons[i].sprite = icon;
            }
            
        }
    }
}
