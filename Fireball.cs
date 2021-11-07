using Godot;
using System;

public class Fireball : Spell
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private int damageAmount = 2;
    private string resourcePath = "";
    private int speed = 80;
    private bool faceDirection;
    private float lifeSpan = 20;
    
    public override void _PhysicsProcess(float delta)
    {
        if(faceDirection)
            Position -= (Transform.x * delta * speed);
        else
            Position += Transform.x * delta * speed;
        lifeSpan -= delta;
        if(lifeSpan < 0){
            QueueFree();
        }
    }

    public override void _Ready()
    {
        GD.Print("test");
    }
    public override void castSpell()
    {
        throw new NotImplementedException();
    }

    public override void loadResourcePath(){

    }

    public override void SetUp(bool facedirection){
        faceDirection = facedirection;
        GetNode<Sprite>("Sprite").FlipH = faceDirection;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void _on_Area2D_body_entered(object body){
        QueueFree();
        if(body is KinematicBody2D){
            if(body is PlayerController){
                PlayerController pc = body as PlayerController;
                pc.TakeDamage();
            }
        }
    }
}
