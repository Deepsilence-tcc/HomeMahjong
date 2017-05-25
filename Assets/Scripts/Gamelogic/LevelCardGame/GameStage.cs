using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GameStage : MonoBehaviour {

    // 生成算法
    public List<CARD_SHOW_TYPE> genTypes;
    // 刷新算法
    public CARD_REFRESH_TYPE refresh_type = CARD_REFRESH_TYPE.TEST2;
    // 用Test4生成的一定可解路径。
    List<CardSeatMonoHandler> cardWays = new List<CardSeatMonoHandler>();

    FrontController frontController;
    CardSeatMonoHandler[] card_seat_list = new CardSeatMonoHandler[] { };
    private int cards_length = 0;
    Dictionary<int, List<CardSeatMonoHandler>> genTypeCards = new Dictionary<int, List<CardSeatMonoHandler>>();

	void Start () {
        frontController = GameObject.Find("CanvasFront").GetComponent<FrontController>();
        card_seat_list = gameObject.GetComponentsInChildren<CardSeatMonoHandler>();
        cards_length = card_seat_list.Length;

        Init();
	}

    void Init()
    {
        frontController.initCard(cards_length);
        cardShow();

        frontController.showTalks();
    }



    void cardShow()
    {
        List<string> set_types = CardProcess.getInstance().initCardType(cards_length);
        separateCardsByBigPos();

        int num = 0;
        for (int i = 0; i < genTypes.Count; i++)
        {
            List<CardSeatMonoHandler> list = genTypeCards[i];
            List<string> typeA = set_types.GetRange(num, list.Count/2);
            num = num + list.Count/2;
            CardSeatGeneratorFactory.GetFactory().Create(genTypes[i], list, cardWays, typeA);
        }
    }

    //根据牌的属性分成几个大堆
    void separateCardsByBigPos()
    {
        List<CardSeatMonoHandler> tmp;
        for (int i = 0; i < card_seat_list.Length; i++)
        {
            int pos = card_seat_list[i].big_pos;
            if (genTypeCards.TryGetValue(pos, out tmp))
            {
                tmp.Add(card_seat_list[i]);
            }
            else
            {
                tmp = new List<CardSeatMonoHandler>();
                tmp.Add(card_seat_list[i]);
                genTypeCards.Add(pos, tmp);
            }

        }
    }

    CardSeatMonoHandler card_first;
    CardSeatMonoHandler card_right;

    public void OnSelect(string _id)
    {
        //Debug.Log(_id);

        int id = Convert.ToInt32(_id);

        if (id < 1 || id > cards_length)
        {
            return;
        }

        if (card_first == null)
        {
            card_first = getCardById(id);
            card_first.ChangeStatus(CARD_STATUS.SELECTED);

            return;
        }

        if (card_first != null)
        {
            card_right = getCardById(id);

            if (card_right.ID.Equals(card_first.ID))
            {
                card_right = null;

                return;
            }

            if (isCancelSelect(card_right, card_first))
            {
                card_first.ChangeStatus(CARD_STATUS.UNSELECTED);
                card_first = card_right;
                card_first.ChangeStatus(CARD_STATUS.SELECTED);
                card_right = null;

                return;
            }

            if (isCardMatch(card_right, card_first))
            {
                card_first.ChangeStatus(CARD_STATUS.DONE);
                card_right.ChangeStatus(CARD_STATUS.DONE);

                frontController.modifyCard();

                card_first = null;
                card_right = null;


                return;
            }
        }
    }

    CardSeatMonoHandler getCardById(int id) { 
        for(int i=0;i<card_seat_list.Length;i++){
            if (card_seat_list[i].ID == id+"") {
                return card_seat_list[i];
            }
        }
        return null;
    }

    // 是否取消选择
    bool isCancelSelect(CardSeatMonoHandler cardA, CardSeatMonoHandler cardB)
    {
        bool result = false;
        if (((Enum.IsDefined(typeof(CARD_TYPE), cardA.card_type) || Enum.IsDefined(typeof(CARD_TYPE), cardB.card_type)) && cardA.card_type != cardB.card_type))
        {
            result = true;
        }
        else if (Enum.IsDefined(typeof(CARD_TYPE_SEASON), cardA.card_type) && Enum.IsDefined(typeof(CARD_TYPE_PLANT), cardB.card_type))
        {
            result = true;
        }
        else if (Enum.IsDefined(typeof(CARD_TYPE_PLANT), cardA.card_type) && Enum.IsDefined(typeof(CARD_TYPE_SEASON), cardB.card_type))
        {
            result = true;
        }
        return result;
    }

    // 是否配对
    bool isCardMatch(CardSeatMonoHandler cardA, CardSeatMonoHandler cardB) {
        bool result = false;
        if ((Enum.IsDefined(typeof(CARD_TYPE), cardA.card_type) && cardA.card_type == cardB.card_type))
        {
            result = true;
        }
        else if (Enum.IsDefined(typeof(CARD_TYPE_SEASON), cardA.card_type) && Enum.IsDefined(typeof(CARD_TYPE_SEASON), cardB.card_type))
        {
            result = true;
        }
        else if (Enum.IsDefined(typeof(CARD_TYPE_PLANT), cardA.card_type) && Enum.IsDefined(typeof(CARD_TYPE_PLANT), cardB.card_type))
        {
            result = true;
        }
        return result;
    }

    void onCardDone() {
        if (frontController.cardCount > 1 && cardWays[frontController.cardCount - 1] != null
             && cardWays[frontController.cardCount - 2] != null)
        {
            cardWays[frontController.cardCount - 1].ChangeStatus(CARD_STATUS.DONE);
            cardWays[frontController.cardCount - 2].ChangeStatus(CARD_STATUS.DONE);
            cardWays[frontController.cardCount - 1].removeRely();
            cardWays[frontController.cardCount - 2].removeRely();

            frontController.modifyCard();
            Invoke("onCardDone", 1);            
        }
    }

    void defaultWayForCard() {
        // 所有可见的牌
        List<CardSeatMonoHandler> listCard = new List<CardSeatMonoHandler>();
        for (int i = 0; i < card_seat_list.Length; i++)
        {
            if (card_seat_list[i].getStatus() != CARD_STATUS.DONE)
            {
                listCard.Add(card_seat_list[i]);
            }
            else
            {
                card_seat_list[i].removeRely();
            }
        }

        if (listCard.Count > 0)
        {
            // 所有可移除的牌
            List<CardSeatMonoHandler> removables = new List<CardSeatMonoHandler>();
            removables.AddRange(CardProcess.getInstance().ExtractRemovableTilesWeight(1, 1, listCard));

            bool findPair = false;
            while (removables.Count > 1 && !findPair)
            {
                System.Random _random = new System.Random();
                int index = _random.Next() % removables.Count;
                CardSeatMonoHandler card = removables[index];
                removables.RemoveAt(index);

                for (int i = 0; i < removables.Count; i++)
                {
                    if (isCardMatch(card, removables[i]))
                    {
                        findPair = true;
                        card.ChangeStatus(CARD_STATUS.DONE);
                        removables[i].ChangeStatus(CARD_STATUS.DONE);
                        card.removeRely();
                        removables[i].removeRely();
                        break;
                    }
                }
                if (findPair)
                {
                    frontController.modifyCard();
                    Invoke("defaultWayForCard", 1);
                }
            }
        }
    }

    public void onRefreshClick()
    {
        List<CardSeatMonoHandler> list = new List<CardSeatMonoHandler>(card_seat_list);
        cardWays = new List<CardSeatMonoHandler>();
        CardSeatRefreshFactory.GetFactory().Create(refresh_type, list, cardWays);
    }

    public void onHintClick()
    {
        // 所有可见的牌
        List<CardSeatMonoHandler> listCard = new List<CardSeatMonoHandler>();
        for (int i = 0; i < card_seat_list.Length; i++)
        {
            if (card_seat_list[i].getStatus() != CARD_STATUS.DONE)
            {
                listCard.Add(card_seat_list[i]);
            }
            else
            {
                card_seat_list[i].removeRely();
            }
        }

        if (listCard.Count > 0)
        {
            // 所有可移除的牌
            List<CardSeatMonoHandler> removables = new List<CardSeatMonoHandler>();
            removables.AddRange(CardProcess.getInstance().ExtractRemovableTilesWeight(1, 1, listCard));

            bool findPair = false;
            while (removables.Count > 1 && !findPair)
            {
                System.Random _random = new System.Random();
                int index = _random.Next() % removables.Count;
                CardSeatMonoHandler card = removables[index];
                removables.RemoveAt(index);

                for (int i = 0; i < removables.Count; i++)
                {
                    if (isCardMatch(card, removables[i]))
                    {
                        findPair = true;
                        break;
                    }
                }
                if (findPair)
                {
                    //高亮
                }
            }
        }
    }

    public void onAutoPlayClick()
    {
        // 一定不会死局
        if (cardWays != null && cardWays.Count == frontController.step)
        {
            onCardDone();
        }
        else
        {
            // 可能会死局，使用两种算法的刷新
            defaultWayForCard();
        }
    }

    public void onGamePause()
    {

    }
}
