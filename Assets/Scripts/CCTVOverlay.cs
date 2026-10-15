using TMPro;
using UnityEngine;

public class CCTVOverlay : MonoBehaviour
{
    [SerializeField] TMP_Text recText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] string camName = "CAM 01 - KITCHEN";
    float t;

    void Update()
    {
        t += Time.unscaledDeltaTime;
        recText.alpha = ((int)(t * 1.5f) % 2 == 0) ? 1f : 0f;   // blinks
        var ts = System.TimeSpan.FromSeconds(t);
        timeText.text = $"{camName}\n{ts:hh\\:mm\\:ss}";
    }

    public void SetCamName(string n) { camName = n; }

}