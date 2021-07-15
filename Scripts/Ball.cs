using Godot;
using System;

public class Ball : RigidBody2D
{
	[Signal]
	delegate void Scored(bool P);
	public void _on_Area2D_area_entered(Area area)
    {
		if (area.IsInGroup("Player_One"))
			EmitSignal("Scored", true);
		if (area.IsInGroup("Player_Two"))
			EmitSignal("Scored", false);
    }
}
