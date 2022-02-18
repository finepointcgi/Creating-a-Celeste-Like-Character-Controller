using Godot;
using System;

public class Key : Pickupable
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    
    [Export]
    public string DoorToOpen;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);   
    }

    private void _on_Area2D_body_entered(object body){
        if(!pickedUp){
            if(body is PlayerController){
                followingPlayer = true;
                pickedUp = true;
                GameManager.Player.Keys.Add(this);
            }
        }
    }
}
