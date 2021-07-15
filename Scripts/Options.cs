using Godot;
using System;

public class Options : Control
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_BackButton_pressed()
    {
        GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
    }

}
