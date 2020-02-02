using Godot;
using System;
using System.Collections.Generic;

public static class CardUtils
{
    private static Random random = new Random();

    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) 
        {
            n--;  
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static Card[] GetRandomizedDeck(PackedScene cardScene)
    {
        List<Card> cards = new List<Card>(52);

        foreach(Card.CardType type in Enum.GetValues(typeof(Card.CardType)))
		{
			for (int i = 0; i < 13; i++)
			{
                var card = (Card)cardScene.Instance();
                card.SetCardSprite(type, i);
                cards.Add(card);
            }
        }

        cards.Shuffle();
        return cards.ToArray();
    }
}
