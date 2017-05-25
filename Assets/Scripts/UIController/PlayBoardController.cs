using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;
using System;

public class PlayBoardController : MonoBehaviour {

    List<DefaultScore> scorelist = new List<DefaultScore>();
    Text Text_level;
    Transform Panel;
    List<Transform> userborder;
    public delegate void updateScrollDelegate();

    public void updateScroll() {
        DataBaseService service = DataBaseService.GetInstance();
        var scores = service.GetDefaultScore(CommonData.currentLV);

        scorelist = scores.ToList<DefaultScore>();
        
        ScoreRecord scoreRecord = DataBaseService.GetInstance().getScoreRecordByLv(CommonData.currentLV);
        if (scoreRecord != null) {
            scorelist.Add(new DefaultScore(3, scoreRecord.rank, scoreRecord.score));
        }
        DefaultScore[] scoreArray = scorelist.ToArray();
        Array.Sort(scoreArray, new ScoreComparer());

        int delta = 0;
        for (int i = 0; i < scoreArray.Length; i++)
        {
            userborder[i].Find("Text_rank").GetComponent<Text>().text = (scoreArray[i].rank + delta) + "";
            userborder[i].Find("Text_score").GetComponent<Text>().text = scoreArray[i].score + "";
            userborder[i].Find("Image_pic").GetComponent<Image>().sprite = scoreArray[i].getSprite();
            if (scoreArray[i].npcid != 3)
            {
                userborder[i].Find("Text_name").GetComponent<Text>().text = scoreArray[i].getName();
            }
            else {
                delta = 1;
                userborder[i].Find("Text_name").GetComponent<Text>().text = "you";
            }
        }
    }

	void Awake () {
        Text_level = transform.Find("Text_level").GetComponent<Text>();
        Panel = transform.Find("UserScroll").Find("Panel");

        userborder = new List<Transform>();
        for (int i = 0; i < Panel.childCount; i++) {
            userborder.Add(Panel.GetChild(i));
        }
	}

    public void init(int level, LEVEL_TYPE type)
    {
        CommonData.currentLvType = type;
        switch (type)
        {
            case LEVEL_TYPE.level:
                CommonData.currentLV = level;
                Text_level.text = "Level " + level;
                break;
            case LEVEL_TYPE.cave:
                Text_level.text = "Cave " + level;
                break;
            default:
                break;
        }

        updateScrollDelegate d = new updateScrollDelegate(updateScroll);
        d.Invoke();

        transform.DOLocalMoveX(0, 0.5f)
            .SetEase(Ease.InQuint)
            .SetAutoKill(true)
            .SetUpdate(true);
    }

    public void onBoardClose() {
        transform.DOLocalMoveX(-CommonData.BASE_WIDTH, 0.5f)
            .SetEase(Ease.InQuint)
            .OnComplete(initPos)
            .SetAutoKill(true)
            .SetUpdate(true);
    }

    private void initPos()
    {
        transform.localPosition = new Vector3(CommonData.BASE_WIDTH, 0, 0);
    }

    public void onPlayClick()
    {
        transform.DOLocalMoveX(-CommonData.BASE_WIDTH, 0.5f)
            .SetEase(Ease.InQuint)
            .OnComplete(enterGame)
            .SetAutoKill(true)
            .SetUpdate(true);
    }

    private void enterGame()
    {
        transform.localPosition = new Vector3(CommonData.BASE_WIDTH, 0, 0);
        switch (CommonData.currentLvType)
        {
            case LEVEL_TYPE.level:
                SceneManager.LoadScene("Level" + CommonData.currentLV);
                break;
            case LEVEL_TYPE.cave:
                CommonData.caveLV = 1;
                CommonData.deltaTime = -1;
                SceneManager.LoadScene("Cave_1_1");
                break;
            default:
                break;
        }
    }
}
