using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public struct InputState
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public bool Pause;
    public bool Confirm;
    public bool Cancel;
    public Keys Key;

    public bool Inside(Rectangle box)
    {
        if (Position.X < box.Left) return false;
        if (Position.X > box.Right) return false;
        if (Position.Y < box.Top) return false;
        if (Position.Y > box.Bottom) return false;
        return true;
    }
}
