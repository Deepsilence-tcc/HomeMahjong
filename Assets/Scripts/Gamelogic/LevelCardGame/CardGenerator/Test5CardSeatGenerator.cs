using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test5CardSeatGenerator : ICardSeatGenerator//: MonoBehaviour 
{
    List<string> set_types;
    List<CardSeatMonoHandler> list = new List<CardSeatMonoHandler>();
    private int cards_length = 0;
    //位置
    List<GameObject> posObj = new List<GameObject>();
    List<CardSeatMonoHandler> cardWays = new List<CardSeatMonoHandler>();

    public void Generator(List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays, List<string> typeA)
    {
    }

    public void Generator(List<CardSeatMonoHandler> list)
    {
    }

    public void Generator(List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays)
    {
        this.list = list;
        this.cardWays = cardWays;
        cards_length = list.Count;
        initCardType();

        DateTime beforDT = System.DateTime.Now;

        fallCards();

        //此处为你需要计算运行时间的代码  

        DateTime afterDT = System.DateTime.Now;
        TimeSpan ts = afterDT.Subtract(beforDT);
        Debug.Log("DateTime总共花费{0}s."+ ts.TotalSeconds);    
    }

    void initCardType()
    {
        set_types = new List<string>();
        System.Random random = new System.Random();
        //可重复
        for (int i = cards_length / 2 - 1; i >= 0; i--)
        {
            int typeIndex = random.Next() % (Enum.GetNames(typeof(CARD_TYPE)).Length - 1);
            string name = Enum.GetName(typeof(CARD_TYPE), typeIndex);
            set_types.Add(name);
        }
    }

    void fallCards()
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject posItem = list[i].transform.parent.gameObject;
            if (!posObj.Contains(posItem))
            {
                posObj.Add(posItem);
            }
        }

        Dictionary<int, List<CardSeatMonoHandler>> cardDir = new Dictionary<int, List<CardSeatMonoHandler>>();
        List<CardSeatMonoHandler> cardLine = new List<CardSeatMonoHandler>();
        List<CardSeatMonoHandler> cards;
        for (int i = 0; i < set_types.Count; i++)
        {
            cardLine = findBottomLine();
            cardDir = placeByPriority(cardLine);

            foreach (KeyValuePair<int, List<CardSeatMonoHandler>> kvp in cardDir)
            {
                for (int m = 0; m < kvp.Value.Count; m++)
                {
                    kvp.Value[m].recoverRely();
                    kvp.Value[m].isTaken = false;
                }
            }
            cards = new List<CardSeatMonoHandler>();
            int num = cardDir.Count;
            if (cardDir.TryGetValue(num, out cards))
            {
                //Debug.Log("============" + cards.Count);

                System.Random _random = new System.Random();
                if (cards.Count > 1)
                {
                    int index = _random.Next() % cards.Count;
                    CardSeatMonoHandler tile1 = cards[index];
                    cards.RemoveAt(index);

                    index = _random.Next() % cards.Count;
                    CardSeatMonoHandler tile2 = cards[index];
                    cards.RemoveAt(index);

                    string type = set_types[i];
                    genCardPair(tile1, type);
                    genCardPair(tile2, type);
                }
                else
                {
                    CardSeatMonoHandler tile1 = cards[0];
                    num--;
                    if (cardDir.TryGetValue(num, out cards))
                    {
                        int index = _random.Next() % cards.Count;
                        CardSeatMonoHandler tile2 = cards[index];
                        cards.RemoveAt(index);

                        string type = set_types[i];
                        genCardPair(tile1, type);
                        genCardPair(tile2, type);
                    }
                }
            }

        }

    }

    // 优先级处理
    Dictionary<int, List<CardSeatMonoHandler>> placeByPriority(List<CardSeatMonoHandler> cardLine)
    {
        Dictionary<int, List<CardSeatMonoHandler>> cardDir = new Dictionary<int, List<CardSeatMonoHandler>>();
        List<CardSeatMonoHandler> removables;
        List<CardSeatMonoHandler> preRemovables = new List<CardSeatMonoHandler>(); 

        int num = 0;
        float aa = cardLine[0].transform.localPosition.x;
        int min = -1, max = -1;
        for (int m = 0; m < cardLine.Count; m++)
        {
            float xx = cardLine[m].transform.localPosition.x;
            //Debug.Log(cardLine[m].ID + "  xx:" + xx + " , aa:" + aa);
            if (xx > aa)
            {
                max = (int)xx / 10;
            }
            else if (xx < aa)
            {
                min = (int)xx / 10;
            }
        }

        if (max == -1)
            max = (int)aa / 10;
        if (min == -1)
            min = (int)aa / 10;
        //Debug.Log("///////// max:" + max + " , min:" + min);
        if (min != max)
        {
            preRemovables = ExtractLevel(cardLine, min);
        }

        while (cardLine.Count > 0)
        {
            removables = ExtractRemovableTiles(cardLine);
            if (removables.Count > 0)
            {
                num++;
                cardDir.Add(num, removables);
            }
        }

        if (preRemovables.Count > 0)
        {
            num++;
            cardDir.Add(num, preRemovables);
        }

        return cardDir;
    }

    void genCardPair(CardSeatMonoHandler tile, string type)
    {
        tile.Init(type);
        tile.removeRely();
        tile.isTaken = true;
        cardWays.Add(tile);

        //Debug.Log(tile.card_type + ":  " + tile.ID);
    }

    List<CardSeatMonoHandler> findBottomLine()
    {
        List<CardSeatMonoHandler> cardLine = new List<CardSeatMonoHandler>();
        for (int j = 0; j < posObj.Count; j++)
        {
            for (int i = 0; i < posObj[j].transform.childCount; i++)
            {
                //从底层到高层 
                CardSeatMonoHandler item_card = posObj[j].transform.GetChild(i).GetComponent<CardSeatMonoHandler>();
                if (!item_card.isTaken)
                {
                    //该位置可以放
                    item_card.isTaken = true;
                    cardLine.Add(item_card);
                    break;
                }
                if (i == posObj[j].transform.childCount - 1)
                {
                    //全部都不能放
                    posObj.RemoveAt(j);
                    j--;
                    break;
                }
            }
        }

        return cardLine;
    }

    //物理层级比较
    private List<CardSeatMonoHandler> ExtractLevel(List<CardSeatMonoHandler> field, int level)
    {
        List<CardSeatMonoHandler> removables = new List<CardSeatMonoHandler>();

        for (int i = 0; i < field.Count; i++)
        {
            if (field[i].transform.localPosition.x == level*10)
            {
                removables.Add(field[i]);
            }
        }

        for (int i = 0; i < removables.Count; i++)
        {
            removables[i].removeRely();
            field.Remove(removables[i]);
        }

        return removables;
    }

    //逻辑层级比较
    private List<CardSeatMonoHandler> ExtractRemovableTiles(List<CardSeatMonoHandler> field)
    {
        List<CardSeatMonoHandler> removables = new List<CardSeatMonoHandler>();
        for (int i = 0; i < field.Count; i++)
        {
            if (field[i].canMoveMiddle())
            {
                removables.Add(field[i]);
            }
        }

        for (int i = 0; i < removables.Count; i++)
        {
            removables[i].removeRely();
            field.Remove(removables[i]);
        }

        return removables;
    }

}
