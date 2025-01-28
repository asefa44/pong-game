using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public ScoreText scoreTextLeft, scoreTextRight;
    public GameObject menuObject;
    public GameObject pauseMenuObject;
    public Action OnStartGame;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI volumeValueText;
    public TextMeshProUGUI playModeButtonText;

    private bool isPaused = false;
    private void Awake()
    {
        AdjustPlayModeButtonText();
        pauseMenuObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else PauseGame();
        }
    }
    public void UpdateScores(int scoreP1, int scoreP2)
    {
        scoreTextLeft.SetScore(scoreP1);
        scoreTextRight.SetScore(scoreP2);
    }

    public void HighlightScore(int id)
    {
        if (id == 1) scoreTextLeft.Highlight();
        if (id == 2) scoreTextRight.Highlight();
    }

    public void OnStartGameButtonClicked()
    {
        menuObject.SetActive(false);
        OnStartGame?.Invoke();
        GameManager.instance.gameAudio.PlayBackgroundMusic();
    }

    public void OnGameEnds(int winnerId)
    {     
        winnerText.text = $"Player {winnerId} wins!";
        menuObject.SetActive(true);  
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        volumeValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
    }

    public void OnSwitchPlayModeClicked()
    {
        GameManager.instance.SwitchPlayMode();
        AdjustPlayModeButtonText();
    }

    public void OnResumeButtonClicked()
    {
        ResumeGame();
    }
    public void OnExitGameButtonClicked()
    {
        Application.Quit();
    }

    private void AdjustPlayModeButtonText()
    {
        switch (GameManager.instance.playMode)
        {
            case GameManager.PlayMode.PlayerVsPlayer:
                playModeButtonText.text = "2 Players";
                break;

            case GameManager.PlayMode.PlayerVsAI:
                playModeButtonText.text = "Player vs AI";
                break;
        }
    }
    private void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;

        pauseMenuObject.SetActive(true);
    }
    private void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;

        pauseMenuObject.SetActive(false);
    }
}
