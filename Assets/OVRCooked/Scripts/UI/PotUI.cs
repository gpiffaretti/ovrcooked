using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class PotUI : MonoBehaviour
{

    private Pot pot;
    public Pot Pot { 
        get { return pot; }
        set 
        {
            pot = value;
            pot.IngredientAdded += OnIngredientAddedHandler;
            pot.FireChanged += OnFireChangedHandler;
            pot.CookProgressStarted += OnCookProgressStarted;
            pot.CookProgressFinished += OnCookProgressFinished;
            pot.PotReset += OnPotReset;
            Inititialize();
        } 
    }

    private CanvasGroup canvasGroup;

    const float FireIconTweenDuration = 0.3f;

    // values used to lerp UI alpha based on vertical ortientation => Vector3.Dot(potUp, worldUp)
    const float MaxVerticalOrientation = 0.7f;
    const float MinVerticalOrientation = 0.3f;

    [SerializeField]
    Vector3 positionOffset;

    [SerializeField]
    Image fireIcon;

    [SerializeField]
    Image warningIcon;

    private bool hasFire;

    [SerializeField]
    Image[] ingredientIcons;

    int currentIngredientIndex;

    [SerializeField]
    Slider cookingSlider;

    // Start is called before the first frame update
    void Start()
    {
        currentIngredientIndex = 0;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFire) 
        {
            cookingSlider.normalizedValue = pot.CookProgress;
        }

        UpdatePositionAndRotation();
    }

    private void UpdatePositionAndRotation() 
    {
        // POSITION
        transform.position = pot.transform.position + pot.transform.TransformDirection(positionOffset);

        // ROTATION
        Vector3 dir = transform.position - Camera.main.transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = rotation;


        float upwardsOrientation = Vector3.Dot(pot.transform.up, Vector3.up);
        //Debug.Log($"Upwards orientation:  {upwardsOrientation}");
        float lerpValue = (upwardsOrientation - MinVerticalOrientation) / (MaxVerticalOrientation - MinVerticalOrientation);
        float alphaLerpValue = Mathf.Clamp01(lerpValue);

        canvasGroup.alpha = alphaLerpValue;
    }

    private void Inititialize() 
    {
        currentIngredientIndex = 0;
        for (int i = 0; i < ingredientIcons.Length; i++)
        {
            ingredientIcons[i].gameObject.SetActive(false);
        }

        cookingSlider.normalizedValue = 0f;
        cookingSlider.gameObject.SetActive(false);

        // hide fire icon
        fireIcon.gameObject.SetActive(true);
        fireIcon.DOFade(0f, 0f);

        warningIcon.gameObject.SetActive(false);
    }

    private void OnFireChangedHandler(bool hasFire)
    {
        
        fireIcon.DOKill();
        fireIcon.DOFade(hasFire ? 1f : 0f, FireIconTweenDuration);

        this.hasFire = hasFire;

    }

    private void OnIngredientAddedHandler(IngredientType ingredient) 
    {
        Sprite icon = GameUIManager.Instance.GetIngredientIcon(ingredient);

        ingredientIcons[currentIngredientIndex].gameObject.SetActive(true);
        ingredientIcons[currentIngredientIndex].sprite = icon;

        cookingSlider.normalizedValue = pot.CookProgress;

        currentIngredientIndex++;
    }

    private void OnCookProgressStarted() 
    {
        cookingSlider.gameObject.SetActive(true);
    }

    private void OnCookProgressFinished()
    {
        cookingSlider.gameObject.SetActive(false);
    }

    private void OnPotReset()
    {
        cookingSlider.gameObject.SetActive(false);
        warningIcon.gameObject.SetActive(false);
        foreach (var ing in ingredientIcons)
        {
            ing.gameObject.SetActive(false);
        }

        currentIngredientIndex = 0;
    }

}
