using System;
using System.Collections.Generic;

class CardPair
{
    private static Random _random = new Random();

    public CardSeatMonoHandler Tile1;
    public CardSeatMonoHandler Tile2;

    public CardPair(CardSeatMonoHandler tile1, CardSeatMonoHandler tile2)
    {
        Tile1 = tile1;
        Tile2 = tile2;
    }

    public static CardPair FetchPair(List<CardSeatMonoHandler> tiles)
    {
        if (tiles.Count < 2)
            throw new Exception("Less than two tiles in the list!");

        int index = _random.Next() % tiles.Count;
        CardSeatMonoHandler tile1 = tiles[index];
        tiles.RemoveAt(index);

        index = _random.Next() % tiles.Count;
        CardSeatMonoHandler tile2 = tiles[index];
        while (tile1.isRelyUp(tile2))
        {
            index = _random.Next() % tiles.Count;
            tile2 = tiles[index];
        }
        tiles.RemoveAt(index);

        tile1.removeRely();
        tile2.removeRely();

        return new CardPair(tile1, tile2);
    }
}
