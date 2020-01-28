using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplay : MonoBehaviour
{
    public Recipe recipe;

    public Image recipeArtwork;
    public Image[] ingredients;
    public Image[] cookware;

    // Start is called before the first frame update
    void Start()
    {
        recipeArtwork.sprite = recipe.recipeArtwork;
        
        foreach(var ingredient in ingredients)
        {
            ingredient.gameObject.SetActive(false);
        }

        foreach (var cookware in cookware)
        {
            cookware.gameObject.SetActive(false);
        }

        for (int i =0; i < ingredients.Length; i++)
        {
            ingredients[i].sprite = GameUIManager.Instance.GetIngredientIcon(recipe.ingredients[i]);
            ingredients[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < cookware.Length; i++)
        {
            cookware[i].sprite = GameUIManager.Instance.GetCookwareIcon(recipe.cookwares[i]);
            cookware[i].gameObject.SetActive(true);
        }
    }
}
