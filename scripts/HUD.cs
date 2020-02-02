using Godot;
using System;

public class HUD : CanvasLayer
{
	[Signal]
	public delegate void StartGame();
	
	public enum HudState
	{
		MainMenu,
		GameMenu
	}
	
	private HudState hudState = HudState.MainMenu;
	
	private Label statsLabel;
	private Button startButton;
	private Button undoButton;
	
	public override void _Ready()
	{
		statsLabel = GetNode<Label>("StatsLabel");
		startButton = GetNode<Button>("StartButton");
		undoButton = GetNode<Button>("UndoButton");
		
		ApplyState(HudState.MainMenu);
	}
	
	public void OnGameEnd()
	{
		ApplyState(HudState.MainMenu);
	}

	private void ApplyState(HudState state)
	{
		hudState = state;
		
		switch (hudState)
		{
			case HudState.MainMenu:
				statsLabel.Text = "";
				statsLabel.Hide();
				startButton.Show();
				undoButton.Hide();
				break;
			case HudState.GameMenu:
				statsLabel.Text = "";
				statsLabel.Show();
				startButton.Hide();
				undoButton.Show();
				break;
		}
	}
	
	private void OnStartButtonPressed()
	{
		ApplyState(HudState.GameMenu);
		EmitSignal("StartGame");
	}
}
