using Godot;

public class MainMenu : Control
{
    // Called when the node enters the scene tree for the first time.
    internal PackedScene STL;
    public override void _Ready()
    {
        foreach (var Node in GetNode("Buttons").GetChildren())
        {
            MenuButton button = Node as MenuButton;
            Tween T = GetNode<Tween>("Buttons/Tween");
            if (button != null)
            {
                button.Connect("pressed", this, "OnButtonPressed", new Godot.Collections.Array { button.SceneToLoad });
                Vector2 New = (Vector2)button.Get("rect_position");
                button.Set("rect_position", new Vector2(-300, New.y));
                button.Visible = true;
                T.InterpolateProperty(button, "rect_position", null, new Vector2(24, New.y), 0.5F);
                T.Start();
            }
        }
    }
    public void OnButtonPressed(PackedScene SceneToLoad)
    {
        if (SceneToLoad == null)
        {
            GetTree().Quit();
        }
        else
        {
            STL = SceneToLoad;
            Tween T = GetNode<Tween>("Buttons/Tween");
            foreach (var Node in GetNode("Buttons").GetChildren())
            {
                MenuButton button = Node as MenuButton;
                if (button != null)
                {
                    Vector2 New = (Vector2)button.Get("rect_position");
                    T.InterpolateProperty(button, "rect_position", null, new Vector2(-300, New.y), 0.5F);
                    T.Start();
                }
            }
            GetNode<Timer>("Buttons/Timer").Start(0);
        }

    }
    public void _on_Timer_timeout()
    {
        QueueFree();
        GetTree().ChangeSceneTo(STL);
    }
    public void _on_OptionsButton_mouse_entered()
    {
        GetNode<AudioStreamPlayer>("Buttons/HoverSFX").Play(0);
    }
    public void _on_QuitButton_mouse_entered()
    {
        GetNode<AudioStreamPlayer>("Buttons/HoverSFX").Play(0);
    }
}
