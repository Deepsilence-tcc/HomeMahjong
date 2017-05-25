using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class MainFrontController : MonoBehaviour {

    const string MAX = "MAX";
    public Text Text_coin;
    public Text Text_heart;
    public Text Text_timer;
    public Text Text_level;
    public GameObject Settings;
    public GameObject PlayBoard;
    public GameObject DialBoard;
    public Transform MapLV;
    public GameObject figureIcon;

    DateTime aimTime;
    string timeStr;
    int currentHeart = 0;
    Vector3 movePos = new Vector3(0, 140, 0);

    GameLevel[] gamelevels = new GameLevel[] { };

    void Start () {
        //PlayerPrefs.SetString(CommonData.AIM_TIME, "");
        //PlayerPrefs.SetInt(CommonData.POWER, CommonData.HEART_MAX);
        //PlayerPrefs.SetInt(CommonData.BEST_LEVEL, 0);

        currentHeart = PlayerPrefs.GetInt(CommonData.POWER, CommonData.HEART_MAX);
        Text_heart.text = "" + currentHeart;
        initPos();

        timeStr = PlayerPrefs.GetString(CommonData.AIM_TIME, "");

        gamelevels = MapLV.gameObject.GetComponentsInChildren<GameLevel>();
        initFigurePos();

    }
	
	void Update () {
        if (timeStr != null && timeStr != "")
        {
            aimTime = Convert.ToDateTime(timeStr);
            compareHeartTime();
        }
        else {
            Text_timer.text = MAX;
        }
	}

    void initFigurePos() {
        int bestlevel = PlayerPrefs.GetInt(CommonData.BEST_LEVEL, 0);
        int lvCount = MapLV.childCount;
        Vector3 pos;
        if (lvCount > bestlevel)
            pos = MapLV.GetChild(bestlevel).transform.localPosition;
        else
            pos = MapLV.GetChild(lvCount - 1).transform.localPosition;

        if (CommonData.isSuccessLv)
        {
            // 闯关成功，显示星星，移动头像。
            int prelevel = bestlevel - 1;
            Vector3 pos2;
            if (lvCount > prelevel)
                pos2 = MapLV.GetChild(prelevel).transform.localPosition;
            else
                pos2 = MapLV.GetChild(lvCount - 1).transform.localPosition;

            CommonData.isSuccessLv = false;

            figureIcon.transform.localPosition = pos2 + movePos;
            figureIcon.transform.DOLocalMove(pos + movePos, 0.5f)
                .SetEase(Ease.Linear)
                .SetAutoKill(true)
                .SetUpdate(true);

        }
        else
        {
            figureIcon.transform.localPosition = pos + movePos;
        }

        var s = DataBaseService.GetInstance().GetScoreRecord();
        List<ScoreRecord> scoreRecords = s.ToList<ScoreRecord>();
        for (int i = 0; i < scoreRecords.Count; i++) {
            gamelevels[i].showStar(scoreRecords[i].star);
        }
    }

    void compareHeartTime()
    {
        DateTime dt = System.DateTime.Now;
        if (dt.CompareTo(aimTime) >= 0)
        {
            Debug.Log("所有体力时间都恢复了 ");
            // 所有体力时间都恢复了
            PlayerPrefs.SetInt(CommonData.POWER, CommonData.HEART_MAX);
            Text_heart.text = "" + CommonData.HEART_MAX;
            PlayerPrefs.SetString(CommonData.AIM_TIME, "");
            Text_timer.text = MAX;
            timeStr = "";
        }
        else
        {
            TimeSpan span1 = aimTime.Subtract(dt);
            int m = (int)(span1.TotalSeconds / (CommonData.HEART_COOLING_TIME * 60));
            int n = (int)(span1.TotalSeconds - m * CommonData.HEART_COOLING_TIME * 60);

            int heart_count1;
            if(n > 0)
                heart_count1 = CommonData.HEART_MAX - m - 1;
            else
                heart_count1 = CommonData.HEART_MAX - m;

            if (currentHeart != heart_count1)
            {
                currentHeart = heart_count1;
                PlayerPrefs.SetInt(CommonData.POWER, currentHeart);
            }
            Text_heart.text = "" + heart_count1;
            TimeSpan span2 = aimTime.Subtract(dt.AddMinutes(CommonData.HEART_COOLING_TIME * m));
            Text_timer.GetComponent<Text>().text = span2.ToString().Substring(3, 5);
        }
    }

    public void onFacebookClick()
    {
    }

    public void onGameCenterClick()
    {
    }

    public void onLevelClick(int level)
    {
        Debug.Log("onLevelClick:" + level);
        PlayBoard.GetComponent<PlayBoardController>().init(level, LEVEL_TYPE.level);
    }

    public void onCaveClick(int level)
    {
        PlayBoard.GetComponent<PlayBoardController>().init(level, LEVEL_TYPE.cave);
    }

    private void initPos()
    {
        Settings.transform.localPosition = new Vector3(CommonData.BASE_WIDTH, 0, 0);
        PlayBoard.transform.localPosition = new Vector3(CommonData.BASE_WIDTH, 0, 0);
        DialBoard.transform.localPosition = new Vector3(CommonData.BASE_WIDTH, 0, 0);
    }

    public void onSettingClick()
    {
        Settings.transform.DOLocalMoveX(0, 0.5f)
            .SetEase(Ease.InQuint)
            .SetAutoKill(true)
            .SetUpdate(true);
    }

    public void onSettingClose()
    {
        Settings.transform.DOLocalMoveX(- CommonData.BASE_WIDTH, 0.5f)
            .SetEase(Ease.InQuint)
            .OnComplete(initPos)
            .SetAutoKill(true)
            .SetUpdate(true);
    }

    public void onDialClick()
    {
        DialBoard.transform.DOLocalMoveX(0, 0.5f)
            .SetEase(Ease.InQuint)
            .SetAutoKill(true)
            .SetUpdate(true);
    }

    public void onDialClose()
    {
        DialBoard.transform.DOLocalMoveX(-CommonData.BASE_WIDTH, 0.5f)
            .SetEase(Ease.InQuint)
            .OnComplete(initPos)
            .SetAutoKill(true)
            .SetUpdate(true);
    }
}
