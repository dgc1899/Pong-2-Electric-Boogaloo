using Godot;

public class Paddle : StaticBody2D
{
    const int ScreenHeight = 360;

    [Export]
    float velocity = 225;
    [Export]
    bool PlayerControlled;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Vector2 direction = new Vector2();

        if (((Input.IsActionPressed("P1PaddleUp") && PlayerControlled == true) || (Input.IsActionPressed("P2PaddleUp") && PlayerControlled == true)) && Position.y-20 > 22)
        {
            direction.y = -1;
        }
        if (((Input.IsActionPressed("P1PaddleDown") && PlayerControlled == true) || (Input.IsActionPressed("P2PaddleDown") && PlayerControlled == true)) && Position.y+20 < ScreenHeight - 22)
        {
            direction.y = 1;
        }


        Position += direction * (delta * velocity);
    }
}
