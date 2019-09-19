using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject startUI;
    public GameObject gameOverT;
    public static UIManager instance;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void StartButton()
    {
        startUI.SetActive(false);
        //  Debug.Log("clicked");
        PlayerController.instance.playerDead = false;
    }

    public void GameOver()
    {
        gameOverT.SetActive(true);
        Invoke("RestartLevel", 2f);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
