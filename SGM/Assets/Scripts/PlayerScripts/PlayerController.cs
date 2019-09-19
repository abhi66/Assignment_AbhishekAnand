using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public PlayerDirection direction;
  //  [HideInInspector]
 //   public float step_length = 18f;
    [HideInInspector]
    public float movement_frequency = .1f;

    float counter;
    bool move;

    [SerializeField]
    GameObject tailPrefab;
    List<Vector3> deltaPosition;
    List<Rigidbody> nodes;
    Rigidbody main_body;
    Rigidbody head_body;
    Transform tr;
    bool create_node_at_tail;
    [HideInInspector]
   public bool playerDead;
    [HideInInspector]
    public static PlayerController instance;

    void Awake()
    {
        playerDead = true;
        if (instance == null)
            instance = this;

            tr = transform;
        main_body = GetComponent<Rigidbody>();
        InitSnakeNodes();
        InitPlayer();

        deltaPosition = new List<Vector3>()
        {
            new Vector3(-.8f,0f,0f), //-X Left
            new Vector3(0f,0f,.8f), //z UP
            new Vector3(.8f,0f,0f), //X Right
            new Vector3(0f,0f,-.8f) //-z Down
        };
     //   Debug.Log(deltaPosition[1]);
    }
    void Update()
    {
        CheckMovementFrequency();
    }
    void FixedUpdate()
    {
        if (move)
        {
            if (!playerDead) {
                move = false;
                Move();
            }
        }
    }
    void InitSnakeNodes()
    {
        nodes = new List<Rigidbody>();
        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());
        head_body = nodes[0];
    }
    void SetDirectionRandom()
    {
        int dirRandom = Random.Range(0, (int)PlayerDirection.COUNT);
        direction = (PlayerDirection)dirRandom;
    }
    void InitPlayer()
    {
        SetDirectionRandom();
        switch (direction)
        {
            case PlayerDirection.RIGHT:
                nodes[1].position = nodes[0].position - new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.NODE * 2f, 0f, 0f);
                break;
            case PlayerDirection.LEFT:
                nodes[1].position = nodes[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.NODE * 2f, 0f, 0f);
                break;
            case PlayerDirection.UP:
                nodes[1].position = nodes[0].position - new Vector3(0f, 0f, Metrics.NODE);
                nodes[2].position = nodes[0].position - new Vector3(0f, 0f, Metrics.NODE * 2f);
                break;
            case PlayerDirection.DOWN:
                nodes[1].position = nodes[0].position + new Vector3(0f, 0f, Metrics.NODE);
                nodes[2].position = nodes[0].position + new Vector3(0f, 0f, Metrics.NODE * 2f);
                break;
        }
    }
    void Move()
    {
        Vector3 dPosition = deltaPosition[(int)direction];
        Vector3 presentPos = head_body.position;
        Vector3 prevPosition;

        main_body.position = main_body.position + dPosition;
        head_body.position = head_body.position + dPosition;


        for (int i = 1; i < nodes.Count; i++)
        {
            prevPosition = nodes[i].position;
            nodes[i].position = presentPos;
            presentPos = prevPosition;
        }
        //create new node if eate fruit
        if (create_node_at_tail)
        {
            create_node_at_tail = false;
            GameObject newNode = Instantiate(tailPrefab, nodes[nodes.Count -1].position, Quaternion.identity);
            newNode.transform.SetParent(transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }
    void CheckMovementFrequency()
    {
        counter += Time.deltaTime;
        if (counter >= movement_frequency)
        {
            counter = 0f;
            move = true;
        }
    }
    public void SetInputDirection(PlayerDirection dir)
    {
        if (dir == PlayerDirection.UP && direction == PlayerDirection.DOWN ||
            dir == PlayerDirection.DOWN && direction == PlayerDirection.UP ||
            dir == PlayerDirection.RIGHT && direction == PlayerDirection.LEFT ||
            dir == PlayerDirection.LEFT && direction == PlayerDirection.RIGHT)
            return;
        direction = dir;
        ForceMove();
    }
    void ForceMove()
    {
        counter = 0;
        move = false;
        Move();
    }
    void OnTriggerEnter(Collider other)
    {
       if (other.tag==Tags.WALL || other.tag == Tags.BOMB || other.tag == Tags.TAIL)
        {
            //Time.timeScale = 0f;
            playerDead = true;
            UIManager.instance.GameOver();

            PlayerPref.instance.SaveLastScore(GamePlayController.instance.scoreCount);

            if (PlayerPref.instance.GetHighScore() < GamePlayController.instance.scoreCount)
            PlayerPref.instance.SaveHighScore(GamePlayController.instance.scoreCount);

        }
        if (other.tag == Tags.FRUIT)
        {
            PickUpsEatable upsEatable = other.GetComponent<PickUpsEatable>();
            
            if (lastFood.Equals(upsEatable.name))
            {
                create_node_at_tail = true;
            }
            else
            {
                //if new food eated, remove tail
                if (nodes.Count > 3)
                {
                    for (int i = nodes.Count-1; i > 2; i--)
                    {
                        GameObject extraNode = nodes[i].gameObject;
                        nodes.Remove(nodes[i]);
                        Destroy(extraNode);
                    }
                }
            }
            lastFood = upsEatable.name;
            //add or remove tail
                      
            
            GamePlayController.instance.IncreaseScore(upsEatable.point);

            // other.gameObject.SetActive(false);
            Destroy(other.gameObject);


        }
    }
    string lastFood ="";
}
