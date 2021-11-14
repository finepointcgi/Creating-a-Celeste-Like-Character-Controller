using Godot;
using System;

public class Slime : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private PlayerController player;
    private int Health = 1;
    private int speed = 30;
    private int acceleration = 10;
    private int facingDirection = 1;
    private Vector2 velocity = new Vector2();
    private int gravity = 200;
    Sprite sprite;
    RayCast2D bottemLeft;
    RayCast2D bottomRight;
    RayCast2D midLeft;
    RayCast2D midRight;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        bottemLeft = GetNode<RayCast2D>("BottomLeft");
        bottomRight = GetNode<RayCast2D>("BottomRight");
        midLeft = GetNode<RayCast2D>("MidLeft");
        midRight = GetNode<RayCast2D>("MidRight");
        velocity.x = speed;
        //animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {

        if (!bottemLeft.IsColliding())
        {
            velocity.x = speed;
            GD.Print("bottem Left");
        }
        else if (!bottomRight.IsColliding())
        {
            velocity.x = -speed;
            GD.Print("bottem Right");
        }
        if(midRight.IsColliding()){
            if(!(midRight.GetCollider() is PlayerController)){
                GD.Print(midRight.GetCollider());
                velocity.x *= -1;
            }
        }
        if (midLeft.IsColliding())
        {
            if(!(midLeft.GetCollider() is PlayerController)){
                GD.Print(midLeft.GetCollider());
            velocity.x *= -1;
            }
           
        }
        velocity.y += gravity * delta;
        if (velocity.y > gravity)
        {
            velocity.y = gravity;
        }

        velocity.y = MoveAndSlide(velocity, GetFloorNormal()).y;




        if (!GetNode<AnimationPlayer>("AnimationPlayer").IsPlaying())
        {
            GetNode<AnimationPlayer>("AnimationPlayer").Play("move");
        }


    }

    public void _on_Detection_Radius_body_entered(object body)
    {
        GD.Print("body has entered" + body);
        if (body is PlayerController)
        {
            player = body as PlayerController;
            player.TakeDamage();
            //active = true;
        }
    }

    public void TakeDamage(int damageAmount){
        Health -= damageAmount;
        if(Health <= 0)
            QueueFree();
    }




}
