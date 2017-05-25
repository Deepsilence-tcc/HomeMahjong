using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Test2CardSeatRefresh : ICardSeatRefresh//: MonoBehaviour 
{
    List<string> set_types = new List<string>();
    List<CardSeatMonoHandler> list = new List<CardSeatMonoHandler>();
    private int cards_length = 0;
    int card_weight = 10; //10

    public void Generator(List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays)
    {
    }

    public void Generator(List<CardSeatMonoHandler> list)
    {
        this.list = list;
        cards_length = list.Count;
        set_types = CardProcess.getInstance().prepareData(list);
        cardShowTest();
    }

    void cardShowTest()
    {

        List<CardPair> reversed = new List<CardPair>();
        List<CardSeatMonoHandler> listCard = new List<CardSeatMonoHandler>();

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].getStatus() != CARD_STATUS.DONE)
            {
                listCard.Add(list[i]);
            }
        }


        List<CardSeatMonoHandler> removables = new List<CardSeatMonoHandler>();

        bool isRefresh = false;
        //是否一定是死局
        bool isHaveMulWay = false;
        int count = 1;
        while (listCard.Count > 0)
        {
            removables.Clear();
            removables.AddRange(CardProcess.getInstance().ExtractRemovableTilesWeight(cards_length, card_weight, listCard));

            if (removables.Count <= 1)
            {
                // 死局
                if (isHaveMulWay)
                {
                    // 只要死局前有两条路，就证明一定还有机会
                    isRefresh = true;
                }
                else
                {
                    // 没有机会
                    isRefresh = false;
                }
                break;
            }

            if (removables.Count > 2)
            {
                // 曾经可选择的路径大于1组
                isHaveMulWay = true;
            }
            else {
                isHaveMulWay = false;
            }

            while (removables.Count > 1)
                reversed.Add(CardPair.FetchPair(removables));

            foreach (CardSeatMonoHandler tile in removables)
                listCard.Add(tile);

            count++;
        }

        for (int i = 0; i < list.Count; i++)
        {
            list[i].recoverRely();
        }

        if (isRefresh)
        {
            cardShowTest();
        }
        else
        {

            for (int i = reversed.Count - 1; i >= 0; i--)
            {
                CardProcess.getInstance().initCards(reversed[i].Tile1, reversed[i].Tile2, set_types[i], null);
            }
        }
    }

}
