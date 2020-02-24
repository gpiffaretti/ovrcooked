using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Pot : MonoBehaviour
{
    [SerializeField]
    PotUI potUIPrefab;

    [SerializeField]
    IngredientType[] content;

    [SerializeField]
    bool hasFire;

    [SerializeField]
    AudioClip ingredientAddedSound;

    AudioSource audioSource;

    [SerializeField] AudioSource boilAudioSource;

    /// <summary>
    /// Index for next ingredient in the <content> array
    /// </summary>
    private int contentCurrentIndex;

    public int IngredientCount { get { return contentCurrentIndex; } }

    const float SecondsForCookCompletion = 10f;
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

        audioSource = GetComponent<AudioSource>();
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

            if (hasFire)
            {
                StartCookingProcess();
                PlayBoilAudio(true);
            }

            audioSource.PlayOneShot(ingredientAddedSound);
        }
    }

    public bool IsEmpty()
    {
        return IngredientCount == 0;
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

    public void ToggleFire(bool isOn) 
    {
        hasFire = isOn;

        FireChanged?.Invoke(hasFire); // trigger event

        if (hasFire && IngredientCount > 0)
        {
            StartCookingProcess();

            PlayBoilAudio(true);
        }
        else {
            PlayBoilAudio(false);
        }

        
    }

    void PlayBoilAudio(bool play) 
    {
        if (play)
        {
            if (!boilAudioSource.isPlaying)
            {
                boilAudioSource.volume = 0f;
                boilAudioSource.Play();
                boilAudioSource.DOFade(1f, 0.5f);
            }
        }
        else 
        {
            if (boilAudioSource.isPlaying)
            {
                boilAudioSource.DOFade(0f, 0.5f).OnComplete(() => {
                    boilAudioSource.Stop();
                });
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ingredient"))
        {
            OVRCookedGrabbable g = other.GetComponent<OVRCookedGrabbable>();
            if (g.isGrabbed) {
                Debug.Log($"Force release {g.name}");
                g.grabbedBy.ForceRelease(g, true);
            }
            AddIngredient(other.GetComponent<Ingredient>());
            Destroy(other.gameObject);
        }
    }

}
