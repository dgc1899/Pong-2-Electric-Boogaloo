using Godot;

public class InGame : Node2D
{
    internal PackedScene Ball = GD.Load<PackedScene>("res://Scenes/Ball.tscn");
    internal PackedScene Pause = GD.Load<PackedScene>("res://Scenes/Pause.tscn");
    internal Vector2 Vel = Vector2.Zero;
    internal RandomNumberGenerator RNG = new RandomNumberGenerator();
    internal const float Max_X = 170F, Min_X = 150F, Max_Y = 80F, Min_Y = 20F;
    internal int TimeOut = 2, ScoreP1 = 0, ScoreP2 = 0;
    internal bool Started = false, OneOff = true;
    internal Label MainLabel;
    Global global;
    [Signal]
    delegate void BallIsAlive();
    [Signal]
    delegate void BallIsDead();
    public override void _Ready()
    {
        GetNode<Timer>("Spawn_Timer").Start(0);
        global = GetNode<Global>("/root/Global");
        MainLabel = GetNode<Label>("Player_Scored");
    }
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            Node2D PauseScreen = Pause.Instance<Node2D>();
            AddChild(PauseScreen);
            GetTree().Paused = true;
        }
    }

    public void _on_Spawn_Timer_timeout()
    {
        if (TimeOut == 0)
        {
            if (OneOff)
            {
                GetNode<AudioStreamPlayer>("SFX/StartSFX").Play(0);
                GetNode<AudioStreamPlayer>("SFX/BackgroundSFX").Play(0);
                OneOff = false;
            }
            _Spawn_Ball();
            Started = true;
            TimeOut = 2;
        }
        else if (!Started)
        {
            GetNode<AudioStreamPlayer>("SFX/CountdownSFX").Play(0);
            MainLabel.Set("text", " Game will start in: \n" + TimeOut);
            TimeOut--;
            GetNode<Timer>("Spawn_Timer").Start(0);
        }
        else
        {
            TimeOut--;
            GetNode<Timer>("Spawn_Timer").Start(0);
        }
    }
    public void _on_End_timer_timeout()
    {
        GetNode<AudioStreamPlayer>("SFX/BackgroundSFX").Stop();
        GetNode<AudioStreamPlayer>("SFX/WinSFX").Play(0);
        if (ScoreP1 == 3)
            if (global.Player_2)
                MainLabel.Set("text", "Player 1 Wins!");
            else
                MainLabel.Set("text", "You win!");
        if (ScoreP2 == 3)
            if (global.Player_2)
                MainLabel.Set("text", "Player 2 Wins!");
            else
                MainLabel.Set("text", "You've lost...");

    }
    public void _Score_Show(bool Who)
    {
        EmitSignal("BallIsDead");
        GetNode<AudioStreamPlayer>("SFX/ScoreSFX").Play(0);
        if (Who) //Player 2 Scored
        {
            if (global.Player_2)
                MainLabel.Set("text", "Player 2 Scored!");
            else
                MainLabel.Set("text", "Enemy has Scored!");
            ScoreP2++;
            GetNode<Label>("Player_Two_Counter").Set("text", "Score: " + ScoreP2);
        }
        else     //Player 1 Scored
        {
            if (global.Player_2)
                MainLabel.Set("text", "Player 1 Scored!");
            else
                MainLabel.Set("text", "You've Scored!");
            ScoreP1++;
            GetNode<Label>("Player_One_Counter").Set("text", "Score: " + ScoreP1);
        }
        if (ScoreP1 == 3 || ScoreP2 == 3)
        {
            Tween T = GetNode<Tween>("SFX/Tween");
            T.InterpolateProperty(GetNode<AudioStreamPlayer>("SFX/BackgroundSFX"), "volume_db", -17, -30, 1F);
            T.Start();
            GetNode<Timer>("End_Timer").Start(0);
        }
        else
            GetNode<Timer>("Spawn_Timer").Start(0);

    }
    public void _Spawn_Ball()
    {
        MainLabel.Set("text", "");
        RigidBody2D IBall = Ball.Instance<RigidBody2D>();
        AddChild(IBall);
        _Randomize_Ball_Direction();
        IBall.Connect("Scored", this, "_Score_Show");
        IBall.Set("linear_velocity", Vel);
        EmitSignal("BallIsAlive");
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
