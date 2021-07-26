using Godot;
using System;

public class Ball : RigidBody2D
{
    [Signal]
    delegate void Scored(bool P);
    internal Vector2 Add = new Vector2(1.1F, 1.1F), New = Vector2.Zero, Paddle = Vector2.Zero;
    internal float Displacement = 0;
    internal RandomNumberGenerator RNG = new RandomNumberGenerator();
    const float X = 401, Y = 460;
    public void _on_Area2D_area_entered(Area area)
    {
        if (area.IsInGroup("Player_One"))
            EmitSignal("Scored", true);
        if (area.IsInGroup("Player_Two"))
            EmitSignal("Scored", false);
        GetNode<Timer>("Death").Start(0);
    }
    public void _on_Timer_timeout()
    {
        QueueFree();
    }
    public void _on_Ball_RigidBody_body_exited(Node body)
    {
        if (body.IsInGroup("Paddle"))
        {
            Paddle = (Vector2)body.Get("position");
            if (Math.Abs(LinearVelocity.x) < 275)
            {
                Displacement = Paddle.y - Position.y;
                if (Displacement >= 0)
                    Math.Min(Displacement, 5);
                else
                    Math.Max(Displacement, -5);
                LinearVelocity += new Vector2(0, (Displacement) * -7);
                if (Math.Abs(LinearVelocity.y) < 350)
                    LinearVelocity *= 1.1F;
            }
            else
                LinearVelocity *= 0.9F;
        }
    }
}