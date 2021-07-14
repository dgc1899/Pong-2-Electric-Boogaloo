using Godot;
using System;

public class InGame : Node2D
{
    internal PackedScene Ball = GD.Load<PackedScene>("res://Scenes/Ball.tscn");
    internal Vector2 Vel = Vector2.Zero;
    internal RandomNumberGenerator RNG = new RandomNumberGenerator();
    internal const float Max_X = 200F, Min_X = 50F, Max_Y = 100F, Min_Y = 20F;
    public override void _Ready()
    {
        GetNode<Timer>("Spawn_Timer").Start(0);
    }

    public void _on_Spawn_Timer_timeout()
    {
        RigidBody2D IBall = Ball.Instance<RigidBody2D>();
        AddChild(IBall);
        _Randomize_Ball_Direction();
        IBall.Connect("Scored", this, "_Score_Show");
        IBall.Set("linear_velocity", Vel);
    }
    public void _Score_Show(bool Who) //true for player 1, false for player 2
    {
        if (Who)
        {
            GetNode<Label>("Player_Scored").Set("text", "Player 2 Scored!!");
        }
        else
        {
            GetNode<Label>("Player_Scored").Set("text", "Player 1 Scored!!");
        }
    }
    public void _Randomize_Ball_Direction()
    {
        RNG.Randomize();
        if(RNG.RandiRange(0, 1) == 1)
            Vel.x = RNG.RandfRange(Min_X, Max_X);
        else
            Vel.x = RNG.RandfRange(-Min_X, -Max_X);
        if (RNG.RandiRange(0, 1) == 1)
            Vel.y = RNG.RandfRange(Min_Y, Max_Y);
        else
            Vel.y = RNG.RandfRange(-Min_Y, -Max_Y);
    }
}
