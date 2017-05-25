using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSeatRefreshFactory //: MonoBehaviour 
{
    static CardSeatRefreshFactory factory;

    public static CardSeatRefreshFactory GetFactory()
    {
        if(factory == null){
            factory = new CardSeatRefreshFactory();
        }

        return factory;
    }

    public ICardSeatRefresh Create(CARD_REFRESH_TYPE type, List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays)
    {
        ICardSeatRefresh refresh = new Test2CardSeatRefresh();

        switch(type){
            case CARD_REFRESH_TYPE.TEST2:
                refresh = new Test2CardSeatRefresh();
                refresh.Generator(list);
                break;
            case CARD_REFRESH_TYPE.TEST4:
                refresh = new Test4CardSeatRefresh();
                refresh.Generator(list, cardWays);
                break;
        }

        return refresh;
    }
}
