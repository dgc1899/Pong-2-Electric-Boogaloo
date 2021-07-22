using Godot;

public class InGame : Node2D
{
    internal PackedScene Ball = GD.Load<PackedScene>("res://Scenes/Ball.tscn");
    internal Vector2 Vel = Vector2.Zero;
    internal RandomNumberGenerator RNG = new RandomNumberGenerator();
    internal const float Max_X = 200F, Min_X = 50F, Max_Y = 100F, Min_Y = 20F;
    internal int TimeOut = 2, ScoreP1 = 0, ScoreP2 = 0;
    internal bool Started = false;
    public override void _Ready()
    {
        GetNode<Timer>("Spawn_Timer").Start(0);
    }

    public void _on_Spawn_Timer_timeout()
    {
        if (TimeOut == 0)
        {
            _Spawn_Ball();
            Started = true;
            TimeOut = 2;
        }
        else if (!Started)
        {
            GetNode<Label>("Player_Scored").Set("text", " Game will start in: \n" + TimeOut);
            TimeOut--;
            GetNode<Timer>("Spawn_Timer").Start(0);
        }
        else
        {
            TimeOut--;
            GetNode<Timer>("Spawn_Timer").Start(0);
        }
    }
    public void _Score_Show(bool Who)
    {
        if (Who) //Player 2 Scored
        {
            GetNode<Label>("Player_Scored").Set("text", "Player 2 Scored!");
            ScoreP2++;
            GetNode<Label>("Player_Two_Counter").Set("text", "Score: " + ScoreP2);
        }
        else     //Player 1 Scored
        {
            GetNode<Label>("Player_Scored").Set("text", "Player 1 Scored!");
            ScoreP1++;
            GetNode<Label>("Player_One_Counter").Set("text", "Score: " + ScoreP1);
        }
        GetNode<Timer>("Spawn_Timer").Start(0);
    }
    public void _Spawn_Ball()
    {
        GetNode<Label>("Player_Scored").Set("text", "");
        RigidBody2D IBall = Ball.Instance<RigidBody2D>();
        AddChild(IBall);
        _Randomize_Ball_Direction();
        IBall.Connect("Scored", this, "_Score_Show");
        IBall.Set("linear_velocity", Vel);
    }
    public void _Randomize_Ball_Direction()
    {
        RNG.Randomize();
        if (RNG.RandiRange(0, 1) == 1)
            Vel.x = RNG.RandfRange(Min_X, Max_X);
        else
            Vel.x = RNG.RandfRange(-Min_X, -Max_X);
        if (RNG.RandiRange(0, 1) == 1)
            Vel.y = RNG.RandfRange(Min_Y, Max_Y);
        else
            Vel.y = RNG.RandfRange(-Min_Y, -Max_Y);
    }
}
