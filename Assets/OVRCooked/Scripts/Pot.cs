using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField]
    IngredientType[] content;

    [SerializeField]
    bool hasFire;

    private int contentCurrentIndex;

    const float CookingSpeed = 0.016666f; // Speed => 1/60 seconds

    private float cookProgress = 0f; // normalized cooking progress

    public event Action<bool> fireChanged;
    public event Action<IngredientType> ingredientAdded;
    public event Action<float> cookProgressChanged;

    // Start is called before the first frame update
    void Start()
    {
        content = new IngredientType[3];
        contentCurrentIndex = 0;
        hasFire = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFire) 
        {
            cookProgress += CookingSpeed * Time.deltaTime;
            cookProgressChanged?.Invoke(cookProgress); // trigger event
        }
    }

    private void AddIngredient(Ingredient ingredient) 
    {
        IngredientType ingredientType = ingredient.Type;
        
        Debug.Log($"new ingredient add attempt => {ingredientType}");

        if (contentCurrentIndex == content.Length)
        {
            Debug.Log($"Pot is FULL!!!");
        }
        else
        {
            Debug.Log($"Ingredient added!");
            content[contentCurrentIndex] = ingredientType;
            contentCurrentIndex++;

            ingredientAdded?.Invoke(ingredientType); // trigger event
        }
    }

    private void ToggleFire(bool isOn) 
    {
        hasFire = isOn;

        fireChanged?.Invoke(hasFire); // trigger event
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("pot entered trigger");

        if (other.gameObject.layer == LayerMask.NameToLayer("fire"))
        {
            Debug.Log("fire trigger enter");
            ToggleFire(true);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("ingredient"))
        {
            AddIngredient(other.GetComponent<Ingredient>());
            Destroy(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("pot exit trigger");

        if (other.gameObject.layer == LayerMask.NameToLayer("fire"))
        {
            Debug.Log("fire trigger exit");
            ToggleFire(false);
        }

    }
}
