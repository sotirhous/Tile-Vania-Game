using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{

    [SerializeField] int playerLives = 3;
    [SerializeField] int coin = 0;

    [SerializeField] Text livesText;
    [SerializeField] Text coinText;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        livesText.text = "Lives: " + playerLives.ToString();
        coinText.text = "Coins: " + coin.ToString();
    }

    public void AddToCoins(int pointsToAdd)
    {
        coin += pointsToAdd;
        coinText.text = "Coins: " + coin.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void TakeLife()
    {
        playerLives --;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = "Lives: " + playerLives.ToString();
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
