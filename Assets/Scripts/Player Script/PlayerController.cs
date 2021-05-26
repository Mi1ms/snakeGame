using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public PlayerDirection direction;

    [HideInInspector]
    public float step_length = 0.2f;

    [HideInInspector]
    public float movement_frequency = 0.1f;

    private float counter;
    private bool move;

    [SerializeField]
    private GameObject tailPrefab;

    private List<Vector3> delta_position;

    private List<Rigidbody> nodes;

    private Rigidbody main_body;
    private Rigidbody head_body;
    private Transform tr;

    private bool create_node_at_tail;

    void Awake() {
        tr = transform;
        main_body = GetComponent<Rigidbody>(); 

        InitSnakeNode();
        InitPlayer();

        delta_position = new List<Vector3>() {
            new Vector3(-step_length, 0f), // -dx -> left
            new Vector3(0f, step_length), // dy -> up
            new Vector3(step_length, 0f), // dx -> right
            new Vector3(0f, -step_length) // -dy -> down
        };
    }

    void Update() {
        CheckMovementFrequency();
    }

    void FixedUpdate() {
        if(move) {
            move = false;
            Move();
        }
    }

    void InitSnakeNode() {
        nodes = new List<Rigidbody>();

        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());

        head_body = nodes[0];
    }

    void SetDirectionRandom() {
        int dirRandom = Random.Range(0, (int) PlayerDirection.COUNT);
        direction = (PlayerDirection) dirRandom;
    }

    void InitPlayer() {
        SetDirectionRandom();
        switch (direction)
        {
            case PlayerDirection.RIGHT:
                nodes[1].position = nodes[0].position - new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.NODE * 2, 0f, 0f);
                break;
            case PlayerDirection.LEFT:
                nodes[1].position = nodes[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.NODE * 2, 0f, 0f);
                break;
            case PlayerDirection.UP:
                nodes[1].position = nodes[0].position - new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position - new Vector3(0f, Metrics.NODE * 2, 0f);
                break;
            case PlayerDirection.DOWN:
                nodes[1].position = nodes[0].position + new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position + new Vector3(0f, Metrics.NODE * 2, 0f);
                break;
        }
    }

    void Move() {
        Vector3 dPosition = delta_position[(int) direction];
        Vector3 parenPos = head_body.position;
        Vector3 prevPos;

        main_body.position = main_body.position + dPosition;
        head_body.position = head_body.position + dPosition;
        
        for(int i = 1; i < nodes.Count; i++) {
            prevPos = nodes[i].position;
            nodes[i].position = parenPos;
            parenPos = prevPos;
        }

        if(create_node_at_tail) {
            create_node_at_tail = false;
            GameObject newNode = Instantiate(tailPrefab, nodes[nodes.Count - 1].position, Quaternion.identity);
            newNode.transform.SetParent(transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }

    void CheckMovementFrequency() {
        counter += Time.deltaTime;

        if(counter >= movement_frequency) {
            counter = 0f;
            move = true;
        }
    }

    public void SetInputDirection(PlayerDirection dir) {
        if(dir == PlayerDirection.UP && direction == PlayerDirection.DOWN ||
            dir == PlayerDirection.DOWN && direction == PlayerDirection.UP ||
            dir == PlayerDirection.RIGHT && direction == PlayerDirection.LEFT ||
            dir == PlayerDirection.LEFT && direction == PlayerDirection.RIGHT) {
                return;
        }

        direction = dir;

        ForceMove();
    }

    void ForceMove() {
        counter = 0;
        move = false;
        Move();
    }

    void OnTriggerEnter(Collider target) {
        if(target.tag == Tags.FRUIT) {
            target.gameObject.SetActive(false);

            create_node_at_tail = true;

            GameplayController.instance.IncreaseScore();
            AudioManager.instance.Play_PickUpSound();
        }

        if(target.tag == Tags.WALL || target.tag == Tags.BOMB || target.tag == Tags.TAIL) {
            Time.timeScale = 0f;
            AudioManager.instance.Play_DeathSound();
        }
    }
}
