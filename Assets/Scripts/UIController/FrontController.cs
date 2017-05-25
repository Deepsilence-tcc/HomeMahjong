using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;

public class FrontController : MonoBehaviour {

    public delegate void saveScoreRecordDelegate();

    public TopUI topUI;
    public GameObject Pause;
    public GameObject Talk;
    public GameObject Finish;
    public GameObject Loading;
    public int cardCount;
    public int totalCount;
    public int step;
    Text Pause_lv;
    Text Finish_lv;
    Text Left_time;
    Text Cave_floor;
    public Image bg;

    LevelTalks levelTalks;
    TalkController talkcontroller;
    float betTimer = -1;
    int betCount = 1;
    int score = 0;
    bool isBetAdding = false;

    const float deltaBetBar = 0.001f;
    const float caveTime = 600; //s

	void Awake () {

        CommonData.isSuccessLv = false;

        levelTalks = new LevelTalks();
        talkcontroller = Talk.GetComponent<TalkController>();
        Pause.transform.localPosition = new Vector3(CommonData.BASE_WIDTH, 0, 0);
        Finish.transform.localPosition = new Vector3(CommonData.BASE_WIDTH, 0, 0);
        Pause_lv = Pause.transform.Find("Text_level").GetComponent<Text>();
        Finish_lv = Finish.transform.Find("Text_finish").GetComponent<Text>();
        Cave_floor = Loading.transform.Find("Text_floor").GetComponent<Text>();
        Left_time = Loading.transform.Find("Left_time").GetComponent<Text>();

    }

    void Start() {
        if (CommonData.currentLvType == LEVEL_TYPE.cave)
        {
            Cave_floor.text = "floor" + CommonData.caveLV + "/3";
            Loading.transform.localPosition = new Vector3(0, 0, 0);
            TimeSpan time = new TimeSpan(0, 0, (int)CommonData.deltaTime);
            if (CommonData.deltaTime == -1)
                time = new TimeSpan(0, 0, (int)caveTime);
            Left_time.text = time.ToString();
            topUI.updateTime(time.ToString());
        }
        else
        {
            topUI.updateScore(score);
            Loading.transform.localPosition = new Vector3(CommonData.BASE_WIDTH, 0, 0);
        }
    }
	
	void Update () {
        checkClick();
        checkBet();
        checkTime();
    }

    void checkBet() {
        if (betTimer > 0)
        {
            if (betTimer > 1)
            {
                isBetAdding = true;
                betTimer = 1;
                betCount++;
                topUI.updateBet(betCount);
            }
            betTimer = betTimer - deltaBetBar;
            topUI.updateBetBar(1 - betTimer);
        }
        else if(betTimer != -1){
            betTimer = -1;
            isBetAdding = false;
            betCount = 1;
            topUI.updateBet(betCount);
            topUI.updateBetBar(0);
        }
    }

    void checkTime() {
        if (Loading.transform.localPosition.x != 0)
        {
            if (CommonData.deltaTime > 0)
            {
                CommonData.deltaTime = CommonData.deltaTime - Time.deltaTime;
                TimeSpan time = new TimeSpan(0, 0, (int)CommonData.deltaTime);
                //Debug.Log(time.ToString());
                topUI.updateTime(time.ToString());
            }
            else if (CommonData.deltaTime != -1)
            {
                CommonData.deltaTime = -1;
            }
        }
    }

