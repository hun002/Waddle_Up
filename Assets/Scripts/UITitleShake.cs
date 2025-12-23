using UnityEngine;

public class UITitleShake : MonoBehaviour
{
    public float amount = 5f;
    public float speed = 1.5f;

    RectTransform rt;
    Vector2 origin;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        origin = rt.anchoredPosition;
    }

    void OnEnable()
    {
        origin = rt.anchoredPosition;
    }

    void Update()
    {
        float t = Time.unscaledTime;

        float x = Mathf.Sin(t * speed) * amount;
        float y = Mathf.Cos(t * speed * 0.8f) * amount;

        rt.anchoredPosition = origin + new Vector2(x, y);
    }
}
