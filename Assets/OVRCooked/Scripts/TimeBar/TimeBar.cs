using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    public CustomGradient gradient;

    public Image timeBar;
    public float totalTimeSeconds;
    private float timeLeftSeconds;
    private float refreshUpdateSeconds = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        timeLeftSeconds = totalTimeSeconds;
        InvokeRepeating(nameof(DecreaseBar), 0.0f, refreshUpdateSeconds);
        Destroy(transform.parent.gameObject.transform.parent.gameObject, totalTimeSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DecreaseBar()
    {
        timeLeftSeconds -= refreshUpdateSeconds;
        timeBar.fillAmount = timeLeftSeconds / totalTimeSeconds;
        timeBar.color = gradient.Evaluate(timeBar.fillAmount);
    }
}
