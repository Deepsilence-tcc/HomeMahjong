using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSeatGeneratorFactory //: MonoBehaviour 
{
    static CardSeatGeneratorFactory factory;

    public static CardSeatGeneratorFactory GetFactory() {
        if(factory == null){
            factory = new CardSeatGeneratorFactory();
        }

        return factory;
    }

    public ICardSeatGenerator Create(CARD_SHOW_TYPE type, List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays, List<string> typeA)
    {
        ICardSeatGenerator generator = new Test1CardSeatGenerator(); //默认Test1

        switch(type){
            case CARD_SHOW_TYPE.TEST1:
                generator = new Test1CardSeatGenerator();
                generator.Generator(list, cardWays);
                break;

            case CARD_SHOW_TYPE.TEST2:
                generator = new Test2CardSeatGenerator();
                generator.Generator(list, cardWays, typeA);
                break;

            case CARD_SHOW_TYPE.TEST3:
                generator = new Test3CardSeatGenerator();
                generator.Generator(list, cardWays);
                break;
            case CARD_SHOW_TYPE.TEST4:
                generator = new Test4CardSeatGenerator();
                generator.Generator(list, cardWays, typeA);
                break;
        }

        return generator;
    }
}
