using Godot;

public class Global : Node
{
    public bool Player_2;
    public int Difficulty;
    public string SFX, Music;
    public override void _Ready()
    {
        SFX = "100";
        Music = "100";
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
