using Godot;
using System;

public class GameManager : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public bool GamePaused = false;
    [Export]
    public Position2D RespawnPoint;
    public static GameManager GlobalGameManager;
    public static PlayerController Player;
    public static MagicController MagicController;
    public static QuestManager QuestManager;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if(GlobalGameManager == null){
            GlobalGameManager = this;
        }else{
            QueueFree();
        }
        MagicController = new MagicController();
        QuestManager = new QuestManager();
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
