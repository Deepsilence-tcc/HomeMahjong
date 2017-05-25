using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;


public class CardSeatMonoHandler : MonoBehaviour {

    public GameStage stage;

    public GameObject up;               //我的上面有压着我的
    public GameObject left;             //我的左边有挡着我的
    public GameObject right;            //我的右边有挡着我的

    GameObject tmp_up;              
    GameObject tmp_left;          
    GameObject tmp_right;          

    public GameObject down_hindered;    //我的下面有被我压着的    
    public GameObject left_hindered;    //我的左边有被我挡着的
    public GameObject right_hindered;   //我的右边有被我挡着的

    public string ID;
    //public bool is_done = false;        //已经成功消除

    //public 
    CARD_STATUS status = CARD_STATUS.INVAILD;

    private bool b_up;          //上面是否有东西挡着我
    private bool b_left;        //左边是否有东西挡着我
    private bool b_right;       //右边是否有东西挡着我

    public bool isTaken = false;       //是否生成了

    private CardMonoHandler card;

    public string card_type ;//= CARD_TYPE.yibing;

    public int big_pos;
    public int little_pos;
    public int z_order;

    public CardMonoHandler getCard() {
        return card;
    }

    public CARD_STATUS getStatus()
    {
        return status;
    }

    void Awake() {
        card = gameObject.GetComponentInChildren<CardMonoHandler>();

        ID = gameObject.name;

        card.text_id.text = ID;

        if (up != null)
        {
            b_up = true;
        }

        if (left != null)
        {
            b_left = true;
        }

        if (right != null)
        {
            b_right = true;
        }

        tmp_up = up;
        tmp_left = left;
        tmp_right = right;

        //Test 
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}

    bool IsVaild() {
        if(b_up){
            return false;
        }

        if(b_left&&b_right){
            return false;
        }

        return true;
    }

    public void ChangeStatus(CARD_STATUS _status) {

        status = _status;

        if(status == CARD_STATUS.INVAILD){
            card.image.sprite = LoadSprite.getInstance().getSpriteByName(CommonData.MAHJONG_TILE_SET,card_type);
            card.text.enabled = false;
            //card.image.enabled = true;
            //card.button.enabled = true;
        }
        else if(status == CARD_STATUS.VAILD){
            card.image.sprite = LoadSprite.getInstance().getSpriteByName(CommonData.MAHJONG_TILE_SET, card_type+"_v");
            card.text.enabled = false;
            //card.image.enabled = true;
            //card.button.enabled = true;
        }
        else if (status == CARD_STATUS.SELECTED)
        {
            card.text.enabled = true;
        }
        else if (status == CARD_STATUS.UNSELECTED)
        {
            card.text.enabled = false;
        }
        else if (status == CARD_STATUS.DONE)
        {
            card.text_id.enabled = false;
            card.iconTag.enabled = false;
            card.text.enabled = false;
            card.image.enabled = false;
            card.button.enabled = false;

            OnDone();
        }
    }

    public void Init(string type) {
        card_type = type;

        if (IsVaild())
        {
            //status = CART_STATUS.VAILD;

            ChangeStatus(CARD_STATUS.VAILD);
        }
        else
        {

            ChangeStatus(CARD_STATUS.INVAILD);
        }
    }

    public bool canMove()
    {
        if (up == null && (left == null || right == null))
            return true;
        else
            return false;
    }

    public bool canMoveMiddle()
    {
        if (left == null || right == null)
            return true;
        else
        {
            //CardSeatMonoHandler bb = left.GetComponent<CardSeatMonoHandler>();
            //bool a = bb.isTaken;
            //bool b = right.GetComponent<CardSeatMonoHandler>().isTaken;
            if (!left.GetComponent<CardSeatMonoHandler>().isTaken || !right.GetComponent<CardSeatMonoHandler>().isTaken)
                return true;
            else
                return false;
        }
    }

    public bool isRelyUp(CardSeatMonoHandler card2)
    {
        if (up == card2 || down_hindered == card2)
            return true;
        else
            return false;
    }

    public void recoverRely()
    {
        if (tmp_up != null)
            up = tmp_up;
        if (tmp_left != null)
            left = tmp_left;
        if (tmp_right != null)
            right = tmp_right;
    }

    public void removeRely()
    {
        CardSeatMonoHandler card_seat;
        if (down_hindered != null)
        {
            card_seat = down_hindered.GetComponent<CardSeatMonoHandler>();
            if (card_seat.up != null)
            {
                card_seat.tmp_up = card_seat.up;
                card_seat.up = null;
            }
        }
        if (left_hindered != null)
        {
            card_seat = left_hindered.GetComponent<CardSeatMonoHandler>();
            if (card_seat.left != null)
            {
                card_seat.tmp_left = card_seat.left;
                card_seat.left = null;
            }
        }
        if (right_hindered != null)
        {
            card_seat = right_hindered.GetComponent<CardSeatMonoHandler>();
            if (card_seat.right != null)
            {
                card_seat.tmp_right = card_seat.right;
                card_seat.right = null;
            }
        }
    }

    public void ClearUp() {
        b_up = false;

        if (status == CARD_STATUS.INVAILD && IsVaild())
        {
            ChangeStatus(CARD_STATUS.VAILD);
        }
    }

    public void ClearLeft(){
        b_left = false;

        if (status == CARD_STATUS.INVAILD && IsVaild())
        {
            ChangeStatus(CARD_STATUS.VAILD);
        }
    }

    public void ClearRight(){
        b_right = false;

        if (status == CARD_STATUS.INVAILD && IsVaild())
        {
            ChangeStatus(CARD_STATUS.VAILD);
        }
    }

    public void OnDone() {
        //is_done = true;
        //ChangeStatus(CARD_STATUS.DONE);

        CardSeatMonoHandler card_seat;

        //改变相邻牌的状态
        if(down_hindered != null){
            card_seat = down_hindered.GetComponent<CardSeatMonoHandler>();

            card_seat.ClearUp();
        }
        if(left_hindered != null){
            card_seat = left_hindered.GetComponent<CardSeatMonoHandler>();

            card_seat.ClearRight();
        }
        if(right_hindered != null){
            card_seat = right_hindered.GetComponent<CardSeatMonoHandler>();

            card_seat.ClearLeft();
        }

    }

    public void OnSelect() {
        if (status == CARD_STATUS.INVAILD)
            return;

        stage.OnSelect(ID);
    }
}
