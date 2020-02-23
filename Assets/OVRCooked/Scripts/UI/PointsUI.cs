using System.Collections;
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
        gameManager.GameStarted += OnGameStarted;
        gameManager.PointsChanged += OnPointsChanged;
        gameManager.GameEnded += OnGameEnded;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetPoints();
    }

    private void OnGameStarted(float timeLeft)
    {
        ResetPoints();
    }

    private void ResetPoints()
    {
        pointsText.text = $"0";
    }

    void OnPointsChanged(int newPoints)
    {
        StartCoroutine(IncreasePoints(newPoints));
    }

    private void OnGameEnded()
    {
    }

    IEnumerator IncreasePoints(int newPoints)
    {
        float duration = 0.5f;

        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            int score = (int)Mathf.Lerp(points, newPoints, progress);
            pointsText.text = $"{score}";
            yield return null;
        }

        points = newPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
