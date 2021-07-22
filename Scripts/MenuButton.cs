using Godot;

public class MenuButton : Button
{
    //Export string variable to capture the node it'll change to
    [Export]
    public PackedScene SceneToLoad { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }


}
