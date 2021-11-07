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
            if(GameManager.player.keys.FindAll(p => p.doorToOpen == DoorKey).Count != 0){
                Key k = GameManager.player.keys.Find(x => x.doorToOpen.Contains(DoorKey));
                GameManager.player.keys.Remove(k);
                k.QueueFree();
                GD.Print("openDoor");
                GetNode<AnimationPlayer>("AnimationPlayer").Play("Open Door");
            }else{
                GD.Print("need key");
            }
        }
    }

    public void _on_Area2D_body_exited(object body){

    }
}
