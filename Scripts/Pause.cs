using Godot;

public class Pause : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Global global;
    Timer T;
    TextureButton AddSFX, RemSFX, AddMusic, RemMusic;
    AudioStreamPlayer HoverSFX;
    PackedScene[] Digits = new PackedScene[10];
    int SFXValue, MusicValue;
    bool Pressed = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        T = GetNode<Timer>("PaHTimer");
        AddSFX = GetNode<TextureButton>("Buttons/AddSFX");
        RemSFX = GetNode<TextureButton>("Buttons/RemSFX");
        AddMusic = GetNode<TextureButton>("Buttons/AddMusic");
        RemMusic = GetNode<TextureButton>("Buttons/RemMusic");
        global = GetNode<Global>("/root/Global");
        SFXValue = int.Parse(global.SFX);
        HoverSFX = GetNode<AudioStreamPlayer>("Buttons/HoverSFX");
        MusicValue = int.Parse(global.Music);
        for (byte i = 0; i < 10; i++)
        {
            Digits[i] = GD.Load<PackedScene>("res://Scenes/Digits/D_" + i.ToString() + ".tscn");
        }
        foreach (var Node in GetNode("Buttons").GetChildren())
        {
            Control Cont = Node as Control;
            if (Cont != null)
            {
                if (Cont.Name == "Value_SFX")
                {
                    R_SFX();
                    var Dig = Cont.GetChildren();
                    float ExtraDistance = 0;
                    foreach (Sprite DigitSprite in Dig)
                    {
                        DigitSprite.Set("position", new Vector2(118 + ExtraDistance, 24));
                        ExtraDistance += 10;
                    }

                }
                if (Cont.Name == "Value_Music")
                {
                    R_Mus();
                    var Dig = Cont.GetChildren();
                    float ExtraDistance = 0;
                    foreach (Sprite DigitSprite in Dig)
                    {
                        DigitSprite.Set("position", new Vector2(118 + ExtraDistance, 43));
                        ExtraDistance += 10;
                    }

                }
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            GetTree().Paused = false;
            QueueFree();
        }
    }
    public void _on_Continue_pressed()
    {
        GetTree().Paused = false;
        QueueFree();
    }
    public void _on_Continue_mouse_entered()
    {
        HoverSFX.Play(0);
    }
    public void _on_Menu_pressed()
    {
        GetNode<TextureButton>("Buttons/Confirmation").Visible = true;
        GetNode<TextureButton>("Buttons/Menu").Visible = false;
    }
    public void _on_Menu_mouse_entered()
    {
        HoverSFX.Play(0);
    }
    public void _on_Confirmation_pressed()
    {
        QueueFree();
        GetTree().Paused = false;
        GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
    }
    public void _on_Confirmation_mouse_entered()
    {
        HoverSFX.Play(0);
    }
    public void _on_PaHTimer_timeout()
    {
        Pressed = true;
        if (AddSFX.Pressed && SFXValue < 100)
            SFXValue += 5;
        if (RemSFX.Pressed && SFXValue > 0)
            SFXValue -= 5;
        if (AddMusic.Pressed && MusicValue < 100)
            MusicValue += 5;
        if (RemMusic.Pressed && MusicValue > 0)
            MusicValue -= 5;
        if (AddSFX.Pressed || RemSFX.Pressed || AddMusic.Pressed || RemMusic.Pressed)
        {
            HoverSFX.Play(0);
            T.Start(0);
            if (T.WaitTime >= 0.15F)
                T.WaitTime -= 0.05F;
            R();
        }
        else
        {
            Pressed = false;
            T.Stop();
            T.WaitTime = 0.3F;
        }

    }
    public void _on_AddSFX_pressed()
    {
        HoverSFX.Play(0);
        if (SFXValue < 100 && !Pressed)
        {
            SFXValue += 5;
            R();
        }
    }
    public void _on_AddSFX_button_down()
    {
        T.Start(0);
    }
    public void _on_RemSFX_pressed()
    {
        HoverSFX.Play(0);
        if (SFXValue > 0 && !Pressed)
        {
            SFXValue -= 5;
            R();
        }
    }
    public void _on_RemSFX_button_down()
    {
        T.Start(0);
    }
    public void _on_AddMusic_pressed()
    {
        HoverSFX.Play(0);
        if (MusicValue < 100 && !Pressed)
        {
            MusicValue += 5;
            R();
        }
    }
    public void _on_AddMusic_button_down()
    {
        T.Start(0);
    }
    public void _on_RemMusic_pressed()
    {
        HoverSFX.Play(0);
        if (MusicValue > 0 && !Pressed)
        {
            MusicValue -= 5;
            R();
        }
    }
    public void _on_RemMusic_button_down()
    {
        T.Start(0);
    }
    public void R()
    {
        if (SFXValue == 100)
            global.SFX = SFXValue.ToString();
        else if (SFXValue < 100 && SFXValue > 5)
            global.SFX = "0" + SFXValue.ToString();
        else if (SFXValue == 5)
            global.SFX = "00" + SFXValue.ToString();
        else
            global.SFX = "000";
        R_SFX();

        if (MusicValue == 100)
            global.Music = MusicValue.ToString();
        else if (MusicValue < 100 && MusicValue > 5)
            global.Music = "0" + MusicValue.ToString();
        else if (MusicValue == 5)
            global.Music = "00" + MusicValue.ToString();
        else
            global.Music = "000";
        R_Mus();
        MasterRefresh();
    }
    public void R_SFX()
    {
        const int X = 118, Y = 24;
        var SFX = GetNode<Control>("Buttons/Value_SFX");
        if (SFX.GetChildren().Count == 0)
            for (byte i = 0; i < 3; i++)
            {
                char[] SFXArray = global.SFX.ToCharArray();
                switch (SFXArray[i])
                {
                    case '0':
                        AddDigit(SFX, 0, X, Y, i);
                        break;
                    case '1':
                        AddDigit(SFX, 1, X, Y, i);
                        break;
                    case '2':
                        AddDigit(SFX, 2, X, Y, i);
                        break;
                    case '3':
                        AddDigit(SFX, 3, X, Y, i);
                        break;
                    case '4':
                        AddDigit(SFX, 4, X, Y, i);
                        break;
                    case '5':
                        AddDigit(SFX, 5, X, Y, i);
                        break;
                    case '6':
                        AddDigit(SFX, 6, X, Y, i);
                        break;
                    case '7':
                        AddDigit(SFX, 7, X, Y, i);
                        break;
                    case '8':
                        AddDigit(SFX, 8, X, Y, i);
                        break;
                    case '9':
                        AddDigit(SFX, 9, X, Y, i);
                        break;
                }
            }
        else
        {
            foreach (Node Dig in SFX.GetChildren())
            {
                Dig.QueueFree();
            }
            for (byte i = 0; i < 3; i++)
            {
                char[] SFXArray = global.SFX.ToCharArray();
                switch (SFXArray[i])
                {
                    case '0':
                        AddDigit(SFX, 0, X, Y, i);
                        break;
                    case '1':
                        AddDigit(SFX, 1, X, Y, i);
                        break;
                    case '2':
                        AddDigit(SFX, 2, X, Y, i);
                        break;
                    case '3':
                        AddDigit(SFX, 3, X, Y, i);
                        break;
                    case '4':
                        AddDigit(SFX, 4, X, Y, i);
                        break;
                    case '5':
                        AddDigit(SFX, 5, X, Y, i);
                        break;
                    case '6':
                        AddDigit(SFX, 6, X, Y, i);
                        break;
                    case '7':
                        AddDigit(SFX, 7, X, Y, i);
                        break;
                    case '8':
                        AddDigit(SFX, 8, X, Y, i);
                        break;
                    case '9':
                        AddDigit(SFX, 9, X, Y, i);
                        break;
                }
            }
        }
    }
    public void R_Mus()
    {
        const int X = 118, Y = 43;
        var Mus = GetNode<Control>("Buttons/Value_Music");
        if (Mus.GetChildren().Count == 0)
            for (byte i = 0; i < 3; i++)
            {
                char[] MusArray = global.Music.ToCharArray();
                switch (MusArray[i])
                {
                    case '0':
                        AddDigit(Mus, 0, X, Y, i);
                        break;
                    case '1':
                        AddDigit(Mus, 1, X, Y, i);
                        break;
                    case '2':
                        AddDigit(Mus, 2, X, Y, i);
                        break;
                    case '3':
                        AddDigit(Mus, 3, X, Y, i);
                        break;
                    case '4':
                        AddDigit(Mus, 4, X, Y, i);
                        break;
                    case '5':
                        AddDigit(Mus, 5, X, Y, i);
                        break;
                    case '6':
                        AddDigit(Mus, 6, X, Y, i);
                        break;
                    case '7':
                        AddDigit(Mus, 7, X, Y, i);
                        break;
                    case '8':
                        AddDigit(Mus, 8, X, Y, i);
                        break;
                    case '9':
                        AddDigit(Mus, 9, X, Y, i);
                        break;
                }
            }
        else
        {
            foreach (Node Dig in Mus.GetChildren())
            {
                Dig.QueueFree();
            }
            for (byte i = 0; i < 3; i++)
            {
                char[] MusArray = global.Music.ToCharArray();
                switch (MusArray[i])
                {
                    case '0':
                        AddDigit(Mus, 0, X, Y, i);
                        break;
                    case '1':
                        AddDigit(Mus, 1, X, Y, i);
                        break;
                    case '2':
                        AddDigit(Mus, 2, X, Y, i);
                        break;
                    case '3':
                        AddDigit(Mus, 3, X, Y, i);
                        break;
                    case '4':
                        AddDigit(Mus, 4, X, Y, i);
                        break;
                    case '5':
                        AddDigit(Mus, 5, X, Y, i);
                        break;
                    case '6':
                        AddDigit(Mus, 6, X, Y, i);
                        break;
                    case '7':
                        AddDigit(Mus, 7, X, Y, i);
                        break;
                    case '8':
                        AddDigit(Mus, 8, X, Y, i);
                        break;
                    case '9':
                        AddDigit(Mus, 9, X, Y, i);
                        break;
                }
            }
        }
    }
    public void AddDigit(Control CNode, int digit, float X, float Y, int i)
    {
        Sprite DigitToAdd = Digits[digit].Instance<Sprite>();
        CNode.AddChild(DigitToAdd);
        DigitToAdd.Set("position", new Vector2(X + i * 10, Y));
    }
    public void MasterRefresh()
    {
        float MusicTodB, SFXTodB;
        MusicTodB = (global.Music.ToFloat() / 100) * 20F;
        SFXTodB = (global.SFX.ToFloat() / 100) * 20F;
        AudioServer.SetBusVolumeDb(1, -20F + SFXTodB);
        AudioServer.SetBusVolumeDb(2, -20F + MusicTodB);
        if(SFXTodB == 0)
            AudioServer.SetBusVolumeDb(1, -80F);
        if(MusicTodB == 0)
            AudioServer.SetBusVolumeDb(2, -80F);
    }
}
