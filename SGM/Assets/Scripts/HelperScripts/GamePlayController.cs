using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController instance;
    float minX =-21f , maxX = 20, minZ = -5, maxZ = 8;
    float yPos = 0;
    Text scoreText;
    [HideInInspector]
   public int scoreCount;
    public List<PickUpsEatable> pickUpsEatables;
    public GameObject bomb;
    public Text hScoreT;
    
    void Awake()
    {
        CreateInstance();
    }
    void Start()
    {
        
        hScoreT.text= "High Score: " + PlayerPref.instance.GetHighScore();
        scoreText = GameObject.Find("ScoreT").GetComponent<Text>();
        Invoke("StartSpawning",0f);
    }

    void CreateInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void StartSpawning()
    {
        StartCoroutine(SpawnPickUps());
    }
    public void CancleSpawning()
    {
        CancelInvoke("StartSpawning");
        Debug.Log("spawn cancelled");
    }
    IEnumerator SpawnPickUps()
    {
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));
        if (!PlayerController.instance.playerDead)
            if (Random.Range(0, 12) >= 2)
        {
            Instantiate(pickUpsEatables[Random.Range(0,pickUpsEatables.Count-1)], new Vector3(Random.Range(minX, maxX),
                yPos, Random.Range(minZ, maxZ)), Quaternion.identity);
        }
        else
        {
            Instantiate(bomb, new Vector3(Random.Range(minX, maxX),
                            yPos, Random.Range(minZ, maxZ)), Quaternion.identity);
        }
       
            Invoke("StartSpawning", 0f);
        
    }
    public void IncreaseScore(int score)
    {
        scoreCount += score;
        scoreText.text = "Score: " + scoreCount;
    }
}
