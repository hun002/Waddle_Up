using UnityEngine;

public class DuckController : MonoBehaviour
{
    [Header("오리 파츠")]
    public Transform leftLeg;
    public Transform rightLeg;

    [Header("몸통")]
    public Transform body;

    [Header("이동 설정")]
    public float walkSpeed = 2f;
    public float walkBounceHeight = 0.1f;

    [Header("다리 애니메이션")]
    public float legSwingAngle = 20f;
    public float legAnimSpeed = 2f;

    [Header("다리 Y 범위 (엔터 이전)")]
    public float minLegY_Before = 0.2f;
    public float maxLegY_Before = 0.25f;

    [Header("다리 Y 범위 (엔터 이후)")]
    public float minLegY_After = 0f;
    public float maxLegY_After = 1f;

    [Header("얼굴 이미지")]
    public SpriteRenderer faceRenderer;
    public Sprite normalFace;
    public Sprite gameOverFace;

    [Header("머리")]
    public Transform head;
    [Header("머리 보정")]


    public float headMinY = 0.2f;  
    public float headMaxY = 1.0f;
    public float maxBodyAngle = 40f;
    private bool isWalking = false;
    private bool inputLocked = false;
    private float legAnimationTime = 0f;
    private float fixedX;

    void Start()
    {
        fixedX = transform.position.x;
        ChangeFace(false);
    }

    void Update()
    {
        if (inputLocked)
            return;

        if (isWalking)
        {
            HandleMovement();
            AnimateLegs();
        }

        ApplyLegYLimit();
    }

    void HandleMovement()
    {
        fixedX += walkSpeed * Time.deltaTime;

        float bounceOffset =
            Mathf.Sin(legAnimationTime * legAnimSpeed) * walkBounceHeight;

        transform.position = new Vector3(
            fixedX,
            transform.position.y + bounceOffset * Time.deltaTime,
            transform.position.z
        );
    }

    void AnimateLegs()
    {
        legAnimationTime += Time.deltaTime;

        float leftLegAngle =
            Mathf.Sin(legAnimationTime * legAnimSpeed) * legSwingAngle;
        float rightLegAngle =
            Mathf.Sin(legAnimationTime * legAnimSpeed + Mathf.PI) * legSwingAngle;

        leftLeg.rotation = Quaternion.Euler(0, 0, leftLegAngle);
        rightLeg.rotation = Quaternion.Euler(0, 0, rightLegAngle);
    }

    void ApplyLegYLimit()
    {
        float loosenRatio = 1f;

        if (body != null)
        {
            float bodyAngle = body.eulerAngles.z;
            if (bodyAngle > 180f) bodyAngle -= 360f;

            float absAngle = Mathf.Abs(bodyAngle);
            loosenRatio = Mathf.InverseLerp(90f, 80f, absAngle);
            loosenRatio = Mathf.Clamp01(loosenRatio);
        }

        bool started = GameManager.Instance.IsGameStarted();

        float minY = started ? minLegY_After : minLegY_Before;
        float maxY = started ? maxLegY_After : maxLegY_Before;

        Vector3 leftPos = leftLeg.position;
        Vector3 rightPos = rightLeg.position;

        float targetLeftY = Mathf.Clamp(leftPos.y, minY, maxY);
        float targetRightY = Mathf.Clamp(rightPos.y, minY, maxY);

        leftPos.y = Mathf.Lerp(leftPos.y, targetLeftY, loosenRatio);
        rightPos.y = Mathf.Lerp(rightPos.y, targetRightY, loosenRatio);

        leftLeg.position = leftPos;
        rightLeg.position = rightPos;
    }

    void ApplyHeadYLimit()
    {
        if (body == null || head == null) return;

        float bodyAngle = body.eulerAngles.z;
        if (bodyAngle > 180f) bodyAngle -= 360f;

        float absAngle = Mathf.Abs(bodyAngle);
        float minY = headMinY;
        float maxY = headMaxY;

        if (absAngle > maxBodyAngle && head.position.y < maxY)
        {
            Vector3 headPos = head.position;
            headPos.y = minY;
            head.position = headPos;
        }
    }


    public void StartWalking()
    {
        if (inputLocked) return;
        isWalking = true;
    }

    public void StopWalking()
    {
        isWalking = false;
    }

    public void LockInput()
    {
        inputLocked = true;
        StopWalking();
    }

    public void ResetDuck()
    {
        isWalking = false;
        inputLocked = false;
        legAnimationTime = 0f;

        leftLeg.rotation = Quaternion.identity;
        rightLeg.rotation = Quaternion.identity;

        fixedX = transform.position.x;
        ChangeFace(false);
    }

    public void ChangeFace(bool isGameOver)
    {
        if (faceRenderer != null)
        {
            faceRenderer.sprite = isGameOver ? gameOverFace : normalFace;
        }
    }
}
