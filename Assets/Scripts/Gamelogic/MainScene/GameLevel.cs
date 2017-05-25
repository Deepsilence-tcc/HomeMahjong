using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevel : MonoBehaviour {

    public int id;
    bool is_lock = false;
    MainFrontController mainFrontController;
    Text Text_lv;
    GameObject Unlock;
    GameObject Lock;
    List<Transform> stars;
    Sprite star1;
    Sprite star2;

    void Awake() {
        Unlock = transform.Find("Unlock").gameObject;
        Lock = transform.Find("Lock").gameObject;

        stars = new List<Transform>();
        Transform star;
        for (int i = 1; i <= 3; i++) {
            star = Unlock.transform.Find("star" + i);
            stars.Add(star);
        }

        mainFrontController = GameObject.Find("CanvasFront").GetComponent<MainFrontController>();
        Text_lv = Unlock.transform.Find("Text_lv").GetComponent<Text>();
        Text_lv.text = "" + id;

        if (id <= PlayerPrefs.GetInt(CommonData.BEST_LEVEL, 0) + 1)
        {
            Unlock.transform.localScale = Vector3.one;
            Lock.transform.localScale = Vector3.zero;
        }
        else {
            Unlock.transform.localScale = Vector3.zero;
            Lock.transform.localScale = Vector3.one;
        }

        string name = "Texture/Atlas/star";
        Texture2D texture1 = (Texture2D)Resources.Load(name);
        Rect rect = new Rect(0, 0, texture1.width, texture1.height);
        star1 = Sprite.Create(texture1, rect, new Vector2(0.5f, 0.5f));

        name = "Texture/Atlas/star_empty";
        Texture2D texture2 = (Texture2D)Resources.Load(name);
        Rect rect2 = new Rect(0, 0, texture2.width, texture2.height);
        star2 = Sprite.Create(texture2, rect2, new Vector2(0.5f, 0.5f));

    }

    public void showStar(int starcount) {
        for (int i = 0; i < stars.Count; i++) {
            if (i < starcount)
            {
                stars[i].GetComponent<Image>().sprite = star1;
            }
            else {
                stars[i].GetComponent<Image>().sprite = star2;
            }
        }
    }

    public void OnClick()
    {
        if (!is_lock && Unlock.transform.localScale.x == 1)
        {
            mainFrontController.onLevelClick(id);
        }
        else
        {
            //Debug.Log("Lock");
        }
    }
    
}
