using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkController : MonoBehaviour {

    GameObject Text_content;
    GameObject figure;

    void Awake() {
        Text_content = transform.Find("Text_content").gameObject;
        figure = transform.Find("figure").gameObject;
        transform.localPosition = new Vector3(CommonData.BASE_WIDTH, 0, 0);
    }


    public void initBorder(TalkBorder talkborder) {
        Text_content.GetComponent<Text>().text = talkborder.content;
        figure.GetComponent<Image>().sprite = talkborder.getSprite();

        if (talkborder.isLeft == 1)
        {
            figure.transform.localPosition = new Vector3(-240, 13, 0);
            Text_content.transform.localPosition = new Vector3(97, -20, 0);
        }
        else {
            figure.transform.localPosition = new Vector3(167, 13, 0);
            Text_content.transform.localPosition = new Vector3(-107, -20, 0);
        }
    }

}
