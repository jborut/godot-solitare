using Godot;
using System;

public class Card : Area2D
{
	public const string CardsGroupName = "cards";

	public enum CardType : int
	{
		Spades,
		Clubs,
		Hearts,
		Diamonds
	}
	
	private Sprite sprite;
	private CollisionShape2D collisionObject;
	private CardType type = CardType.Clubs;

	public CardType Type => type;
	private int number = 0;

	public int Number => number;
	private float Width = Settings.CardPhysicalWidth;
	private float Height = Settings.CardPhysicalHeight;

	public float DrawWidth { get => Width * Scale.x; }
	public float DrawHeight { get => Height * Scale.y; }

	public bool IsMouseOver => isMouseInside;

	private bool faceUp = false;
	public bool FaceUp 
	{ 
		get
		{
			return faceUp;
		} 
		set 
		{
			faceUp = value;
			sprite.RegionRect = faceUp ? GetRegion() : GetBottomRegion();
		}
	}

	private bool isMouseInside = false;
	private bool isMousePressed = false;
	private Vector2 dragMousePosition;
	
	public override void _Ready()
	{
		AddToGroup(CardsGroupName);
		sprite = GetNode<Sprite>("Sprite");
		sprite.RegionEnabled = true;
		sprite.RegionRect = faceUp ? GetRegion() : GetBottomRegion();

		collisionObject = GetNode<CollisionShape2D>("CollisionShape");
		((RectangleShape2D)collisionObject.Shape).Extents = new Vector2(Width / 2, Height / 2);
		collisionObject.Position = new Vector2(0, 0);
	}
	
	public void SetCardSprite(CardType type, int number)
	{		
		this.type = type;
		this.number = number;
	}

	public void SetDimensions(float width, float height)
	{
		Width = width;
		Height = height;
	}

	public void MoveToRelativePosition(Vector2 position)
	{
		position.x += DrawWidth / 2f;
		position.y += DrawHeight / 2f;
		Position = position;
	}

	public Vector2 GetRelativePosition => new Vector2(Position.x - DrawWidth / 2f, Position.y - DrawHeight / 2f);

	private void OnMouseEntered() => isMouseInside = true;
	private void OnMouseExited() => isMouseInside = false;

	private bool IsDragged => isMouseInside && isMousePressed;
	
	private Rect2 GetRegion() => new Rect2(number * Width - (number * 3), (int)type * Height - ((int)type * 3), Width, Height);
	private Rect2 GetBottomRegion() => new Rect2(0, 4 * Height - 10, Width, Height);
}
