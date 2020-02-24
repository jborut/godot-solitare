using Godot;
using System.Collections.Generic;

public class Table : Godot.Object
{
    private Pile[] foundations = new Pile[4];
    private Pile[] tableau = new Pile[7];
    private Pile stock;
    private Pile talon;
    

    private PackedScene piles;
    private ViewportUtils view;

    public Table(ViewportUtils view, PackedScene piles)
    {
        this.view = view;
        this.piles = piles;

        view.OnViewportResize += new ViewportUtils.OnViewportResizeDelegate(OnViewportResize);
    }

    public void Create()
    {
        for (int i = 0; i < 4; i++)
        {
            foundations[i] = (Pile)piles.Instance();
            foundations[i].SetDimension(Settings.CardPhysicalWidth, Settings.CardPhysicalHeight);
        }

        for (int i = 0; i < 7; i++)
        {
            tableau[i] = (Pile)piles.Instance();
            tableau[i].SetDimension(Settings.CardPhysicalWidth, Settings.CardPhysicalHeight);
        }

        stock = (Pile)piles.Instance();
        stock.SetDimension(Settings.CardPhysicalWidth, Settings.CardPhysicalHeight);

        talon = (Pile)piles.Instance();
        talon.SetDimension(Settings.CardPhysicalWidth, Settings.CardPhysicalHeight);

        OnViewportResize();
    }

    public void Clear()
    {
        foreach (var pile in foundations)
        {
            pile.QueueFree();
        }

        foreach (var pile in tableau)
        {
            pile.QueueFree();
        }

        stock.QueueFree();
        talon.QueueFree();
    }

    public Node[] GetTableNodes()
    {
        var nodes = new List<Node>(11);
        nodes.AddRange(foundations);
        nodes.AddRange(tableau);
        nodes.Add(stock);
        nodes.Add(talon);
        return nodes.ToArray();
    }

    private void OnViewportResize()
    {
        for(int i = 0; i < 4; i++)
        {
            if (foundations[i] != null)
            {
                foundations[i].Position = view.GetGridPosition(i, 0);
                foundations[i].Scale = new Vector2(view.RelativeCardScale, view.RelativeCardScale);
            }
        }

        for(int i = 0; i < 7; i++)
        {
            if (tableau[i] != null)
            {
                tableau[i].Position = view.GetGridPosition(i, 1);
                tableau[i].Scale = new Vector2(view.RelativeCardScale, view.RelativeCardScale);
            }
        }

        if (stock != null)
        {
            stock.Position = view.GetGridPosition(6, 0);
            stock.Scale = new Vector2(view.RelativeCardScale, view.RelativeCardScale);
        }

        if (talon != null)
        {
            talon.Position = view.GetGridPosition(5, 0);
            talon.Scale = new Vector2(view.RelativeCardScale, view.RelativeCardScale);
        }
    }

    public bool IsCardCloseToTalon(Card card)
    {
        var size = GetCardWidthHeight;
        return GetNodeRectangle(talon, size).Intersects(GetNodeRectangle(card, size));
    }

    public bool IsCardCloseToStock(Card card)
    {
        var size = GetCardWidthHeight;
        return GetNodeRectangle(stock, size).Intersects(GetNodeRectangle(card, size));
    }

    public bool IsCardCloseToTableau(int ord, Card card)
    {
        var size = GetCardWidthHeight;
        return GetNodeRectangle(tableau[ord], size).Intersects(GetNodeRectangle(card, size));
    }

    public bool IsCardCloseToFoundation(int ord, Card card)
    {
        var size = GetCardWidthHeight;
        return GetNodeRectangle(foundations[ord], size).Intersects(GetNodeRectangle(card, size));
    }

    private Vector2 GetCardWidthHeight => new Vector2(Settings.CardPhysicalWidth * view.RelativeCardScale, Settings.CardPhysicalHeight * view.RelativeCardScale);

    private Rect2 GetNodeRectangle(Node2D node, Vector2 widthHeight) => new Rect2(node.Position, widthHeight);
}
