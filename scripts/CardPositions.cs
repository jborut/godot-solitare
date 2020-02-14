using Godot;
using System.Collections.Generic;

public class CardPositions
{
    private List<List<Card>> foundationCards = new List<List<Card>>(4);
    private List<List<Card>> tableauCards = new List<List<Card>>(7);
    private List<Card> stockCards = new List<Card>(52);
    private List<Card> talonCards = new List<Card>(52);

    private ViewportUtils viewport;

    public CardPositions(ViewportUtils viewport)
    {
        this.viewport = viewport;
        for (int i = 0; i < 4; i++)
            foundationCards.Add(new List<Card>(13));
        for (int i = 0; i < 7; i++)
            tableauCards.Add(new List<Card>(13));
    }

    public void AddCardToFoundation(Card card, int order)
    {
        foundationCards[order].Add(card);
        card.MoveToRelativePosition(viewport.GetGridPosition(order, 0));
    }

    public void RemoveCardFromFoundation(Card card, int order)
    {
        foundationCards[order].Remove(card);
    }

    public void AddCardToTableau(Card card, int order)
    {
        tableauCards[order].Add(card);
        var pos = viewport.GetGridPosition(order, 1);
        pos.y += Settings.TableauMargin * viewport.ViewportSize.y * (tableauCards[order].Count - 1);
        card.MoveToRelativePosition(pos);
    }

    public void RemoveCardFromTableau(Card card, int order)
    {
        tableauCards[order].Remove(card);
    }

    public void AddCardToStock(Card card)
    {
        stockCards.Add(card);
        card.MoveToRelativePosition(viewport.GetGridPosition(6, 0));
    }

    public void RemoveCardFromStock(Card card)
    {
        stockCards.Remove(card);
    }

    public void AddCardToTalon(Card card)
    {
        talonCards.Add(card);
        card.MoveToRelativePosition(viewport.GetGridPosition(5, 0));
    }

    public void RemoveCardFromTalon(Card card)
    {
        talonCards.Remove(card);
    }

    public void OnViewportResize()
    {
        for (int i = 0; i < 4; i++)
        {
            foreach (var card in foundationCards[i])
                card.MoveToRelativePosition(viewport.GetGridPosition(i, 0));
        }

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < tableauCards[i].Count; j++)
            {
                var pos = viewport.GetGridPosition(i, 1);
                pos.y += Settings.TableauMargin * viewport.ViewportSize.y * j;
                tableauCards[i][j].MoveToRelativePosition(pos);
            }
        }

        foreach (var card in stockCards)
            card.MoveToRelativePosition(viewport.GetGridPosition(6, 0));

        foreach (var card in talonCards)
            card.MoveToRelativePosition(viewport.GetGridPosition(5, 0));
    }
}
