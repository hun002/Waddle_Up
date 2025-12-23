using System.Collections.Generic;
using UnityEngine;

public class InfiniteParallaxBackground : MonoBehaviour
{
    [Header("배경 Prefab 배열 (하늘, 중간, 땅1, 땅2, 땅3 순서)")]
    public GameObject[] backgroundPrefabs;

    [Header("레이어별 속도")]
    public float[] layerSpeeds = new float[5] { 0.2f, 0.6f, 2f, 2f, 2f };

    [Header("복제 간격")]
    public float spawnInterval = 0f;

    [Header("시간 간격 (초)")]
    public float spawnTimeInterval = 2f;

    [Header("고정 위치")]
    public Vector2 fixedPosition = Vector2.zero;

    [Header("무한 생성 최대 개수")]
    public int maxSpawnedObjects = 100;

    public enum LayerType { Sky = 0, Middle = 1, Ground1 = 2, Ground2 = 3, Ground3 = 4 }
    public LayerType layerType;

    private List<GameObject> spawnedBackgrounds = new List<GameObject>();
    private float scrollSpeed;
    private float lastSpawnTime;
    private float nextSpawnX;

    void Start()
    {
        scrollSpeed = layerSpeeds[(int)layerType];

        if (Camera.main == null)
        {
            Debug.LogError("MainCamera 태그 카메라 없음");
            return;
        }
        foreach (Transform child in transform)
        {
            if (child != null)
                spawnedBackgrounds.Add(child.gameObject);
        }
        if (spawnedBackgrounds.Count == 0)
        {
            Debug.LogError("첫 배경 배치 안됨 ");
            return;
        }

        GameObject first = spawnedBackgrounds[0];
        first.transform.position = new Vector3(fixedPosition.x, fixedPosition.y, first.transform.position.z);

        SpriteRenderer sr = spawnedBackgrounds[spawnedBackgrounds.Count - 1].GetComponent<SpriteRenderer>();
        nextSpawnX = spawnedBackgrounds[spawnedBackgrounds.Count - 1].transform.position.x + sr.bounds.size.x + spawnInterval;

        lastSpawnTime = Time.time;
    }

    void Update()
    {
        spawnedBackgrounds.RemoveAll(item => item == null);

        if (spawnedBackgrounds.Count == 0) return;

        foreach (GameObject bg in spawnedBackgrounds)
        {
            if (bg != null)
                bg.transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        }

        if (Time.time - lastSpawnTime >= spawnTimeInterval && spawnedBackgrounds.Count < maxSpawnedObjects)
        {
            GameObject prefab = backgroundPrefabs.Length > (int)layerType ? backgroundPrefabs[(int)layerType] : null;
            if (prefab == null)
            {
                Debug.LogWarning($"Prefab 없음: {layerType}");
                return;
            }

            GameObject last = spawnedBackgrounds[spawnedBackgrounds.Count - 1];
            if (last == null) return;

            SpriteRenderer lastSR = last.GetComponent<SpriteRenderer>();
            if (lastSR == null) return;

            Vector3 spawnPos = new Vector3(nextSpawnX, fixedPosition.y, last.transform.position.z);
            GameObject newBg = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
            spawnedBackgrounds.Add(newBg);

            nextSpawnX += lastSR.bounds.size.x + spawnInterval;
            lastSpawnTime = Time.time;
        }
    }
}
