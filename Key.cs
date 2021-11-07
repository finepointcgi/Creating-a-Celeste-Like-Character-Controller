using Godot;
using System;

public class Key : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Vector2 velocity = new Vector2(0,0);
    private bool folowingPlayer;
    private bool pickedup;
    [Export]
    public string doorToOpen;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public override void _PhysicsProcess(float delta){
        if(folowingPlayer){           
            if(GameManager.player.GetNode<AnimatedSprite>("AnimatedSprite").FlipH)
                velocity = (GameManager.player.GetNode<Position2D>("KeyPos_right").GlobalPosition - GlobalPosition);
            else
                velocity = (GameManager.player.GetNode<Position2D>("KeyPos_left").GlobalPosition - GlobalPosition);
            MoveAndSlide(velocity * 5f,Vector2.Up);
        }
        else{
             
        }
    }

    private void _on_Area2D_body_entered(object body){
        if(!pickedup){
            folowingPlayer = true;
            GameManager.player.keys.Add(this);
            GetNode<Area2D>("Area2D").SetCollisionLayerBit(1, false);
            pickedup = true;
        }

    }

    private void _on_Area2D_body_exited(object body){
        
    }
}
