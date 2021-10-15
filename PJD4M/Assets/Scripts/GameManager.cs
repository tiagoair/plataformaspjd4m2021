using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int lives = 3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        HUDObserverManager.LivesChanged(lives);
    }

    private void Update()
    {
        if(lives < 0) ResetGame();
    }

    public void LoadNextLevel()
    {
        int nextLevelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevelToLoad < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevelToLoad);
        }
        //TODO: colocar um tratamento pra quando o jogador zerar o jogo
    }

    private void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    private void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    private void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CheckDeath()
    {
        lives--;
        HUDObserverManager.LivesChanged(lives);

        if (lives < 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            ResetLevel();
        }
    }

    private void ResetGame()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame &&
            SceneManager.GetActiveScene().name == "GameOver")
        {
            lives = 3;
            HUDObserverManager.LivesChanged(lives);
            LoadLevel1();
        }
    }
}
