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
        }
    }

    private void ToggleFire(bool isOn) 
    {
        hasFire = isOn;
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
