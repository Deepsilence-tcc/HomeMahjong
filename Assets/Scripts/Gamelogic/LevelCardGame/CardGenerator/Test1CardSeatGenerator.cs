using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1CardSeatGenerator : ICardSeatGenerator//: MonoBehaviour 
{
    //无限制随机一次生成一对四张牌,基本局局死局，但死局比较靠后
    public void Generator4(List<CardSeatMonoHandler> list)
    {
        //Debug.Log("Test1");

        CardSeatMonoHandler card_seat1;
        CardSeatMonoHandler card_seat2;
        CardSeatMonoHandler card_seat3;
        CardSeatMonoHandler card_seat4;

        //CARD_TYPE type;

        System.Random random = new System.Random();

        int card_pairs = list.Count >> 2;

        //List<int> type_list = new List<int>();

        //Dictionary<int, int> type_dic = new Dictionary<int, int>();

        for (int i = 0; i < card_pairs; i++)
        {
            //bool b3 = true;

            int typeIndex = 0;

            //while (b3)
            {
                typeIndex = random.Next() % (Enum.GetNames(typeof(CARD_TYPE)).Length - 1);

                //if (!type_list.Contains(typeIndex))
                //{
                //    b3 = false;
                //    type_list.Add(typeIndex);
                //}

                //控制花色的的种类 同种最多出两对
                //if (!type_dic.ContainsKey(typeIndex))
                //{
                //    b3 = false;
                //    type_dic.Add(typeIndex, 1);
                //}
                //else
                //{
                //    int j = type_dic[typeIndex];

                //    if (j == 1)
                //    {
                //        b3 = false;
                //        type_dic[typeIndex] = 2;
                //    }
                //}
            }

            string name = Enum.GetName(typeof(CARD_TYPE), typeIndex);

            int pos1 = random.Next() % (list.Count);
            card_seat1 = list[pos1];
            card_seat1.Init(name);
            list.RemoveAt(pos1);

            //int pos2 = random.Next() % (list.Count);
            //card_seat2 = list[pos2];
            //card_seat2.Init(type);
            //list.RemoveAt(pos2);

            int pos2 = random.Next() % (list.Count);
            card_seat2 = list[pos2];

            //一对牌儿不能彼此压着
            bool b = true;

            while (b)
            {
                bool b1 = false;
                GameObject go = card_seat2.up;

                while (go != null)
                {
                    if (go == card_seat1.gameObject)
                    {
                        b1 = true;
                        break;
                    }

                    go = go.GetComponent<CardSeatMonoHandler>().up;
                }

                bool b2 = false;
                go = card_seat2.down_hindered;

                while (go != null)
                {
                    if (go == card_seat1.gameObject)
                    {
                        b2 = true;
                        break;
                    }

                    go = go.GetComponent<CardSeatMonoHandler>().down_hindered;
                }

                if (b1 || b2)
                {
                    pos2 = random.Next() % (list.Count);
                    card_seat2 = list[pos2];
                }
                else
                {
                    card_seat2.Init(name);
                    list.RemoveAt(pos2);

                    b = false;
                }
            }

            int pos3 = random.Next() % (list.Count);
            card_seat3 = list[pos3];
            card_seat3.Init(name);
            list.RemoveAt(pos3);

            int pos4 = random.Next() % (list.Count);
            card_seat4 = list[pos4];
            card_seat4.Init(name);
            list.RemoveAt(pos4);

            card_seat1 = null;
            card_seat2 = null;
            card_seat3 = null;
            card_seat4 = null;
        }

    }


    //无限制随机一次生成一对两张牌 基本局局死局
    public void Generator(List<CardSeatMonoHandler> list)
    {
        //Debug.Log("Test1");

        CardSeatMonoHandler card_seat1;
        CardSeatMonoHandler card_seat2;

        //CARD_TYPE type;

        System.Random random = new System.Random();

        int card_pairs = list.Count >> 1;

        //List<int> type_list = new List<int>();

        Dictionary<int, int> type_dic = new Dictionary<int, int>();

        for (int i = 0; i < card_pairs; i++)
        {
            bool b3 = true;

            int typeIndex = 0;

            while (b3)
            {
                typeIndex = random.Next() % (Enum.GetNames(typeof(CARD_TYPE)).Length - 1);

                //if (!type_list.Contains(typeIndex))
                //{
                //    b3 = false;
                //    type_list.Add(typeIndex);
                //}

                //控制花色的的种类 同种最多出两对
                if (!type_dic.ContainsKey(typeIndex))
                {
                    b3 = false;
                    type_dic.Add(typeIndex, 1);
                }
                else
                {
                    int j = type_dic[typeIndex];

                    if (j == 1)
                    {
                        b3 = false;
                        type_dic[typeIndex] = 2;
                    }
                }
            }

            string name = Enum.GetName(typeof(CARD_TYPE), typeIndex);

            int pos1 = random.Next() % (list.Count);
            card_seat1 = list[pos1];
            card_seat1.Init(name);
            list.RemoveAt(pos1);

            int pos2 = random.Next() % (list.Count);
            card_seat2 = list[pos2];

            //一对牌儿不能彼此压着
            bool b = true;

            while (b)
            {
                bool b1 = false;
                GameObject go = card_seat2.up;

                while (go != null)
                {
                    if (go == card_seat1.gameObject)
                    {
                        b1 = true;
                        break;
                    }

                    go = go.GetComponent<CardSeatMonoHandler>().up;
                }

                bool b2 = false;
                go = card_seat2.down_hindered;

                while (go != null)
                {
                    if (go == card_seat1.gameObject)
                    {
                        b2 = true;
                        break;
                    }

                    go = go.GetComponent<CardSeatMonoHandler>().down_hindered;
                }

                if (b1 || b2)
                {
                    pos2 = random.Next() % (list.Count);
                    card_seat2 = list[pos2];
                }
                else
                {
                    card_seat2.Init(name);
                    list.RemoveAt(pos2);

                    b = false;
                }
            }

            card_seat1 = null;
            card_seat2 = null;
        }

    }

    public void Generator(List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays, List<string> typeA)
    { 
    }

    public void Generator(List<CardSeatMonoHandler> list, List<CardSeatMonoHandler> cardWays)
    {
    }
}
