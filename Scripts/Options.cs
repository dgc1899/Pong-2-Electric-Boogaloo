using Godot;

public class Options : Control
{
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
        Tween Tw = GetNode<Tween>("Buttons/Tween");
        for (byte i = 0; i < 10; i++)
        {
            Digits[i] = GD.Load<PackedScene>("res://Scenes/Digits/D_" + i.ToString() + ".tscn");
        }
        foreach (var Node in GetNode("Buttons").GetChildren())
        {
            TextureButton button = Node as TextureButton;
            if (button != null)
            {
                Vector2 New = (Vector2)button.Get("rect_position");
                button.Set("rect_position", new Vector2(-300, New.y));
                button.Visible = true;
                var G = button.GetGroups();
                if (G != null)
                {
                    switch (G[0])
                    {
                        case "Add":
                            button.Set("rect_position", new Vector2(-174, New.y));
                            button.Visible = true;
                            Tw.InterpolateProperty(button, "rect_position", null, new Vector2(150, New.y), 0.5F);
                            Tw.Start();
                            break;
                        case "Rem":
                            button.Set("rect_position", new Vector2(-220, New.y));
                            button.Visible = true;
                            Tw.InterpolateProperty(button, "rect_position", null, new Vector2(104, New.y), 0.5F);
                            Tw.Start();
                            break;
                        case "Label":
                            button.Set("rect_position", new Vector2(-300, New.y));
                            button.Visible = true;
                            Tw.InterpolateProperty(button, "rect_position", null, new Vector2(24, New.y), 0.5F);
                            Tw.Start();
                            break;
                    }
                }
            }
            Node2D Sprite = Node as Node2D;
            if (Sprite != null)
            {
                Vector2 New = (Vector2)Sprite.Get("position");
                Sprite.Set("position", new Vector2(-300, New.y));
                Sprite.Visible = true;
                Tw.InterpolateProperty(Sprite, "position", null, new Vector2(24, New.y), 0.5F);
                Tw.Start();
            }
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
                        Vector2 New = (Vector2)DigitSprite.Get("position");
                        DigitSprite.Set("position", new Vector2(-208 + ExtraDistance, New.y));
                        Tw.InterpolateProperty(DigitSprite, "position", null, new Vector2(118 + ExtraDistance, New.y), 0.5F);
                        Tw.Start();
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
                        Vector2 New = (Vector2)DigitSprite.Get("position");
                        DigitSprite.Set("position", new Vector2(-208 + ExtraDistance, New.y));
                        Tw.InterpolateProperty(DigitSprite, "position", null, new Vector2(118 + ExtraDistance, New.y), 0.5F);
                        Tw.Start();
                        ExtraDistance += 10;
                    }

                }
            }

        }
    }
    public void _on_Timer_timeout()
    {
        QueueFree();
        GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
    }

    public void _on_Back_pressed()
    {
        Tween Tw = GetNode<Tween>("Buttons/Tween");
        foreach (var Node in GetNode("Buttons").GetChildren())
        {
            Control button = Node as Control;
            if (button != null)
            {
                Vector2 New = (Vector2)button.Get("rect_position");
                button.Set("rect_position", new Vector2(-300, New.y));
                var G = button.GetGroups();
                switch (G[0])
                {
                    case "Add":
                        button.Set("rect_position", new Vector2(150, New.y));
                        Tw.InterpolateProperty(button, "rect_position", null, new Vector2(-174, New.y), 0.5F);
                        Tw.Start();
                        break;
                    case "Rem":
                        button.Set("rect_position", new Vector2(104, New.y));
                        Tw.InterpolateProperty(button, "rect_position", null, new Vector2(-220, New.y), 0.5F);
                        Tw.Start();
                        break;
                    case "Label":
                        button.Set("rect_position", new Vector2(24, New.y));
                        Tw.InterpolateProperty(button, "rect_position", null, new Vector2(-300, New.y), 0.5F);
                        Tw.Start();
                        break;
                }
            }
            Node2D Sprite = Node as Node2D;
            if (Sprite != null)
            {
                Vector2 New = (Vector2)Sprite.Get("position");
                Sprite.Set("position", new Vector2(24, New.y));
                Tw.InterpolateProperty(Sprite, "position", null, new Vector2(-300, New.y), 0.5F);
                Tw.Start();
            }
        }
        GetNode<Timer>("Buttons/Timer").Start(0);
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
        var SFX = GetNode<Control>("Buttons/Value_SFX");
        if (SFX.GetChildren().Count == 0)
            for (byte i = 0; i < 3; i++)
            {
                char[] SFXArray = global.SFX.ToCharArray();
                Sprite Digit;
                switch (SFXArray[i])
                {
                    case '0':
                        Digit = Digits[0].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '1':
                        Digit = Digits[1].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '2':
                        Digit = Digits[2].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '3':
                        Digit = Digits[3].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '4':
                        Digit = Digits[4].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '5':
                        Digit = Digits[5].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '6':
                        Digit = Digits[6].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '7':
                        Digit = Digits[7].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '8':
                        Digit = Digits[8].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '9':
                        Digit = Digits[9].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
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
                Sprite Digit;
                switch (SFXArray[i])
                {
                    case '0':
                        Digit = Digits[0].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '1':
                        Digit = Digits[1].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '2':
                        Digit = Digits[2].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '3':
                        Digit = Digits[3].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '4':
                        Digit = Digits[4].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '5':
                        Digit = Digits[5].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '6':
                        Digit = Digits[6].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '7':
                        Digit = Digits[7].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '8':
                        Digit = Digits[8].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                    case '9':
                        Digit = Digits[9].Instance<Sprite>();
                        SFX.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 242));
                        break;
                }
            }
        }
    }
    public void R_Mus()
    {
        var Mus = GetNode<Control>("Buttons/Value_Music");
        if (Mus.GetChildren().Count == 0)
            for (byte i = 0; i < 3; i++)
            {
                char[] MusArray = global.Music.ToCharArray();
                Sprite Digit;
                switch (MusArray[i])
                {
                    case '0':
                        Digit = Digits[0].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '1':
                        Digit = Digits[1].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '2':
                        Digit = Digits[2].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '3':
                        Digit = Digits[3].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '4':
                        Digit = Digits[4].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '5':
                        Digit = Digits[5].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '6':
                        Digit = Digits[6].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '7':
                        Digit = Digits[7].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '8':
                        Digit = Digits[8].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '9':
                        Digit = Digits[9].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
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
                Sprite Digit;
                switch (MusArray[i])
                {
                    case '0':
                        Digit = Digits[0].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '1':
                        Digit = Digits[1].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '2':
                        Digit = Digits[2].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '3':
                        Digit = Digits[3].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '4':
                        Digit = Digits[4].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '5':
                        Digit = Digits[5].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '6':
                        Digit = Digits[6].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '7':
                        Digit = Digits[7].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '8':
                        Digit = Digits[8].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                    case '9':
                        Digit = Digits[9].Instance<Sprite>();
                        Mus.AddChild(Digit);
                        Digit.Set("position", new Vector2(118 + i * 10, 261));
                        break;
                }
            }
        }
    }
    public void MasterRefresh()
    {
        float MusicTodB, SFXTodB;
        MusicTodB = (global.Music.ToFloat() / 100) * 20F;
        SFXTodB = (global.SFX.ToFloat() / 100) * 20F;
        AudioServer.SetBusVolumeDb(1, -20F + SFXTodB);
        AudioServer.SetBusVolumeDb(2, -20F + MusicTodB);
        if (SFXTodB == 0)
            AudioServer.SetBusVolumeDb(1, -80F);
        if (MusicTodB == 0)
            AudioServer.SetBusVolumeDb(2, -80F);
    }
}