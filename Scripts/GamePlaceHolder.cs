using Godot;

public class GamePlaceHolder : Node2D
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public void _on_Button_pressed()
    {
        GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
    }
}
