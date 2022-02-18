using Godot;
using System;

public abstract class Pickupable : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public int id;
    public Vector2 velocity = new Vector2(0,0);
    public bool followingPlayer;
    public bool pickedUp;
    
    // Called when the node enters the scene tree for the first time.


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public override void _PhysicsProcess(float delta){
        if(followingPlayer){
            if(GameManager.Player.GetNode<AnimatedSprite>("AnimatedSprite").FlipH)
                velocity = GameManager.Player.GetNode<Position2D>("KeyFollowLocationRight").GlobalPosition - GlobalPosition;
            else
                 velocity = GameManager.Player.GetNode<Position2D>("KeyFollowLocationLeft").GlobalPosition - GlobalPosition;
        }
        MoveAndSlide(velocity * 5f, Vector2.Up);
    }
}
