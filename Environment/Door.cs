using Godot;
using System;

public class Door : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    public string DoorKey;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void _on_Area2D_body_entered(object body){
        if(body is PlayerController){
            if(GameManager.Player.Keys.FindAll(k => k.DoorToOpen == DoorKey).Count != 0){
                Key k = GameManager.Player.Keys.Find(x => x.DoorToOpen.Contains(DoorKey));
                GameManager.Player.Keys.Remove(k);
                k.QueueFree();
                GD.Print("open Door");
                GetNode<AnimationPlayer>("AnimationPlayer").Play("OpenDoor");
            }else{
                GD.Print("Need Key");
            }
        }
    }
}
