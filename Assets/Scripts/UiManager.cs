using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup WinConteiner, LoseConteiner, StartConteiner;
    [SerializeField] private Button StartGameButton, RestartGameButton, NextGameButton;
    [SerializeField] private PlayerRunner playerController;

    private void Start()
    {
        StartGameButton.onClick.AddListener(StartGame);
        RestartGameButton.onClick.AddListener(RestartGame);
        NextGameButton.onClick.AddListener(NextGame);
    }

    private void NextGame()
    {
        PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel", 0) + 1);
        SceneManager.LoadScene(0);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        playerController.StartGame();
        StartConteiner.blocksRaycasts = false;
        StartConteiner.alpha = 0;
    }

    public void WinGame()
    {
        WinConteiner.blocksRaycasts = true;
        WinConteiner.alpha = 1;
    }

    public void LoseGame()
    {
        LoseConteiner.blocksRaycasts = true;
        LoseConteiner.alpha = 1;
    }
}
