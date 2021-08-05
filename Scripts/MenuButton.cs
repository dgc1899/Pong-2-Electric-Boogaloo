using Godot;

public class MenuButton : TextureButton
{
    //Export string variable to capture the node it'll change to
    [Export]
    public PackedScene SceneToLoad { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }
    public void _on_NewGame_mouse_entered()
    {
        GetParent().GetNode<AudioStreamPlayer>("HoverSFX").Play(0);
    }

}
