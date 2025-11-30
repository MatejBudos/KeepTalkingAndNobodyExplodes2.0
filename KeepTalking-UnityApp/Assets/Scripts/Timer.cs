using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    public int currentSeconds;
    public int currentMinutes;
    void Start()
    {
        
    }

    void Update()
    {
        if (remainingTime < 10)
        {
            timerText.color = Color.red;
        }
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
        }

        currentMinutes = Mathf.FloorToInt(remainingTime / 60);
        currentSeconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", currentMinutes, currentSeconds);   
    }
}
