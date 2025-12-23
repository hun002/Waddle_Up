using UnityEngine;

public class HeadCollisionChecker : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground") && GameManager.Instance.IsGameStarted() && !GameManager.Instance.IsGameOver())
        {
            Debug.Log("게임 오버");
            GameManager.Instance.TriggerGameOver("오리가 넘어짐");
        }
    }
}
