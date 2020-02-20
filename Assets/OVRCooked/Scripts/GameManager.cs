using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<float> GameStarted;
    public event Action GamePaused; // To use in the future
    public event Action GameEnded;

    public float gameTimeSeconds = 180;

    // Start is called before the first frame update
    void Start()
    {
        this.GameStarted?.Invoke(gameTimeSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
