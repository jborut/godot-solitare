using Godot;
using System;

public class Main : Node2D
{
  [Export]
  public Color BackgroundColor = ColorUtils.FromRGB(36, 117, 101);

  [Export]
  public PackedScene Cards;

  [Export]
  public PackedScene Piles;

  private Card[] deck;
  private Card selectedCard;

  private int currentZindex = 64;

  private bool isMousePressed = false;
  private Vector2 mouseDragPosition = new Vector2(0, 0);

  private ViewportUtils viewUtils;
  private Table table;

  public override void _Ready()
  {
    viewUtils = new ViewportUtils(GetViewport());
    viewUtils.OnViewportResize += new ViewportUtils.OnViewportResizeDelegate(OnViewportResize);

    table = new Table(viewUtils, Piles);

    VisualServer.SetDefaultClearColor(BackgroundColor);
  }

  public override void _Process(float delta)
  {
    if (isMousePressed)
    {
      if (selectedCard != null)
      {
        var currentPosition = GetViewport().GetMousePosition();
        selectedCard.Position = selectedCard.Position - (mouseDragPosition - currentPosition);
        mouseDragPosition = currentPosition;
      }
    }
  }

  public override void _Input(InputEvent e)
  {
    if (e is InputEventMouseButton)
    {
      isMousePressed = ((InputEventMouseButton)e).IsPressed();
      if (isMousePressed && deck != null)
      {
        foreach (var card in deck)
        {
          if (card.IsMouseOver)
          {
            selectedCard = card;
            selectedCard.ZIndex = currentZindex++;
            break;
          }
        }
      }
      else
      {
        selectedCard = null;
      }
      mouseDragPosition = GetViewport().GetMousePosition();
    }
  }

  private void OnViewportResize()
  {
  }

  private void SetTable()
  {
    table.SetTable();
    foreach(var node in table.GetTableNodes())
    {
      AddChild(node);
    }
  }

  private void OnHUDStartGame()
  {
    SetTable();
    /*
    float offsetX = 0;
    float offsetY = 0;
    float fullWidth = 0;

    ClearDeck();

    deck = CardUtils.GetRandomizedDeck(Cards);

    for (int i = 0; i < deck.Length; i++)
    {
      if (offsetX == 0)
      {
      offsetX = deck[i].DrawWidth / 2 + 10;
      offsetY = deck[i].DrawHeight / 2 + 10;

      fullWidth = 13 * (deck[i].DrawWidth + 10);
      }

      AddChild(deck[i]);

      deck[i].Position = new Vector2(offsetX + i * (deck[i].DrawWidth + 10) - (i / 13) * fullWidth, offsetY + (i / 13) * (deck[i].DrawHeight + 10));
      deck[i].Show();
    }
    */


  }

  private void ClearDeck()
  {
    foreach (var node in GetTree().GetNodesInGroup(CardUtils.CardsGroupName))
    {
      ((Node2D)node).QueueFree();
    }
  }
}
