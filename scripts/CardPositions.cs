using System.Linq;
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

    public bool CanCardGoOnFoundation(Card card, int order)
    {
        if (foundationCards[order].Count == 0 && card.Number == 0)
            return true;
        else if (foundationCards[order].Count > 0 && foundationCards[order].Last().Type == card.Type && foundationCards[order].Last().Number + 1 == card.Number)
            return true;
        return false;
    }

    public int CardInFoundationPlace(Card card, int foundationOrder) => foundationCards[foundationOrder].FindIndex(c => c == card);

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

   public int CardInTableauPlace(Card card, int tableauOrder) => tableauCards[tableauOrder].FindIndex(c => c == card);

    public void AddCardToStock(Card card)
    {
        stockCards.Add(card);
        card.MoveToRelativePosition(viewport.GetGridPosition(6, 0));
    }

    public void RemoveCardFromStock(Card card)
    {
        stockCards.Remove(card);
    }

    public int CardInStockPlace(Card card) => stockCards.FindIndex(c => c == card);

    public bool RevealLastCardInStock()
    {
        if (stockCards.Count == 0)
            return false;

        stockCards.Last().FaceUp = true;
        return true;
    }

    public bool IsStockEmpty => stockCards.Count == 0;

    public void MoveFromTalonToStock()
    {
        for (int i = talonCards.Count - 1; i >= 0; i--)
        {
            var card = talonCards[i];
            if (i > 0)
                card.FaceUp = false;
            AddCardToStock(card);
        }
        talonCards.Clear();
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

    public int CardInTalonPlace(Card card) => talonCards.FindIndex(c => c == card);

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
