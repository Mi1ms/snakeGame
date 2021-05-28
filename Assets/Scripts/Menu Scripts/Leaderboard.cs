using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    private Transform container;
    private Transform template;
    void Awake()
    {
        container = transform.Find("ScoreContainer");
        template = container.Find("ScoreTemplate");

        template.gameObject.SetActive(false);

        LeaderboardModel[] leaderboards = Api.GetLeaderboards();

        float templateHeight = 30f;
        for (int i = 0; i < leaderboards.GetLength(0); i++)
        {
            Transform entryTranform = Instantiate(template, container);
            RectTransform entryRectTransform = entryTranform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entryRectTransform.gameObject.SetActive(true);

            int rank = i + 1;

            string rankString;

            switch (rank)
            {
                case 1:
                    rankString = "1ST";
                    break;
                case 2:
                    rankString = "2ND";
                break;
                case 3:
                    rankString = "3RD";
                    break;
                default:
                    rankString = rank + "TH";
                    break;
            }

            entryTranform.Find("Pos").GetComponent<Text>().text = rankString;
            entryTranform.Find("Name").GetComponent<Text>().text = leaderboards[i].name;
            entryTranform.Find("Score").GetComponent<Text>().text = leaderboards[i].score.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
