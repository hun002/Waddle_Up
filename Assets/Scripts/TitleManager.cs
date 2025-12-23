using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("캔버스")]
    public GameObject targetCanvas;

    public void StartGame()
    {
        SceneManager.LoadScene("Main Game Scene");
    }

    public void OpenCanvas()
    {
        if (targetCanvas != null)
            targetCanvas.SetActive(true);
    }

    public void CloseCanvas()
    {
        if (targetCanvas != null)
            targetCanvas.SetActive(false);
    }
}