    public void checkClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (Talk.transform.localPosition.x == 0)
            {
                hideTalks();
            }
            else if (Loading.transform.localPosition.x == 0) {
                Loading.transform.localPosition = new Vector3(CommonData.BASE_WIDTH, 0, 0);
            }
        }
    }

    public void showTalks() {
        bg.gameObject.SetActive(false);
        Talk.transform.localPosition = new Vector3(CommonData.BASE_WIDTH, 0, 0);
        TalkBorder talkborder = levelTalks.getCurrentTalkBorder();
        if (talkborder != null)
        {
            bg.gameObject.SetActive(true);
            Talk.transform.DOLocalMoveX(0, 0.5f)
                .SetEase(Ease.InQuint)
                .SetAutoKill(true)
                .SetUpdate(true);
            bg.CrossFadeAlpha(143f/255, 0.5f, true);
            talkcontroller.initBorder(talkborder);
        }
    }

    void hideTalks() {
        Talk.transform.DOLocalMoveX(-CommonData.BASE_WIDTH, 0.5f)
            .SetEase(Ease.InQuint)
            .SetAutoKill(true)
            .OnComplete(showTalks)
            .SetUpdate(true);
        bg.CrossFadeAlpha(0, 0.5f, true);
    }

    public void initCard(int count)
    {
        cardCount = count;
        totalCount = count;
        step = count / 2;
        topUI.updateStep(cardCount);
    }

    public void modifyCard()
    {
        switch (CommonData.currentLvType)
        {
            case LEVEL_TYPE.cave:
                modifyCaveCard();
                break;
            case LEVEL_TYPE.level:
                modifyLevelCard();
                break;
        }
    }

    void modifyLevelCard() {
        if (cardCount > 0)
        {
            // bet时间线性减少，每点击一次增加部分bet时间，如果bet时间超过2，则点击无效，时间只能减少不能累加，但是此时的分数倍数累加。
            if (betTimer == -1)
            {
                betTimer = 0;
            }
            score = score + 10 * betCount;
            topUI.updateScore(score);

            if (isBetAdding)
            {
                betCount++;
                topUI.updateBet(betCount);
            }
            else
            {
                betTimer = betTimer + 0.45f;
            }

            if (cardCount == totalCount)
            {
                // 走了第一步，用于判断是否减少体力
                int heart_count = PlayerPrefs.GetInt(CommonData.POWER, CommonData.HEART_MAX);
                heart_count--;
                PlayerPrefs.SetInt(CommonData.POWER, heart_count);

                string timeStr = PlayerPrefs.GetString(CommonData.AIM_TIME, "");
                DateTime aimTime = System.DateTime.Now.AddMinutes(CommonData.HEART_COOLING_TIME);
                if (timeStr != "")
                {
                    DateTime curAimTime = Convert.ToDateTime(timeStr);
                    aimTime = curAimTime.AddMinutes(CommonData.HEART_COOLING_TIME);
                }
                PlayerPrefs.SetString(CommonData.AIM_TIME, aimTime.ToString());
            }

            cardCount = cardCount - 2;
            step = step - 1;
            topUI.updateStep(cardCount);
        }

        if (step == 0)
        {
            endLevel();
        }
    }

    void modifyCaveCard()
    {
        if (cardCount > 0)
        {
            //动态增长进度条
            cardCount = cardCount - 2;
            step = step - 1;
            topUI.updateStep(cardCount);

            if (CommonData.deltaTime == -1)
            {
                CommonData.deltaTime = caveTime;
            }
        }

        if (step == 0)
        {
            endCave();
        }
    }

    void endCave() {
        if (CommonData.caveLV < 3)
        {
            CommonData.caveLV = CommonData.caveLV + 1;
            SceneManager.LoadScene("Cave_1_" + CommonData.caveLV);
        }
        else
        {
            CommonData.deltaTime = -1;
            CommonData.caveLV = 0;
            Finish_lv.text = "Cave " + " Finished";
            Finish.transform.DOLocalMoveX(0, 0.5f)
                .SetEase(Ease.InQuint)
                .SetAutoKill(true)
                .SetUpdate(true);
        }
    }

    void endLevel()
    {
        saveScoreRecordDelegate dele = new saveScoreRecordDelegate(saveScoreRecord);
        dele.Invoke();

        int bestlevel = PlayerPrefs.GetInt(CommonData.BEST_LEVEL, 0);
        if (bestlevel < CommonData.currentLV)
        {
            PlayerPrefs.SetInt(CommonData.BEST_LEVEL, CommonData.currentLV);
            CommonData.isSuccessLv = true;
        }

        Finish_lv.text = "Level " + CommonData.currentLV + " Finished";
        Finish.transform.DOLocalMoveX(0, 0.5f)
            .SetEase(Ease.InQuint)
            .SetAutoKill(true)
            .SetUpdate(true);
    }

    public void saveScoreRecord()
    {
        DataBaseService service = DataBaseService.GetInstance();
        var scores = service.GetDefaultScore(CommonData.currentLV);
        List<DefaultScore> scorelist = scores.ToList<DefaultScore>();
        int rank = 1;
        for (int i = 0; i < scorelist.Count; i++) {
            if (scorelist[i].score < score) {
                rank = scorelist[i].rank;
                break;
            }
        }

        int star = 1;
        if (score > topUI.scoredata.star3)
            star = 3;
        else if (score > topUI.scoredata.star2)
            star = 2;
        // 写入最好成绩
        ScoreRecord scoreRecord = service.getScoreRecordByLv(CommonData.currentLV);
        ScoreRecord scoreRecord2 = new ScoreRecord(CommonData.currentLV, rank, score, star);
        if (scoreRecord == null)
        {
            service.insertData(scoreRecord2);
        }
        else if (scoreRecord.score < score)
        {
            service.updateData(scoreRecord2);
        }

    }

    public void onPauseClick() {
        Time.timeScale = 0;
        Pause_lv.text = "Level " + CommonData.currentLV;
        Pause.transform.DOLocalMoveX(0, 0.5f)
            .SetEase(Ease.InQuint)
            .SetAutoKill(true)
            .SetUpdate(true);
    }

    ////////////// Pause 
    public void onPlayClick()
    {
        Pause.transform.DOLocalMoveX(-CommonData.BASE_WIDTH, 0.5f)
            .SetEase(Ease.InQuint)
            .SetAutoKill(true)
            .OnComplete(initPos)
            .SetUpdate(true);
    }
    private void initPos()
    {
        Pause.transform.localPosition = new Vector3(CommonData.BASE_WIDTH, 0, 0);
        Time.timeScale = 1;
    }

    private void refresh()
    {
        Pause.transform.localPosition = new Vector3(CommonData.BASE_WIDTH, 0, 0);
        Time.timeScale = 1;
        SceneManager.LoadScene("Level" + CommonData.currentLV);
    }

    public void onRefreshClick()
    {
        Pause.transform.DOLocalMoveX(-CommonData.BASE_WIDTH, 0.5f)
            .SetEase(Ease.InQuint)
            .SetAutoKill(true)
            .OnComplete(refresh)
            .SetUpdate(true);
    }

    public void onHomeClick()
    {
        SceneManager.LoadScene("MainScene");
    }
}
