using UnityEngine;
using TMPro;

public class DistanceTextFromGameManager : MonoBehaviour
{
    public TMP_Text distanceText;
    public float scaleFactor = 0.1f; 

    void Update()
    {
        if (distanceText != null && GameManager.Instance != null)
        {
            float scaledDistance = GameManager.Instance.GetDistance() * scaleFactor;
            distanceText.text = $" {scaledDistance:F1} m";
        }
    }
}
