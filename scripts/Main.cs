using System.Reflection;
using Godot;
using System;

public class Main : Node2D
{
	[Export]
	public Color BackgroundColor = ColorUtils.FromRGB(36, 117, 101);
	
	[Export]
	public PackedScene Cards;

	private Card[] deck;
	
	public override void _Ready()
	{
		VisualServer.SetDefaultClearColor(BackgroundColor);
	}

	private void OnHUDStartGame()
	{
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
	}
	
	private void ClearDeck()
	{
		foreach (var node in GetTree().GetNodesInGroup(CardUtils.CardsGroupName))
		{
			((Node2D)node).QueueFree();
		}
	}
}
