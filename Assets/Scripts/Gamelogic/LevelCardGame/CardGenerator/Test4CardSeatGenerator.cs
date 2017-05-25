using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Test4CardSeatGenerator : ICardSeatGenerator//: MonoBehaviour 
{
    List<string> set_types;
    //List<CardSeatMonoHandler> list = new List<CardSeatMonoHandler>();
    private int cards_length = 0;
    //位置
    Dictionary<int, List<CardSeatMonoHandler>> posObj = new Dictionary<int, List<CardSeatMonoHandler>>();
    //List<CardSeatMonoHandler> cardWays = new List<CardSeatMonoHandler>();

    public void Generator(List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays, List<string> typeA)
    {
        //this.list = list;
        //this.cardWays = cardWays;
        cards_length = list.Count;
        set_types = typeA;

        DateTime beforDT = System.DateTime.Now;

        CardProcess.getInstance().fallCards(list, posObj, set_types, cardWays);

        for (int i = 0; i < list.Count; i++)
        {
            list[i].recoverRely();
        }

        DateTime afterDT = System.DateTime.Now;
        TimeSpan ts = afterDT.Subtract(beforDT);
        Debug.Log("444444444 总共花费{0}s." + ts.TotalSeconds);
    }

    public void Generator(List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays)
    {
        Generator(list, cardWays, CardProcess.getInstance().initCardType(cards_length));
    }

    public void Generator(List<CardSeatMonoHandler> list) { }

}
