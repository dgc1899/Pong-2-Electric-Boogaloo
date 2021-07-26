using Godot;

public class Paddle : StaticBody2D
{
    const int ScreenHeight = 360;

    [Export]
    float velocity = 225;
    [Export]
    bool PlayerControlled;
    [Export]
    bool Player2;

    Global global;
    RigidBody2D Ball;
    bool BallAI;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");

        if (!global.Player_2 && Player2 || global.Difficulty == 3)
            PlayerControlled = false;
    }
    public void _on_Node2D_BallIsAlive()
    {
        BallAI = true;
        Ball = GetParent().GetNode<RigidBody2D>("Ball_RigidBody");
    }
    public void _on_Node2D_BallIsDead()
    {
        BallAI = false;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Vector2 direction = new Vector2();

        if (PlayerControlled)
        {
            if (((Input.IsActionPressed("P1PaddleUp") && !Player2) || (Input.IsActionPressed("P2PaddleUp") && Player2)) && Position.y - 20 > 22)
            {
                direction.y = -1;
            }
            if (((Input.IsActionPressed("P1PaddleDown") && !Player2) || (Input.IsActionPressed("P2PaddleDown") && Player2)) && Position.y + 20 < ScreenHeight - 22)
            {
                direction.y = 1;
            }
            Position += direction * (delta * velocity);
        }
        else if (BallAI & (GetParent().GetNode<RigidBody2D>("Ball_RigidBody") != null))
        {
            switch (global.Difficulty)
            {
                case 0:
                    direction = _AI_Easy();
                    Position += direction * (delta * 100);
                    break;
                case 1:
                    direction = _AI_Medium();
                    Position += direction * (delta * 150);
                    break;
                case 2:
                    direction = _AI_Hard();
                    Position += direction * (delta * 200);
                    break;
                case 3:
                    direction = _AI_Debug();
                    Position += direction * (delta * velocity);
                    break;
            }
        }
    }
    //Negative Diference = Ball is down relative to the paddle
    //Positive Diference = Ball is up relative to the paddle
    public Vector2 _AI_Easy()
    {
        float Diference = Position.y - Ball.Position.y;
        Vector2 AIDirection = Vector2.Zero;
        if (Diference < -5 && Position.y + 20 < ScreenHeight - 22)
            AIDirection.y = 1;
        else if (Diference > 5 && Position.y - 20 > 22)
            AIDirection.y = -1;

        return AIDirection;
    }
    public Vector2 _AI_Medium()
    {
        float Diference = Position.y - Ball.Position.y;
        Vector2 AIDirection = Vector2.Zero;
        if (Diference < -5 && Position.y + 20 < ScreenHeight - 22)
            AIDirection.y = 1;
        else if (Diference > 5 && Position.y - 20 > 22)
            AIDirection.y = -1;

        return AIDirection;
    }
    public Vector2 _AI_Hard()
    {
        float Diference = Position.y - Ball.Position.y;
        Vector2 AIDirection = Vector2.Zero;
        if (Diference < -5 && Position.y + 20 < ScreenHeight - 22)
            AIDirection.y = 1;
        else if (Diference > 5 && Position.y - 20 > 22)
            AIDirection.y = -1;

        return AIDirection;
    }
    public Vector2 _AI_Debug()
    {
        float Diference = Position.y - Ball.Position.y;
        Vector2 AIDirection = Vector2.Zero;
        if (Diference < -5 && Position.y + 20 < ScreenHeight - 23)
            AIDirection.y = 1;
        else if (Diference > 5 && Position.y - 20 > 22)
            AIDirection.y = -1;

        return AIDirection;
    }
}
