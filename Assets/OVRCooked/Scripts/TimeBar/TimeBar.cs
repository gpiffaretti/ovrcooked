using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    public CustomGradient gradient;
    public event Action TimeExpired;

    public Image timeBar;
    public float totalTimeSeconds;
    private float timeLeftSeconds;
    private float refreshUpdateSeconds = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void InitTimebar()
    {
        timeLeftSeconds = totalTimeSeconds;
        InvokeRepeating(nameof(DecreaseBar), 0.0f, refreshUpdateSeconds);
    }
    public void InitTimebar(float time)
    {
        totalTimeSeconds = time;
        InitTimebar();
    }

    void DecreaseBar()
    {
        timeLeftSeconds -= refreshUpdateSeconds;

        if (timeLeftSeconds <= 0)
        {
            CancelInvoke(nameof(DecreaseBar));
            TimeExpired?.Invoke();
        }

        timeBar.fillAmount = timeLeftSeconds / totalTimeSeconds;
        timeBar.color = gradient.Evaluate(timeBar.fillAmount);
    }
}
