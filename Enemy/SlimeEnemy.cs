using Godot;
using System;

public class SlimeEnemy : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Sprite sprite;
    public int id = 1;
    private float health;
    RayCast2D bottomLeft;
    RayCast2D bottomRight;
    RayCast2D rightMiddle;
    RayCast2D leftMiddle;
    private Vector2 velocity;
    private int gravity = 200;
    private int speed = 30;
    private AnimationPlayer animationPlayer;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        bottomLeft = GetNode<RayCast2D>("LeftRaycast");
        bottomRight = GetNode<RayCast2D>("RightRaycast");
        leftMiddle = GetNode<RayCast2D>("LeftMiddleRaycast");
        rightMiddle = GetNode<RayCast2D>("RightMiddleRaycast");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        velocity.x = speed;
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity.y += gravity * delta;
        if(velocity.y > gravity){
            velocity.y = gravity;
        }
        if(!bottomRight.IsColliding()){
            velocity.x = -speed;
            sprite.FlipH = false;
        }else if(!bottomLeft.IsColliding()){
            velocity.x = speed;
            sprite.FlipH = true;
        }else if(rightMiddle.IsColliding()){
            velocity.x = -speed;
            sprite.FlipH = false;
        }else if(leftMiddle.IsColliding()){
            velocity.x = speed;
            sprite.FlipH = true;
        }
        if(!animationPlayer.IsPlaying()){
            animationPlayer.Play("Move");
        }
        
        MoveAndSlide(velocity,Vector2.Up);
    }

    public void _on_Area2D_body_entered(object body){
        if(body is PlayerController){
            PlayerController player = body as PlayerController;
            player.TakeDamage();
        }
    }

    public void TakeDamage(float damageAmount){
        health -= damageAmount;
        if(health <= 0){
            GameManager.QuestManager.updateQuest(this);
            QueueFree();
        }
    }
}
