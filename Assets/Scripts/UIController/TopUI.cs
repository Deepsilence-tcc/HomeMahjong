using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TopUI : MonoBehaviour {

    public delegate void updateStarDelegate();

    public GameObject Text_leftcount;
    Text Text_score;
    Image betBar;
    Text Text_bet;

    Transform CaveTop;
    Transform caveBg;
    Text Text_time;

    Transform levelTop;
    Transform scoreBarBg;
    Image scoreBar;
    Image star1;
    Image star2;
    Image star3;
    Sprite star_a;

    public LevelData scoredata;

    void Awake()
    {
        CaveTop = transform.Find("CaveTop");
        caveBg = CaveTop.Find("cavebarBg");
        Text_time = CaveTop.Find("Text_time").GetComponent<Text>();

        levelTop = transform.Find("levelTop");
        Text_score = levelTop.Find("Text_score").GetComponent<Text>();
        Text_bet = levelTop.Find("BetBorder").Find("Text_bet").GetComponent<Text>();
        betBar = levelTop.Find("BetBorder").Find("bet_Bar").GetComponent<Image>();
        scoreBarBg = levelTop.Find("scorebarBg");
        scoreBar = scoreBarBg.Find("scorebar").GetComponent<Image>();
        star1 = scoreBarBg.Find("star1").GetComponent<Image>();
        star2 = scoreBarBg.Find("star2").GetComponent<Image>();
        star3 = scoreBarBg.Find("star3").GetComponent<Image>();

        updateStarDelegate d = new updateStarDelegate(readScoreData);
        d.Invoke();

        string name = "Texture/Atlas/star";
        Texture2D texture1 = (Texture2D)Resources.Load(name);
        Rect rect = new Rect(0, 0, texture1.width, texture1.height);
        star_a = Sprite.Create(texture1, rect, new Vector2(0.5f, 0.5f));
    }

    void readScoreData()
    {
        if (CommonData.currentLvType == LEVEL_TYPE.cave)
        {
            CaveTop.localScale = Vector3.one;
            levelTop.localScale = Vector3.zero;
        }
        else
        {
            levelTop.localScale = Vector3.one;
            CaveTop.localScale = Vector3.zero;

            DataBaseService service = DataBaseService.GetInstance();
            var scores = service.GetLevelData(CommonData.currentLV);
            scoredata = scores.ToList<LevelData>()[0];

            //摆放星星位置
            float pos1 = scoredata.star1 * 1.0f / scoredata.max;
            float pos2 = scoredata.star2 * 1.0f / scoredata.max;
            float pos3 = scoredata.star3 * 1.0f / scoredata.max;

            float width = scoreBarBg.GetComponent<RectTransform>().rect.width;

            star1.transform.localPosition = new Vector3(0 - width / 2 + pos1 * width, 0, 0);
            star2.transform.localPosition = new Vector3(0 - width / 2 + pos2 * width, 0, 0);
            star3.transform.localPosition = new Vector3(0 - width / 2 + pos3 * width, 0, 0);
        }
    }
	
	void Update () {
		
	}

    public void updateScore(int score) {
        Text_score.text = score + "";
        float percent = score * 1.0f / scoredata.max;
        scoreBar.fillAmount = percent;

        if (score >= scoredata.star3) {
            star3.sprite = star_a;
        }
        else if (score >= scoredata.star2)
        {
            star2.sprite = star_a;
        }
        else if (score >= scoredata.star1)
        {
            star1.sprite = star_a;
        }
    }

    public void updateBetBar(float num) {
        betBar.fillAmount = num;
    }

    public void updateBet(int bet)
    {
        Text_bet.text = bet + "";
    }

    public void updateStep(int cardCount)
    {
        Text_leftcount.GetComponent<Text>().text = "" + cardCount;
    }

    public void updateTime(string time)
    {
        Text_time.text = time;
    }
}
