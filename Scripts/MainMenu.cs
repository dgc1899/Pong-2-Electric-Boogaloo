using Godot;

public class MainMenu : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach (MenuButton button in GetNode("Menu/ButtonContainer").GetChildren())
        {
            button.Connect("pressed", this, "OnButtonPressed", new Godot.Collections.Array { button.SceneToLoad });
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
            GetTree().ChangeSceneTo(SceneToLoad);
        }

    }

}
