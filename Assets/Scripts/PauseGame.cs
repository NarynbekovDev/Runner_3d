using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public static PauseGame Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void PausedGame()
    {
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
    }
}
