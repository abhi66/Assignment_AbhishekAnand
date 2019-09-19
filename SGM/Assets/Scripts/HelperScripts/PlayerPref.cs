using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPref : MonoBehaviour
{
    public static PlayerPref instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
   public void SaveLastScore(int score)
    {
        PlayerPrefs.SetInt("Score", score);
    }
    public int GetLastScore()
    {
       return  PlayerPrefs.GetInt("Score", 0);
    }

    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt("HScore", score);
    }
    public int GetHighScore()
    {
       return PlayerPrefs.GetInt("HScore", 0);
    }
}
