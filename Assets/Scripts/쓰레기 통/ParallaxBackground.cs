using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("배경 스프라이트들 (왼→오 순서)")]
    public Transform[] backgrounds;

    [Header("레이어 타입")]
    public LayerType layerType;

    private float scrollSpeed;
    private float bgWidth;
    private Camera cam;

    public enum LayerType
    {
        Sky,
        Middle,
        Ground
    }

    void Start()
    {
        if (backgrounds.Length < 2)
        {
            Debug.LogError("배경은 최소 2개 필요함");
            return;
        }

        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("MainCamera 태그가 붙은 카메라가 없음");
            return;
        }

        // 레이어별 속도 설정
        switch (layerType)
        {
            case LayerType.Sky: scrollSpeed = 0.2f; break;
            case LayerType.Middle: scrollSpeed = 0.6f; break;
            case LayerType.Ground: scrollSpeed = 2.0f; break;
        }

        // 배경 가로 길이 계산
        SpriteRenderer sr = backgrounds[0].GetComponent<SpriteRenderer>();
        bgWidth = sr.bounds.size.x;

        // 초기 위치 정렬
        for (int i = 0; i < backgrounds.Length; i++)
        {
            Vector3 pos = backgrounds[0].position;
            pos.x += bgWidth * i;
            backgrounds[i].position = pos;
        }
    }

    void Update()
    {
        float camLeftEdge = cam.transform.position.x - cam.orthographicSize * cam.aspect;

        foreach (Transform bg in backgrounds)
        {
            // 배경 이동
            bg.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

            // 배경이 화면 왼쪽 끝 완전히 벗어나면 뒤로 보내기
            if (bg.position.x + bgWidth / 2 < camLeftEdge)
            {
                MoveToBack(bg);
            }
        }
    }

    void MoveToBack(Transform target)
    {
        // 현재 가장 오른쪽 배경 찾기
        Transform rightMost = backgrounds[0];
        foreach (Transform bg in backgrounds)
        {
            if (bg.position.x > rightMost.position.x)
                rightMost = bg;
        }

        // 정확히 이어지도록 새 위치 계산
        Vector3 newPos = rightMost.position;
        newPos.x += bgWidth;
        target.position = newPos;
    }
}
