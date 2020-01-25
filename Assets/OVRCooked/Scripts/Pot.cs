using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField]
    PotUI potUIPrefab;

    [SerializeField]
    IngredientType[] content;

    [SerializeField]
    bool hasFire;

    /// <summary>
    /// Index for next ingredient in the <content> array
    /// </summary>
    private int contentCurrentIndex;

    public int IngredientCount { get { return contentCurrentIndex; } }

    const float SecondsForCookCompletion = 20f;
    const float CookingSpeed = 1f / SecondsForCookCompletion; // Speed => 1/seconds

    private bool cookProcessStarted;
    private float cookProgress = 0f; // normalized cooking progress

    public float CookProgress { get { return cookProgress; } }

    public event Action<bool> FireChanged;
    public event Action<IngredientType> IngredientAdded;
    public event Action CookProgressStarted;
    public event Action CookProgressFinished;
    public event Action PotReset;

    // Start is called before the first frame update
    void Start()
    {
        
        hasFire = false;
        Initialize();

        PotUI potUI = Instantiate<PotUI>(potUIPrefab);
        potUI.Pot = this;
    }

    void Initialize() 
    {
        content = new IngredientType[3];
        contentCurrentIndex = 0;
        cookProgress = 0f;
        cookProcessStarted = false;

        PotReset?.Invoke();
    }

    public void Reset() 
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFire) 
        {
            cookProgress += CookingSpeed * Time.deltaTime;
            if (cookProgress >= 1f && IsFull()) 
            {
                CookProgressFinished?.Invoke();
            }

            cookProgress = Mathf.Clamp01(cookProgress);
        }
    }

    public IngredientType[] GetPotContent() 
    {
        return content;
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

            cookProgress = Mathf.Clamp01(cookProgress - 0.3f); // TODO: make a constant for this

            IngredientAdded?.Invoke(ingredientType); // trigger event

            if (hasFire) StartCookingProcess();
        }
    }

    private bool IsFull() => IngredientCount == content.Length;

    public bool IsFoodReady() 
    {
        return IsFull() && CookProgress >= 1f;
    }

    private void StartCookingProcess() 
    {
        if (!cookProcessStarted) 
        {
            cookProcessStarted = true;
            CookProgressStarted?.Invoke();
        }
    }

    private void ToggleFire(bool isOn) 
    {
        hasFire = isOn;

        FireChanged?.Invoke(hasFire); // trigger event

        if (hasFire && IngredientCount > 0) 
        {
            StartCookingProcess();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("pot entered trigger");

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
        //Debug.Log("pot exit trigger");

        if (other.gameObject.layer == LayerMask.NameToLayer("fire"))
        {
            Debug.Log("fire trigger exit");
            ToggleFire(false);
        }

    }
}
