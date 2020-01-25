using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    public Image timeBar;
    public float totalTimeSeconds;
    private float timeLeftSeconds;

    // Start is called before the first frame update
    void Start()
    {
        timeLeftSeconds = totalTimeSeconds;
        InvokeRepeating(nameof(DecreaseBar), 0.0f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DecreaseBar()
    {
        timeLeftSeconds -= 0.1f;
        timeBar.fillAmount = timeLeftSeconds / totalTimeSeconds;
    }
}
