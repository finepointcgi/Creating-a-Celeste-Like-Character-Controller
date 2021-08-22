using Godot;
using System;

public class InterfaceManager : CanvasLayer
{
    public static DialougeManger dialougeManger;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        dialougeManger = GetNode("DialougeManger") as DialougeManger;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
