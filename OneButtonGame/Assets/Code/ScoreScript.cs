using System;
using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public bool active = false;
    public float elapsedTime;
    public float delay = 1;
    public float timeTo;

    void Update()
    {
        if (active)
        {
            elapsedTime += Time.deltaTime * delay;
        }
        timerText.text = TimeSpan.FromSeconds(elapsedTime).ToString("mm\\:ss\\.fff");
        if (elapsedTime >= timeTo)
        {
            delay = 1;
        }
    }
    public void Counting(bool b)
    {
        active = b;
    }
    public void DelayTime(float f, int i)
    {
        delay = f;
        timeTo = elapsedTime + i;
    }
}
