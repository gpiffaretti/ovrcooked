using Assets.OVRCooked.Scripts.Orders;
using UnityEngine;
using UnityEngine.UI;

public class OrderDisplay : MonoBehaviour
{
    public Order order;

    public Image recipeArtwork;
    public Image[] ingredients;
    public Image[] cookwares;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TimeBar>().TimeExpired += OnTimeExpired;
    }

    private void OnTimeExpired()
    {
        
    }

    public void Initialize(Order order)
    {
        this.order = order;

        recipeArtwork.sprite = order.recipe.recipeArtwork;

        foreach (var ingredient in ingredients)
        {
            ingredient.gameObject.SetActive(false);
        }

        foreach (var cookware in cookwares)
        {
            cookware.gameObject.SetActive(false);
        }

        for (int i = 0; i < order.recipe.ingredients.Length; i++)
        {
            ingredients[i].sprite = GameUIManager.Instance.GetIngredientIcon(order.recipe.ingredients[i]);
            ingredients[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < order.recipe.cookwares.Length; i++)
        {
            cookwares[i].sprite = GameUIManager.Instance.GetCookwareIcon(order.recipe.cookwares[i]);
            cookwares[i].gameObject.SetActive(true);
        }
    }
}
