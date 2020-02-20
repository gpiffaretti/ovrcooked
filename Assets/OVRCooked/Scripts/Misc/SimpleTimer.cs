using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTimer : MonoBehaviour
{
    [SerializeField]
    bool running = false;

    [SerializeField]
    float totalTime = 60f;

    [SerializeField]
    float elapsedTime = 0f;

    public float TimeRemaining { get { return totalTime - elapsedTime; } }

    public event Action Completed;

    public void SetTimer(float totalTime)
    {
        this.totalTime = totalTime;
        this.elapsedTime = 0f;
    }

    /// <summary>
    /// Starts the timer from the begginning.
    /// </summary>
    public void StartTimer() {
        running = true;
        elapsedTime = 0f;
    }

    /// <summary>
    /// Pause timer, keeps state.
    /// </summary>
    public void Pause() {
        running = false;
    }

    /// <summary>
    /// Resumes timer after being paused.
    /// </summary>
    public void Resume()
    {
        running = true;
    }

    /// <summary>
    /// Sets timer to zero and stops it if running.
    /// </summary>
    public void Reset()
    {
        running = false;
        elapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (running) {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= totalTime) 
            {
                elapsedTime = totalTime;
                Completed?.Invoke();
            }
        }
    }
}
