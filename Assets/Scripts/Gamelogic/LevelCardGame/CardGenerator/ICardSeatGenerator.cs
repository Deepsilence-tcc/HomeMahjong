using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardSeatGenerator //: MonoBehaviour 
{
    void Generator(List<CardSeatMonoHandler> list);
    //固定路线
    void Generator(List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays);
    //固定路线，固定花色
    void Generator(List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays, List<string> typeA);
}
