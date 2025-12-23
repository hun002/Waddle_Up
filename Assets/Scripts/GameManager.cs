using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    [Header("게임 설정")]
    public float gameOverAngle = 45f;

    [Header("오리 질량 설정")]
    public Rigidbody2D bodyRb;
    public Rigidbody2D headRb;
    public Rigidbody2D leftLegRb;
    public Rigidbody2D rightLegRb;

    public float bodyMassBeforeStart = 2f;
    public float bodyMassAfterStart = 1f;
    public float headMassBeforeStart = 0.5f;
    public float headMassAfterStart = 2f;
    public float legMassBeforeStart = 2f;
    public float legMassAfterStart = 0.5f;

    [Header("레이어 이름")]
    public string bodyLayerName = "DuckBody";
    public string groundLayerName = "Ground";

    [Header("게임오버 캔버스")]
    public GameObject gameOverCanvas;
    public TMP_Text finalDistanceText;

    [Header("이동거리 축소 배율")]
    public float scaleFactor = 0.1f;

    [Header("사운드")]
    public AudioSource sfxSource;
    public AudioClip gameOverSfx;

    private int bodyLayer;
    private int groundLayer;

    public static GameManager Instance;

    private bool gameStarted = false;
    private bool gameOver = false;
    private float distance = 0f;

    private DuckController duckController;
    private DuckBodyController bodyController;

    void Awake()
    {
        Instance = this;

        duckController = FindObjectOfType<DuckController>();
        bodyController = FindObjectOfType<DuckBodyController>();

        bodyLayer = LayerMask.NameToLayer(bodyLayerName);
        groundLayer = LayerMask.NameToLayer(groundLayerName);

        if (bodyLayer == -1 || groundLayer == -1)
        {
            Debug.LogError("레이어 설정 오류(DuckBody/Ground)");
        }

        if (gameOverCanvas)
            gameOverCanvas.SetActive(false);
    }

    void Start()
    {
        Debug.Log("엔터를 눌러 게임 시작");
        SetMasses(true);
        SetBodyGroundCollision(true);
    }

    void Update()
    {
        if (!gameStarted && !gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Return))
                StartGame();
        }
        else if (gameStarted && !gameOver)
        {
            distance += duckController.walkSpeed * Time.deltaTime;
        }

        if (gameOver && Input.GetKeyDown(KeyCode.R))
            RestartGame();
    }

    void StartGame()
    {
        gameStarted = true;
        gameOver = false;
        distance = 0f;

        duckController.StartWalking();
        bodyController.StartBodyAngleChange();

        SetBodyGroundCollision(false);
        StartCoroutine(DelayedMassChange());

        if (gameOverCanvas)
            gameOverCanvas.SetActive(false);

        Debug.Log("게임 시작");
    }

    IEnumerator DelayedMassChange()
    {
        yield return new WaitForSeconds(2f);
        if (gameStarted && !gameOver)
            SetMasses(false);
    }

    void SetMasses(bool beforeStart)
    {
        if (bodyRb) bodyRb.mass = beforeStart ? bodyMassBeforeStart : bodyMassAfterStart;
        if (headRb) headRb.mass = beforeStart ? headMassBeforeStart : headMassAfterStart;
        if (leftLegRb) leftLegRb.mass = beforeStart ? legMassBeforeStart : legMassAfterStart;
        if (rightLegRb) rightLegRb.mass = beforeStart ? legMassBeforeStart : legMassAfterStart;
    }

    void SetBodyGroundCollision(bool enable)
    {
        if (bodyLayer < 0 || groundLayer < 0) return;
        Physics2D.IgnoreLayerCollision(bodyLayer, groundLayer, !enable);
    }

    public void TriggerGameOver(string reason)
    {
        if (gameOver) return;

        gameOver = true;
        gameStarted = false;

        if (sfxSource && gameOverSfx)
            sfxSource.PlayOneShot(gameOverSfx);

        duckController.StopWalking();
        duckController.LockInput();
        bodyController.StopBodyAngleChange();
        duckController.ChangeFace(true);

        SetBodyGroundCollision(true);

        bodyRb.velocity = Vector2.zero;
        bodyRb.angularVelocity = 0f;
        headRb.velocity = Vector2.zero;
        headRb.angularVelocity = 0f;
        leftLegRb.velocity = Vector2.zero;
        leftLegRb.angularVelocity = 0f;
        rightLegRb.velocity = Vector2.zero;
        rightLegRb.angularVelocity = 0f;

        float finalDistance = distance * scaleFactor;

        if (gameOverCanvas)
            gameOverCanvas.SetActive(true);

        if (finalDistanceText)
            finalDistanceText.text = $"{finalDistance:F1} m";

        Debug.Log("게임 오버 R = 재시작");
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public float GetDistance() => distance;
    public bool IsGameStarted() => gameStarted;
    public bool IsGameOver() => gameOver;
}
