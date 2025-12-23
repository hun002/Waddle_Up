using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public Transform[] backgrounds; 
    public float backgroundWidth = 19.2f;
    public float scrollSpeed = 2f;

    void Update()
    {
        foreach (Transform bg in backgrounds)
        {
            bg.position += Vector3.left * scrollSpeed * Time.deltaTime;
        }

        Transform leftmost = backgrounds[0];

        float cameraLeftEdge = Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect;

        if (leftmost.position.x + backgroundWidth / 2 < cameraLeftEdge)
        {
            Transform rightmost = backgrounds[0];
            foreach (Transform bg in backgrounds)
                if (bg.position.x > rightmost.position.x) rightmost = bg;

            leftmost.position = new Vector3(
                rightmost.position.x + backgroundWidth,
                leftmost.position.y,
                leftmost.position.z
            );

            for (int i = 0; i < backgrounds.Length - 1; i++)
                backgrounds[i] = backgrounds[i + 1];
            backgrounds[backgrounds.Length - 1] = leftmost;
        }
    }
}
