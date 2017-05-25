using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Test2CardSeatGenerator : ICardSeatGenerator//: MonoBehaviour 
{
    List<string> set_types;
    List<CardSeatMonoHandler> list = new List<CardSeatMonoHandler>();
    private int cards_length = 0;
    int card_weight = 10; //10

    public void Generator(List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays, List<string> typeA)
    {
        this.list = list;
        cards_length = list.Count;

        DateTime beforDT = System.DateTime.Now;

        set_types = typeA;
        cardShowTest();

        DateTime afterDT = System.DateTime.Now;
        TimeSpan ts = afterDT.Subtract(beforDT);
        Debug.Log("2222222 总共花费{0}s." + ts.TotalSeconds);
    }
    public void Generator(List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays)
    { }

    public void Generator(List<CardSeatMonoHandler> list)
    {
        this.list = list;
        cards_length = list.Count;

        DateTime beforDT = System.DateTime.Now;

        set_types = CardProcess.getInstance().initCardType(cards_length);
        cardShowTest();

        DateTime afterDT = System.DateTime.Now;
        TimeSpan ts = afterDT.Subtract(beforDT);
        Debug.Log("2222222 总共花费{0}s." + ts.TotalSeconds);    
    }

    void cardShowTest()
    {

        List<CardPair> reversed = new List<CardPair>();
        List<CardSeatMonoHandler> listCard = new List<CardSeatMonoHandler>();
        for (int i = 0; i < list.Count; i++)
        {
            listCard.Add(list[i]);
        }
        List<CardSeatMonoHandler> removables = new List<CardSeatMonoHandler>();

        bool isDead = false;
        while (listCard.Count > 0)
        {
            removables.Clear();
            removables.AddRange(CardProcess.getInstance().ExtractRemovableTilesWeight(cards_length, card_weight, listCard));

            if (removables.Count <= 1)
            {
                // 卡死
                isDead = true;
                break;
            }

            while (removables.Count > 1)
                reversed.Add(CardPair.FetchPair(removables));

            foreach (CardSeatMonoHandler tile in removables)
                listCard.Add(tile);
        }

        for (int i = 0; i < list.Count; i++)
        {
            list[i].recoverRely();
        }

        if (isDead)
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
