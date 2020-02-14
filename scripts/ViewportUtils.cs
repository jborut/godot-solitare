using Godot;

public class ViewportUtils : Godot.Object // it has to extend Godot.Object because we want to Connect to viewport events
{
    private Viewport viewport;
    public Vector2 ViewportSize
    {
        get => viewport.Size;
        set
        {
            viewport.Size = value;
            OnViewportResize?.Invoke();
        }
    }

    public float RelativeCardScale => (ViewportSize.x * Settings.CardRelativeWidthPercentage) / Settings.CardPhysicalWidth;

    public delegate void OnViewportResizeDelegate();
    public event OnViewportResizeDelegate OnViewportResize;

    public ViewportUtils(Viewport rootViewport)
    {
        viewport = rootViewport;
        viewport.Connect("size_changed", this, "OnViewportSizeChange");
    }

    /// <summary>
    /// The whole table is a grid of 7x2
    /// This method will return actual position in pixels
    /// for a given column,row value
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector2 GetGridPosition(int x, int y)
    {
        if (x < 0 || x > 6 || y < 0 || y > 1)
            return new Vector2(0, 0);

        return new Vector2(
            (x + 1) * Settings.MarginLeftPercentage * ViewportSize.x + Settings.CardPhysicalWidth * RelativeCardScale * x,
            (y + 1) * Settings.MarginTopPercentage * ViewportSize.y + Settings.CardPhysicalHeight * RelativeCardScale * y
        );
    }

    private void OnViewportSizeChange()
    {
        ViewportSize = viewport.Size;
    }
}
