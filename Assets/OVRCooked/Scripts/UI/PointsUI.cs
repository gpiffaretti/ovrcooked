using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PointsUI : MonoBehaviour
{
    GameManager gameManager;
    int points;

    [SerializeField]
    Text pointsText;

    Tweener pointTween;

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
        IncreasePoints(newPoints);
    }

    private void OnGameEnded()
    {
    }

    void IncreasePoints(int newPoints)
    {
        float duration = 2f;

        int currentScore = this.points;

        if (pointTween != null) pointTween.Kill();
        pointTween = DOTween.To(() => currentScore, x => currentScore = x, newPoints, duration).SetEase(Ease.OutExpo)
            .OnUpdate(() => {
                pointsText.text = currentScore.ToString();
            })
            .OnComplete(() => {
                points = newPoints;
            });

    }

}
