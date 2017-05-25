using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class CardProcess
    {
        private static CardProcess cardProcess;

        public static CardProcess getInstance()
        {
            if (cardProcess == null)
            {

                cardProcess = new CardProcess();

            }
            return cardProcess;
        }

        /// <summary>
        /// Test4掉落算法，从底层开始填满。
        /// </summary>
        /// <param name="list">所有的牌</param>
        /// <param name="posObj">小柱子位置</param>
        /// <param name="set_types">牌色</param>
        /// <param name="cardWays">算出的路线，返回</param>
        public void fallCards(List<CardSeatMonoHandler> list, Dictionary<int, List<CardSeatMonoHandler>> posObj, List<string> set_types, List<CardSeatMonoHandler> cardWays)
        {
            List<CardSeatMonoHandler> tmpC;
            for (int i = 0; i < list.Count; i++)
            {
                int pos = list[i].little_pos;
                if (posObj.TryGetValue(pos, out tmpC))
                {
                    tmpC.Add(list[i]);
                }
                else
                {
                    tmpC = new List<CardSeatMonoHandler>();
                    tmpC.Add(list[i]);
                    posObj.Add(pos, tmpC);
                }
            }

            Dictionary<int, List<CardSeatMonoHandler>> cardDir = new Dictionary<int, List<CardSeatMonoHandler>>();
            List<CardSeatMonoHandler> cardLine = new List<CardSeatMonoHandler>();
            List<CardSeatMonoHandler> cards;

            // 放置特殊点的位置
            int[] tagPos = new int[]{2,11,18};
            int indexPos = 0;
            bool isTag = false;
            for (int i = 0; i < set_types.Count; i++)
            {
                // 拿出所有柱子最底层可选牌
                cardLine = findBottomLine(posObj);
                if (cardLine.Count == 0)
                    break;

                // 按照优先级包括物理级排序的字典
                cardDir = placeByPriority(cardLine);
                // 回复所有牌的状态
                foreach (KeyValuePair<int, List<CardSeatMonoHandler>> kvp in cardDir)
                {
                    for (int m = 0; m < kvp.Value.Count; m++)
                    {
                        kvp.Value[m].recoverRely();
                        kvp.Value[m].isTaken = false;
                    }
                }

                isTag = false;
                if (tagPos.Length > indexPos && i == tagPos[indexPos]) {
                    indexPos++;
                    isTag = true;
                }
                // 取牌
                cards = new List<CardSeatMonoHandler>();
                int num = cardDir.Count;
                if (cardDir.TryGetValue(num, out cards))
                {
                    System.Random _random = new System.Random();
                    if (cards.Count > 1)
                    {
                        int index = _random.Next() % cards.Count;
                        CardSeatMonoHandler tile1 = cards[index];
                        cards.RemoveAt(index);

                        index = _random.Next() % cards.Count;
                        CardSeatMonoHandler tile2 = cards[index];
                        cards.RemoveAt(index);

                        initCards(tile1, tile2, set_types[i], cardWays);
                        tile1.getCard().changeIconTagActive(isTag);
                        tile2.getCard().changeIconTagActive(isTag);
                    }
                    else
                    {
                        // 跨层
                        CardSeatMonoHandler tile1 = cards[0];
                        num--;
                        if (cardDir.TryGetValue(num, out cards))
                        {
                            int index = _random.Next() % cards.Count;
                            CardSeatMonoHandler tile2 = cards[index];
                            cards.RemoveAt(index);

                            initCards(tile1, tile2, set_types[i], cardWays);
                            tile1.getCard().changeIconTagActive(isTag);
                            tile2.getCard().changeIconTagActive(isTag);
                        }
                    }
                }

            }

        }

        //最后为牌附上图片
        public void initCards(CardSeatMonoHandler tile1, CardSeatMonoHandler tile2, string type, List<CardSeatMonoHandler> cardWays)
        {
            System.Random _random = new System.Random();
            if (type == CARD_TYPE.season.ToString())
            {
                if(CommonData.seasonList == null || CommonData.seasonList.Count == 0)
                    CommonData.seasonList = new List<String>(Enum.GetNames(typeof(CARD_TYPE_SEASON)));

                int typeIndex = _random.Next() % (CommonData.seasonList.Count);
                string name1 = CommonData.seasonList[typeIndex];
                CommonData.seasonList.RemoveAt(typeIndex);

                typeIndex = _random.Next() % (CommonData.seasonList.Count);
                string name2 = CommonData.seasonList[typeIndex];
                CommonData.seasonList.RemoveAt(typeIndex);

                genCardPair(tile1, name1, cardWays);
                genCardPair(tile2, name2, cardWays);

            }
            else if (type == CARD_TYPE.plant.ToString())
            {
                if (CommonData.plantList == null || CommonData.plantList.Count == 0)
                    CommonData.plantList = new List<String>(Enum.GetNames(typeof(CARD_TYPE_PLANT)));

                int typeIndex = _random.Next() % (CommonData.plantList.Count);
                string name1 = CommonData.plantList[typeIndex];
                CommonData.plantList.RemoveAt(typeIndex);

                typeIndex = _random.Next() % (CommonData.plantList.Count);
                string name2 = CommonData.plantList[typeIndex];
                CommonData.plantList.RemoveAt(typeIndex);

                genCardPair(tile1, name1, cardWays);
                genCardPair(tile2, name2, cardWays);
            }
            else
            {
                genCardPair(tile1, type, cardWays);
                genCardPair(tile2, type, cardWays);
            }
        }

        List<CardSeatMonoHandler> findBottomLine(Dictionary<int, List<CardSeatMonoHandler>> posObj)
        {
            List<CardSeatMonoHandler> cardLine = new List<CardSeatMonoHandler>();

            foreach (KeyValuePair<int, List<CardSeatMonoHandler>> kvp in posObj)
            {
                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    CardSeatMonoHandler item_card = kvp.Value[i];
                    if (!item_card.isTaken)
                    {
                        //该位置可以放
                        item_card.isTaken = true;
                        cardLine.Add(item_card);
                        break;
                    }
                    //if (i == kvp.Value.Count - 1)
                    //{
                    //    //全部都不能放
                    //    posObj.Remove(kvp.Key);
                    //    j--;
                    //    break;
                    //}
                }
            }

            return cardLine;
        }

        void genCardPair(CardSeatMonoHandler tile, string type, List<CardSeatMonoHandler> cardWays)
        {
            tile.Init(type);
            tile.isTaken = true;

            if (cardWays != null)
            {
                cardWays.Add(tile);
                tile.removeRely();
            }

            //Debug.Log(tile.card_type + ":  " + tile.ID);
        }

        // 优先级处理
        Dictionary<int, List<CardSeatMonoHandler>> placeByPriority(List<CardSeatMonoHandler> cardLine)
        {
            Dictionary<int, List<CardSeatMonoHandler>> cardDir = new Dictionary<int, List<CardSeatMonoHandler>>();
            List<CardSeatMonoHandler> removables;
            List<CardSeatMonoHandler> preRemovables = new List<CardSeatMonoHandler>();

            int num = 0;
            int aa = cardLine[0].z_order;
            int min = -1, max = -1;
            for (int m = 0; m < cardLine.Count; m++)
            {
                int xx = cardLine[m].z_order;
                if (xx > aa)
                {
                    max = xx;
                }
                else if (xx < aa)
                {
                    min = xx;
                }
            }

            if (max == -1)
                max = aa;
            if (min == -1)
                min = aa;

            // Debug.Log(" max:" + max + " min:" + min);

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

        //物理层级比较
        private List<CardSeatMonoHandler> ExtractLevel(List<CardSeatMonoHandler> field, int level)
        {
            List<CardSeatMonoHandler> removables = new List<CardSeatMonoHandler>();

            for (int i = 0; i < field.Count; i++)
            {
                if (field[i].z_order == level)
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


        //////////////////////////////////////////

        //生成限制花色 总牌数1/4种 每种2次 
        public List<string> initCardType(int cards_length)
        {
            List<string> type_list = new List<string>();
            System.Random random = new System.Random();
            int typeIndex = 0;

            List<String> arrNames = new List<String>(Enum.GetNames(typeof(CARD_TYPE)));
            // 所有牌色
            arrNames.RemoveAt(arrNames.Count - 1);
            arrNames.AddRange(arrNames);

            if (cards_length / 2 > arrNames.Count)
            {
                // 如果超过
                arrNames.AddRange(arrNames.GetRange(0, cards_length / 2 - arrNames.Count));
            }

            for (int i = 0; i < arrNames.Count; i++)
            {
                typeIndex = random.Next() % (arrNames.Count);
                string name = arrNames[typeIndex];
                arrNames.RemoveAt(typeIndex);
                type_list.Add(name);
                i--;
            }
            return type_list;
        }

        //无限制花色
        public List<CARD_TYPE> initSimpleCardType(int cards_length)
        {
            List<CARD_TYPE> set_types = new List<CARD_TYPE>();
            System.Random random = new System.Random();
            //可重复
            for (int i = cards_length / 2 - 1; i >= 0; i--)
            {
                int typeIndex = random.Next() % (Enum.GetNames(typeof(CARD_TYPE)).Length - 1);
                string name = Enum.GetName(typeof(CARD_TYPE), typeIndex);
                CARD_TYPE type = (CARD_TYPE)Enum.Parse(typeof(CARD_TYPE), name, true);
                set_types.Add(type);
            }

            return set_types;
        }

        // 刷新前处理花色和牌位
        public List<string> prepareData(List<CardSeatMonoHandler> list)
        {
            List<string> set_types = new List<string>();
            List<string> tmp_set_types = new List<string>();

            List<CardSeatMonoHandler> listCard = new List<CardSeatMonoHandler>();
            List<CardSeatMonoHandler> lastCard = new List<CardSeatMonoHandler>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].getStatus() != CARD_STATUS.DONE)
                {
                    list[i].recoverRely();
                    list[i].isTaken = false;
                    listCard.Add(list[i]);

                    if (Enum.IsDefined(typeof(CARD_TYPE_SEASON), list[i].card_type))
                    {
                        list[i].card_type = CARD_TYPE.season.ToString();
                    }else if (Enum.IsDefined(typeof(CARD_TYPE_PLANT), list[i].card_type))
                    {
                        list[i].card_type = CARD_TYPE.plant.ToString();
                    }

                    if (tmp_set_types.Contains(list[i].card_type))
                    {
                        set_types.Add(list[i].card_type);
                        tmp_set_types.Remove(list[i].card_type);
                    }
                    else
                    {
                        tmp_set_types.Add(list[i].card_type);
                    }
                }
                else
                {
                    lastCard.Add(list[i]);
                }
            }

            for (int i = 0; i < lastCard.Count; i++)
            {
                lastCard[i].removeRely();
                lastCard[i].isTaken = true;
            }
            return set_types;
        }

        //取得当前可点击的牌，根据权重。
        public List<CardSeatMonoHandler> ExtractRemovableTilesWeight(int cards_length, int card_weight, List<CardSeatMonoHandler> field)
        {
            List<CardSeatMonoHandler> removables = new List<CardSeatMonoHandler>();

            for (int i = 0; i < field.Count; i++)
            {
                if (field[i].canMove())
                {
                    removables.Add(field[i]);
                }
            }

            int num = removables.Count;
            if (field.Count == cards_length)
            {
                // 第一层需要权重
                num = removables.Count * card_weight / 10;
            }
            for (int i = 0; i < removables.Count; i++)
            {
                if (i < num)
                {
                    field.Remove(removables[i]);
                }
                else
                {
                    removables.RemoveAt(i);
                    i--;
                }
            }

            return removables;
        }
    }
}