using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMonoHandler : MonoBehaviour {

    public Button button;
    public Image image;
    public Text text;
    public Text text_id;
    public Image iconTag;
    CardSeatMonoHandler card_seat;

    void Awake() {
        card_seat = transform.parent.gameObject.GetComponent<CardSeatMonoHandler>();
    }

    public void changeIconTagActive(bool isActive) {
        iconTag.gameObject.SetActive(isActive);
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}


    public void OnSelect() {
        card_seat.OnSelect();
    }
}
