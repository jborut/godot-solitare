using Godot;
using System.Collections.Generic;

public class Pile : Node2D
{
    private Polygon2D bgPolygon;

    private List<Vector2> polygonVectors = new List<Vector2>(4);

    public override void _Ready()
    {
        bgPolygon = GetNode<Polygon2D>("BgPolygon");
        bgPolygon.Polygon = polygonVectors.ToArray();
    }

    public void SetDimension(float width, float height)
    {
        polygonVectors.Clear();
        polygonVectors.Add(new Vector2(0, 0));
        polygonVectors.Add(new Vector2(width, 0));
        polygonVectors.Add(new Vector2(width, height));
        polygonVectors.Add(new Vector2(0, height));
    }

    public void SetScale(float scale)
    {
        Scale = new Vector2(scale, scale);
    }
}
