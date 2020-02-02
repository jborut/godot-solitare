using Godot;
using System;

public static class ColorUtils
{
	public static Color FromRGB(int r, int g, int b)
	{
		return new Color(r / 255f, g / 255f, b / 255f);
	}
}
