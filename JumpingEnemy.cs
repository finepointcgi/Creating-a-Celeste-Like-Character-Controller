using Godot;
using System;

public class JumpingEnemy : Enemy
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private bool isInAir;
    private float maxJumpDistance = 30;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        currentState = EnemyState.Idle;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Moving:
                Moving(delta);
                break;
            case EnemyState.Attacking:
                Attacking(delta);
                break;
            case EnemyState.Dieing:
                break;
            default:
                break;
        }
    }

    public  override void Idle()
    {
        //animationPlayer.Play("Idle");
    }

    public override void Moving(float delta){
         velocity.y += gravity * delta;
        if(velocity.y > gravity){
            velocity.y = gravity;
        }

        
        if(!IsOnFloor()){
            GD.Print(velocity);
            velocity.x = Mathf.Lerp(velocity.x, 0, friction);
            //velocity.x = Mathf.Lerp(velocity.x, directionOfMovement * speed, acceleration);
            
        }else{
            int directionOfMovement = 1;
            if(Position.x > GameManager.Player.Position.x){
                directionOfMovement = -1;
            }else{
                directionOfMovement = 1;
            }
            velocity.x = maxJumpDistance * directionOfMovement;
            //isInAir = true;
            velocity.y = 0;
            velocity.y = -jumpHeight;
            
            
        }
        GD.Print(Mathf.Abs(Position.x - GameManager.Player.Position.x) );
        if(Mathf.Abs(Position.x - GameManager.Player.Position.x) < 1 && Position.y < GameManager.Player.Position.y){
            
            currentState = EnemyState.Attacking;
        }

        MoveAndSlide(velocity, Vector2.Up);
    }

    public override void Attacking(float delta){
        GD.Print("welp");
        velocity.x = 0;
        velocity.y = 100;
        MoveAndSlide(velocity, Vector2.Up);
        if(IsOnFloor()){
            currentState = EnemyState.Moving;
        }
    }

    private void _on_Area2D_body_entered(object body){
        if(body is PlayerController){
            currentState = EnemyState.Moving;
        }
    }

    private void _on_Area2D_body_exited(object body){
        if(body is PlayerController){
            currentState = EnemyState.Moving;
        }
    }

    private void _on_Moving_body_entered(object body){
        if(body is PlayerController){
            currentState = EnemyState.Moving;
        }
    }

    private void _on_Moving_body_exited(object body){
        if(body is PlayerController){
            currentState = EnemyState.Idle;
        }
    }

    private async void _on_DamageArea_body_entered(object body){
         if(body is PlayerController){
            PlayerController player = body as PlayerController;
            CollisionShape2D shape = GetNode("CollisionShape2D") as CollisionShape2D;
            shape.Disabled = true;
            await ToSignal(GetTree(), "physics_frame");
            player.TakeDamage();
        }
    } 
    
}
