using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3CardSeatGenerator : ICardSeatGenerator//: MonoBehaviour 
{
    public void Generator(List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays)
    {
    }
    public void Generator(List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays, List<string> typeA)
    {
    }

    public void Generator(List<CardSeatMonoHandler> list)
    {
        //生成限制花色 总牌数1/4种 每种2次 
        Dictionary<string, int> type_dic = new Dictionary<string, int>();//花色，使用次数
        List<string> type_list = new List<string>();

        System.Random random = new System.Random();

        int type_length = list.Count >> 2;

        string type;
        int typeIndex = 0;
        
        while(true){
            typeIndex = random.Next() % (Enum.GetNames(typeof(CARD_TYPE)).Length - 1);
            type = Enum.GetName(typeof(CARD_TYPE), typeIndex);

            if (!type_dic.ContainsKey(type))
            {
                type_dic.Add(type, 0);
                type_list.Add(type);

                if (type_dic.Count >= type_length) {
                    Debug.Log("Type Dic Count :"+ type_dic.Count.ToString());
                    Debug.Log("Type Dic Count :" + type_dic.Count.ToString());

                    break;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //生成容器
        Dictionary<int, List<CardSeatMonoHandler>> dic = new Dictionary<int, List<CardSeatMonoHandler>>();
        List<List<CardSeatMonoHandler>> list_list = new List<List<CardSeatMonoHandler>>();

        bool b = true;

        int level = 1;

        while (b)
        {
            List<CardSeatMonoHandler> level_list = new List<CardSeatMonoHandler>();

            foreach (CardSeatMonoHandler card in list)
            {

                if (card.canMove())
                {
                    level_list.Add(card);
                }
            }

            dic.Add(level, level_list);
            list_list.Add(level_list);

            level++;

            foreach (CardSeatMonoHandler card in level_list)
            {
                card.removeRely();

                list.Remove(card);
            }

            if (list.Count <= 0)
            {
                b = false;
            }
        }

        Debug.Log("Dic Count : " + dic.Count);

        foreach (KeyValuePair<int, List<CardSeatMonoHandler>> kvp in dic)
        {
            Debug.Log(kvp.Key + " : " + kvp.Value.Count);

            foreach(CardSeatMonoHandler card in kvp.Value){
                card.recoverRely();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //加路径


        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //发牌
        int length = list_list.Count;

        List<CardSeatMonoHandler> list_item;
        List<CardSeatMonoHandler> list_temp = new List<CardSeatMonoHandler>();

        //CardSeatMonoHandler card_seat;

        CardSeatMonoHandler card_seat1;
        CardSeatMonoHandler card_seat2;

        //int type_index = 0;
        //int add = 1;
        for (int i = length - 1; i >= 0; i--)
        {
            list_item = list_list[i];

            Debug.Log("List :　" + list_item.Count.ToString());

            int item_length = list_item.Count;

            bool b1 = false;                            //上一层是否有奇数牌

            if (list_temp.Count > 0)
            {
                b1 = true;

                list_item.Add(list_temp[0]);            //理论上只有一张奇数牌
            }

            list_temp.Clear();

            if (item_length < 1)
            {
                continue;
            }
            else if (item_length < 2)
            {
                list_temp.Add(list_item[0]);            //此层只有一张牌的话 并入下一层
            }
            else
            {
                item_length = list_item.Count;

                //CARD_TYPE type;
                
                //int typeIndex = 0;

                while (true)
                {
                    /*
                    typeIndex = random.Next() % (Enum.GetNames(typeof(CARD_TYPE)).Length - 1);
                    string name = Enum.GetName(typeof(CARD_TYPE), typeIndex);
                    type = (CARD_TYPE)Enum.Parse(typeof(CARD_TYPE), name, true);
                    */

                    //每种花色的牌 只限制出现两次
                    
                    while(true){
                        int type_pos = random.Next() % (type_list.Count);

                        type = type_list[type_pos];

                        if (type_dic[type] < 2)
                        {
                            type_dic[type]++;
                            break;
                        }
                    }
                    

                    /*
                    type = type_list[type_index];

                    type_index += add;

                    if (type_index >= type_list.Count)
                    {
                        add  = -add;
                        type_index += add;
                    }
                    */

                    if (b1)
                    {
                        card_seat1 = list_item[item_length - 1];    //下层奇数项 加在最后
                        card_seat1.Init(type);
                        list_item.RemoveAt(item_length - 1);

                        int pos2 = random.Next() % (list_item.Count);
                        card_seat2 = list_item[pos2];

                        //找一个不压着第一张牌的
                        bool b2 = true;

                        while (b2)
                        {

                            //bool b3 = false;
                            GameObject go = card_seat2.down_hindered;

                            while (go != null)
                            {
                                if (go != card_seat1.gameObject)
                                {
                                    //b3 = true;
                                    break;
                                }
                                pos2 = random.Next() % (list_item.Count);
                                card_seat2 = list_item[pos2];

                                go = card_seat2.down_hindered;
                            }

                            //if (b3)
                            //{
                            //    pos2 = random.Next() % (list.Count);
                            //    card_seat2 = list_item[pos2];
                            //}
                            //else
                            {
                                card_seat2.Init(type);
                                list_item.RemoveAt(pos2);

                                b2 = false;
                            }
                        }

                    }
                    else
                    {
                        int pos1 = random.Next() % (list_item.Count);
                        card_seat1 = list_item[pos1];

                        list_item.RemoveAt(pos1);

                        int pos2 = random.Next() % (list_item.Count);
                        card_seat2 = list_item[pos2];

                        list_item.RemoveAt(pos2);

                        /*
                        CARD_TYPE type3 = CARD_TYPE.max;
                        CARD_TYPE type4 = CARD_TYPE.max;

                        CardSeatMonoHandler card_seat3;
                        CardSeatMonoHandler card_seat4;

                        if (card_seat1.down_hindered != null || card_seat2.down_hindered != null)
                        {

                            if (card_seat1.down_hindered != null)
                            {
                                card_seat3 = card_seat1.down_hindered.GetComponent<CardSeatMonoHandler>();

                                type3 = card_seat3.card_type;
                            }

                            if (card_seat2.down_hindered != null)
                            {
                                card_seat4 = card_seat2.down_hindered.GetComponent<CardSeatMonoHandler>();

                                type4 = card_seat4.card_type;
                            }
                        }

                        while (true)
                        {
                            if ((type3 != CARD_TYPE.max && type == type3) || (type4 != CARD_TYPE.max && type == type4))
                            {
                                type_dic[type]--;

                                while (true)
                                {
                                    int type_pos = random.Next() % (type_list.Count);

                                    type = type_list[type_pos];

                                    if (type_dic[type] < 2)
                                    {
                                        type_dic[type]++;
                                        break;
                                    }
                                }
                            }
                            else {
                                break;
                            }
                        }
                        */

                        card_seat1.Init(type);
                        //list_item.RemoveAt(pos1);

                        card_seat2.Init(type);
                        //list_item.RemoveAt(pos2);
                    }

                    item_length = list_item.Count;

                    card_seat1 = null;
                    card_seat2 = null;

                    if (item_length < 1)
                    {
                        break;
                    }

                    if (item_length < 2)
                    {
                        list_temp.Add(list_item[0]);

                        break;
                    }
                }
            }
        }
	}
}
