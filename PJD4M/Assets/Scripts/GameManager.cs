using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void LoadNextLevel()
    {
        int nextLevelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevelToLoad < SceneManager.sceneCount)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
}
