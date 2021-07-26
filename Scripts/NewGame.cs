using Godot;

public class NewGame : Control
{
    CheckBox OnePlayer;
    CheckBox TwoPlayer;
    VBoxContainer DifficultyContainer;
    OptionButton Difficulties;
    Button PlayButton;
    Global global;
    // Called when the node enters the scene tree for the first time.s
    public override void _Ready()
    {
        OnePlayer = GetNode<CheckBox>("PlayerOptionContainer/OnePlayerCheck");
        TwoPlayer = GetNode<CheckBox>("PlayerOptionContainer/TwoPlayerCheck");
        PlayButton = GetNode<Button>("PlayContainer/PlayButton");
        DifficultyContainer = GetNode<VBoxContainer>("DifficultyContainer");
        Difficulties = GetNode<OptionButton>("DifficultyContainer/OptionButton");
        DifficultyContainer.Visible = false;
        global = GetNode<Global>("/root/Global");

        //Connect pressed signals
        OnePlayer.Connect("pressed", this, "OnOnePlayerPressed");
        TwoPlayer.Connect("pressed", this, "OnTwoPlayerPressed");
    }
    public void _on_BackButton_pressed()
    {
        GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
    }
    public void OnOnePlayerPressed()
    {
        DifficultyContainer.Visible = true;
        TwoPlayer.Pressed = false;

        Difficulties.AddItem("Easy", 1);
        Difficulties.AddItem("Medium", 2);
        Difficulties.AddItem("Hard", 3);
        Difficulties.AddItem("Debug", 4);
    }
    public void OnTwoPlayerPressed()
    {
        OnePlayer.Pressed = false;
        DifficultyContainer.Visible = false;
    }
    public void _on_PlayButton_pressed()
    {
        if ((OnePlayer.Pressed == true && Difficulties.Selected != -1) || TwoPlayer.Pressed == true)
        {
            global.Difficulty = Difficulties.Selected;
            global.Player_2 = TwoPlayer.Pressed;
            GetTree().ChangeScene("res://Scenes/InGame.tscn");
        }
    }
}
