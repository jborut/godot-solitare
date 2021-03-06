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

  private bool isMousePressed = false;
  private Vector2 mouseDragPosition = new Vector2(0, 0);

  private ViewportUtils viewUtils;
  private Table table;
  private Deck deck;
  private CardPositions cardPositions;

  public override void _Ready()
  {
    viewUtils = new ViewportUtils(GetViewport());
    viewUtils.OnViewportResize += new ViewportUtils.OnViewportResizeDelegate(OnViewportResize);

    table = new Table(viewUtils, Piles);
    deck = new Deck(viewUtils, Cards);
    cardPositions = new CardPositions(viewUtils);

    VisualServer.SetDefaultClearColor(BackgroundColor);
  }

  public override void _Process(float delta)
  {
    if (isMousePressed)
    {
      if (deck.SelectedCard != null)
      {
        var currentPosition = GetViewport().GetMousePosition();
        deck.SelectedCard.Position = deck.SelectedCard.Position - (mouseDragPosition - currentPosition);
        mouseDragPosition = currentPosition;
      }
    }
  }

  public override void _Input(InputEvent e)
  {
    if (e is InputEventMouseButton)
    {
      isMousePressed = ((InputEventMouseButton)e).IsPressed();
      if (isMousePressed)
      {
        deck.SelectCard();
      }
      else
      {
        if (deck.SelectedCard != null)
        {
          if (cardPositions.CardInStockPlace(deck.SelectedCard) >= 0 && table.IsCardCloseToTalon(deck.SelectedCard))
          {
            cardPositions.RemoveCardFromStock(deck.SelectedCard);
            cardPositions.AddCardToTalon(deck.SelectedCard);
            if (!cardPositions.IsStockEmpty)
            {
              cardPositions.RevealLastCardInStock();
            }
          }
          else if (table.IsCardCloseToFoundation(0, deck.SelectedCard) && cardPositions.CanCardGoOnFoundation(deck.SelectedCard, 0))
          {
            cardPositions.AddCardToFoundation(deck.SelectedCard, 0);
          }
          else
          {
            deck.ResetSelectedCardPlace();
          }
          deck.DeselectCard();
        }
      }
      mouseDragPosition = GetViewport().GetMousePosition();
    }
  }

  private void OnViewportResize()
  {
    cardPositions.OnViewportResize();
  }

  private void SetTable()
  {
    table.Create();
    foreach(var node in table.GetTableNodes())
      AddChild(node);
  }

  private void SetCards()
  {
    deck.Create();
    deck.Shuffle();
    foreach(var node in deck.GetCardNodes())
      AddChild(node);
  }

  private void InitializeCardPlacement()
  {
    int k = 0;
    for (int i = 0; i < 7; i++)
    {
      for (int j = 0; j < i + 1; j++)
      {
        cardPositions.AddCardToTableau(deck[k++], i);
        if (j == i)
          deck[k - 1].FaceUp = true;
      }
    }

    for (int i = k; i < 52; i++)
    {
      cardPositions.AddCardToStock(deck[i]);
      if (i == 51)
        deck[i].FaceUp = true;
    }
  }

  private void OnHUDStartGame()
  {
    SetTable();
    SetCards();
    InitializeCardPlacement();
  }

  private void ClearDeck()
  {
    deck.Clear();
    table.Clear();
  }
}
