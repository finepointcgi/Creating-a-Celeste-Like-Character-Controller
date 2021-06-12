using Godot;
using System;

public class GameManager : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    public Position2D RespawnPoint;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void RespawnPlayer(){
        PlayerController pc = GetNode<PlayerController>("Player");
        pc.GlobalPosition = RespawnPoint.GlobalPosition;
        pc.RespawnPlayer();
    }

    private void _on_Player_Death(){
        RespawnPlayer();
    }
}
