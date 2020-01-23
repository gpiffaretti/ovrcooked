using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PotUI : MonoBehaviour
{

    private Pot pot;
    public Pot Pot { 
        get { return pot; }
        set 
        {
            pot = value;
            pot.ingredientAdded += OnIngredientAddedHandler;
            pot.fireChanged += OnFireChangedHandler;
            pot.cookProgressChanged += OnCookProgressChangedHandler;
        } 
    }

    const float FireIconTweenDuration = 0.3f;

    [SerializeField]
    Image fireIcon;

    [SerializeField]
    Image[] ingredientIcons;

    int currentIngredientIndex;

    [SerializeField]
    Slider cookingSlider;

    // Start is called before the first frame update
    void Start()
    {
        currentIngredientIndex = 0;
        Pot = GetComponentInParent<Pot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFireChangedHandler(bool hasFire)
    {
        fireIcon.DOKill();
        fireIcon.DOFade(hasFire ? 1f : 0f, FireIconTweenDuration);
        
    }

    private void OnIngredientAddedHandler(IngredientType ingredient) 
    {
        Sprite icon = GameUIManager.Instance.GetIngredientIcon(ingredient);

        ingredientIcons[currentIngredientIndex].gameObject.SetActive(true);
        ingredientIcons[currentIngredientIndex].sprite = icon;

        currentIngredientIndex++;
    }

    private void OnCookProgressChangedHandler(float normalizedProgress) 
    {
        cookingSlider.normalizedValue = normalizedProgress;
    }
}
