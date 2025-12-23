using System.Collections;
using UnityEngine;

public class DuckBodyController : MonoBehaviour
{
    private float springVelocity = 0f;

    [Header("움직임 설정")]
    public float restoreForce = 2f;
    public float dampingForce = 0.8f;

    [Header("몸통 파츠")]
    public Transform body;

    [Header("몸통 각도 설정")]
    public float bodyAngleChangeInterval = 0.5f;
    public float maxBodyAngle = 30f;
    public float bodyRotationSmooth = 5f;

    private float targetBodyAngle = 0f;
    private float currentBodyAngle = 0f;
    private float displayBodyAngle = 0f;
    private Coroutine bodyAngleCoroutine;

    void Update()
    {
        if (GameManager.Instance.IsGameStarted() && !GameManager.Instance.IsGameOver())
        {
            HandleBodyControl();

            springVelocity += -currentBodyAngle * restoreForce * Time.deltaTime;
            springVelocity -= springVelocity * dampingForce * Time.deltaTime;
            currentBodyAngle += springVelocity * Time.deltaTime;
        }
    }

    void HandleBodyControl()
    {
        float targetAngle = currentBodyAngle + targetBodyAngle;
        displayBodyAngle = Mathf.Lerp(displayBodyAngle, targetAngle, bodyRotationSmooth * Time.deltaTime);
        body.rotation = Quaternion.Euler(0, 0, displayBodyAngle);
    }

    public void StartBodyAngleChange()
    {
        if (bodyAngleCoroutine == null)
        {
            bodyAngleCoroutine = StartCoroutine(ChangeBodyAngle());
        }
    }

    public void StopBodyAngleChange()
    {
        if (bodyAngleCoroutine != null)
        {
            StopCoroutine(bodyAngleCoroutine);
            bodyAngleCoroutine = null;
        }
    }

    IEnumerator ChangeBodyAngle()
    {
        while (GameManager.Instance.IsGameStarted() && !GameManager.Instance.IsGameOver())
        {
            yield return new WaitForSeconds(bodyAngleChangeInterval);
            targetBodyAngle = Random.Range(-maxBodyAngle, maxBodyAngle);
            Debug.Log($"몸통이 {targetBodyAngle:F1}도 기울어집니다!");
        }
    }

    // 🟢 Getter 추가
    public float GetTotalBodyAngle()
    {
        return displayBodyAngle;
    }
}
