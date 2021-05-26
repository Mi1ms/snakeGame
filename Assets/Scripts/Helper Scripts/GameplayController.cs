using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    public GameObject fruit_pickup, bomb_pickup;

    private float min_x = -4.25f, max_x = 4.25f, min_y = -2.26f, max_y = 2.26f;
    private float z_pos = 5.8f;

    private Text score_text;
    private int scoreCount;

    void Start()
    {
        score_text = GameObject.Find("Score").GetComponent<Text>();

        Invoke("StartSpawning", 0f);
    }

    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void StartSpawning() {
        StartCoroutine(SpawnPickUps());
    }

    public void CancelSpawnign() {
        CancelInvoke("StartSpawning");
    }

    IEnumerator SpawnPickUps()
    {
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));

        if (Random.Range(0, 10) >= 2)
        {
            Instantiate(fruit_pickup, new Vector3(Random.Range(min_x, max_x), Random.Range(min_y, max_y), z_pos), Quaternion.identity);
        } else {
            Instantiate(bomb_pickup, new Vector3(Random.Range(min_x, max_x), Random.Range(min_y, max_y), z_pos), Quaternion.identity);
        }

        Invoke("StartSpawning", 0f);
    }

    public void IncreaseScore() {
        scoreCount++;
        score_text.text = "Score: " + scoreCount;
    }
}
