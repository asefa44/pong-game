using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameUI gameUI;
    public GameAudio gameAudio;
    public Shake screenShake;
    public Ball ball;
    public ParticleSystem winnerParticle;
    
    
    public int scoreP1, scoreP2;
    private int maxScore = 4;

    public Action OnReset;
    public enum PlayMode
    {
        PlayerVsPlayer,
        PlayerVsAI
    }
    public PlayMode playMode;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            gameUI.OnStartGame += OnStartGame;
        }
    }
    public void OnScoreZoneReached(int id)
    {       
        if (id == 1) scoreP1++;
        if (id == 2) scoreP2++;

        gameUI.UpdateScores(scoreP1, scoreP2);
        gameUI.HighlightScore(id);
        gameAudio.PlayScoreSound();
        CheckWin();
    }

    private void CheckWin()
    {
        int winnerId = scoreP1 == maxScore ? 1 : scoreP2 == maxScore ? 2 : 0;

        if(winnerId != 0)
        {
            gameUI.OnGameEnds(winnerId);
            gameAudio.StopBackgroundMusic();
            gameAudio.PlayWinnerSound();
            winnerParticle.Emit(300);
        }
        else
        {
            OnReset?.Invoke();
        }
    }
    private void OnStartGame()
    {
        scoreP1 = 0;
        scoreP2 = 0;
        gameUI.UpdateScores(scoreP1, scoreP2);
    }

    public void SwitchPlayMode()
    {
        switch(playMode)
        {
            case PlayMode.PlayerVsPlayer:
                playMode = PlayMode.PlayerVsAI;
                break;

            case PlayMode.PlayerVsAI:
                playMode = PlayMode.PlayerVsPlayer;
                break;
        }
    }

    public bool IsPlayerVsAi()
    {
        return playMode == PlayMode.PlayerVsAI;
    }
}
