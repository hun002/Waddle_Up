using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    public Transform duck;

    [Header("자식 배경들 (아무 순서 OK)")]
    public Transform[] backgrounds;

    [Header("이동 속도")]
    public float speed = 0.6f;

    private float width;

    void Start()
    {
        width = backgrounds[0]
            .GetComponent<SpriteRenderer>()
            .bounds.size.x;
    }

    void Update()
    {
        foreach (Transform bg in backgrounds)
        {
            bg.position += Vector3.left * speed * Time.deltaTime;

            if (bg.position.x < duck.position.x - width)
            {
                MoveToRightEnd(bg);
            }
        }
    }

    void MoveToRightEnd(Transform bg)
    {
        float rightMostX = bg.position.x;

        foreach (Transform t in backgrounds)
        {
            if (t.position.x > rightMostX)
                rightMostX = t.position.x;
        }

        bg.position = new Vector3(
            rightMostX + width,
            bg.position.y,
            bg.position.z
        );
    }
}
