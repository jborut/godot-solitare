using Godot;
using System;

public class Card : Area2D
{
	public enum CardType : int
	{
		Spades,
		Clubs,
		Hearts,
		Diamonds
	}
	
	private Sprite sprite;
	private CardType type = CardType.Clubs;
	private int number = 0;
	
	private const int OffsetX = 0;
	private const int OffsetY = 2;
	private const int Width = 224;
	private const int Height = 313;
	private const int MarginWidth = 39;
	private const int MarginHeight = 45;

	public float DrawWidth { get => Width * Scale.x; }
	public float DrawHeight { get => Height * Scale.y; }
	
	public override void _Ready()
	{
		
		AddToGroup(CardUtils.CardsGroupName);
		sprite = GetNode<Sprite>("Sprite");
		sprite.RegionEnabled = true;
		sprite.RegionRect = GetRegion();
	}
	
	public void SetCardSprite(CardType type, int number)
	{		
		this.type = type;
		this.number = number;
	}
	
	private Rect2 GetRegion() => new Rect2(OffsetX + number * (Width + MarginWidth), OffsetY + (int)type * (Height + MarginHeight), Width, Height);
}
