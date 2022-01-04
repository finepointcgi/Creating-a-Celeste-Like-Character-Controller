using Godot;
using System;

public class Key : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Vector2 velocity = new Vector2(0,0);
    private bool followingPlayer;
    private bool pickedUp;
    [Export]
    public string DoorToOpen;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        if(followingPlayer){
            if(GameManager.Player.GetNode<AnimatedSprite>("AnimatedSprite").FlipH)
                velocity = GameManager.Player.GetNode<Position2D>("KeyFollowLocationRight").GlobalPosition - GlobalPosition;
            else
                 velocity = GameManager.Player.GetNode<Position2D>("KeyFollowLocationLeft").GlobalPosition - GlobalPosition;
        }
        MoveAndSlide(velocity * 5f, Vector2.Up);
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
