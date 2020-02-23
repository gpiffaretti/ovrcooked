using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsUI : MonoBehaviour
{
    GameManager gameManager;
    int points;

    [SerializeField]
    Text pointsText;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.PointsChanged += OnPointsChanged;
    }

    void OnPointsChanged(int newPoints)
    {
        StartCoroutine(IncreasePoints(newPoints));
    }

    IEnumerator IncreasePoints(int newPoints)
    {
        float duration = 0.8f;

        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            int score = (int)Mathf.Lerp(points, newPoints, progress);
            pointsText.text = $"{score}";
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
