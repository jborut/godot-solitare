using Godot;
using System;
using System.Collections.Generic;

public class Deck
{
    private PackedScene cardsScene;
    private ViewportUtils viewport;

    private List<Card> cards = new List<Card>(52);
    private Card selectedCard = null;
    private int selectedCardZIndex = 64;
    private Vector2 selectedCardStartPosition;
    private int selectedCardStartZIndex;

    public Card SelectedCard => selectedCard;

    public Card this[int i] => cards[i];

    public Deck(ViewportUtils viewport, PackedScene cardsScene)
    {
        this.cardsScene = cardsScene;
        this.viewport = viewport;
        this.viewport.OnViewportResize += new ViewportUtils.OnViewportResizeDelegate(OnViewportResize);
    }

    public void Create()
    {
        cards.Clear();

        foreach(Card.CardType type in Enum.GetValues(typeof(Card.CardType)))
		{
			for (int i = 0; i < 13; i++)
			{
                var card = (Card)cardsScene.Instance();
                card.SetCardSprite(type, i);
                card.SetDimensions(Settings.CardPhysicalWidth, Settings.CardPhysicalHeight);
                cards.Add(card);
            }
        }

        OnViewportResize();
    }

    public void Clear()
    {
        foreach(var card in cards)
        {
            card.QueueFree();
        }
        cards.Clear();
    }

    public void SelectCard()
    {
        selectedCard = null;

        foreach (var card in cards)
        {
            if (card.IsMouseOver && card.FaceUp)
            {
                if (selectedCard == null || (selectedCard != null && selectedCard.ZIndex < card.ZIndex)) 
                {
                    selectedCard = card;
                    selectedCardStartPosition = selectedCard.Position;
                    selectedCardStartZIndex = selectedCard.ZIndex;
                }
            }
        }

        if (selectedCard != null)
            selectedCard.ZIndex = selectedCardZIndex++;
    }

    public void ResetSelectedCardPlace()
    {
        if (selectedCard != null)
        {
            selectedCard.Position = selectedCardStartPosition;
            selectedCard.ZIndex = selectedCardStartZIndex;
        }
    }

    public void DeselectCard()
    {
        if (selectedCard != null)
        {
            selectedCard = null;
        }
    }

    public Card[] GetCardNodes()
    {
        return cards.ToArray();
    }

    public void Shuffle()
    {
        cards.Shuffle();
        for (int i = 0; i < 52; i++)
        {
            cards[i].ZIndex = i;
        }
    }

    private void OnViewportResize()
    {
        foreach (var card in cards)
        {
            card.Scale = new Vector2(viewport.RelativeCardScale, viewport.RelativeCardScale);
        }
    }

}
