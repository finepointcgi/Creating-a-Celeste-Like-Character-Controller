using Godot;
using System;

public class Arrow : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private int speed = 150;
    private float lifeSpan = 20;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Position += Transform.x * delta * speed;
        GD.Print(Position);
        lifeSpan -= delta;
        if(lifeSpan < 0){
            GD.Print("queue free");
            QueueFree();
        }
    }

    private void _on_Area2D_body_entered(object body){
        if(body is ArcherEnemy)
            return;
        QueueFree();
        if(body is KinematicBody2D){
            if(body is ArcherEnemy){
                if(body is PlayerController){
                    PlayerController pc = body as PlayerController;
                    pc.TakeDamage();
                }
            }
        }
    }

}
