using UnityEngine;

public class HeadController : MonoBehaviour
{
    public Rigidbody2D headRb;
    public float torqueForce = -1500f;

    public float minRandomTorque = 10000f;
    public float maxRandomTorque = 18000f;
    public float minRandomInterval = 0.2f;
    public float maxRandomInterval = 0.8f;

    float torqueInput = 0f;
    float randomTimer = 0f;
    float nextRandomInterval;
    bool isEnterPressed = false;
    float enterPressedTime = 0f;
    public float randomDelay = 2f;

    void Start()
    {
        nextRandomInterval = Random.Range(minRandomInterval, maxRandomInterval);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver())
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            isEnterPressed = true;
            enterPressedTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            torqueInput = 1f;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            torqueInput = -1f;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver())
            return;

        if (Mathf.Abs(torqueInput) > 0.01f)
        {
            float randomFactor = Random.Range(0.9f, 1.1f);
            headRb.AddTorque(torqueInput * torqueForce * randomFactor);
            torqueInput = 0f;
        }

        if (isEnterPressed 
            && Time.time - enterPressedTime >= randomDelay
            && GameManager.Instance != null 
            && GameManager.Instance.IsGameStarted())
        {
            randomTimer += Time.fixedDeltaTime;
            if (randomTimer >= nextRandomInterval)
            {
                randomTimer = 0f;
                nextRandomInterval = Random.Range(minRandomInterval, maxRandomInterval);

                float randomSign = Random.value < 0.5f ? -1f : 1f;
                float randomAmount = randomSign * Random.Range(minRandomTorque, maxRandomTorque);
                headRb.AddTorque(randomAmount);
            }
        }
    }
}
