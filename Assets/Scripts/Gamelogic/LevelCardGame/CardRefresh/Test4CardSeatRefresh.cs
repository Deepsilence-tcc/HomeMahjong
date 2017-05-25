using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Test4CardSeatRefresh : ICardSeatRefresh//: MonoBehaviour 
{
    List<string> set_types;
    //List<CardSeatMonoHandler> list = new List<CardSeatMonoHandler>();
    //位置
    Dictionary<int, List<CardSeatMonoHandler>> posObj = new Dictionary<int, List<CardSeatMonoHandler>>();

    public void Generator(List<CardSeatMonoHandler> list)
    {
        Generator(list, null);
    }

    public void Generator(List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays)
    {
        //this.list = list;
        set_types = CardProcess.getInstance().prepareData(list);
        CardProcess.getInstance().fallCards(list, posObj, set_types, cardWays);
    }
}
