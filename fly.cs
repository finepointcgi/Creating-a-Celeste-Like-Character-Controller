using Godot;
using System;

public class fly : Enemy
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        currentState = EnemyState.Moving;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public override void Moving(float delta){
        base.Moving(delta);
        int directionOfMovement = 1;
        if(Position.x > GameManager.Player.Position.x){
            directionOfMovement = -1;
        }else{
            directionOfMovement = 1;
        }
        velocity.x = speed * directionOfMovement;

        GD.Print(Mathf.Abs(Position.x - GameManager.Player.Position.x) );
        if(Mathf.Abs(Position.x - GameManager.Player.Position.x) < 5 || Mathf.Abs(Position.y - GameManager.Player.Position.y) < 5){
            
            currentState = EnemyState.Attacking;
        }

        MoveAndSlide(velocity, Vector2.Up);
    }

    public override void Attacking(float delta)
    {
        base.Attacking(delta);
        Vector2 directionOfMovement = new Vector2();
        directionOfMovement.x = Position.x > GameManager.Player.Position.x ? -1 : 1;
        directionOfMovement.y = Position.y > GameManager.Player.Position.y ? -1 : 1;
        velocity = speed * directionOfMovement;
        LookAt(GameManager.Player.Position);
        MoveAndSlide(velocity, Vector2.Up);
    }

    public override void IdledAfterPlayerDamage(float delta){
        CollisionShape2D collision = (CollisionShape2D)GetNode("CollisionShape2D");
        collision.SetDeferred("disabled", true);
        Area2D area = (Area2D)GetNode("Area2D");
        area.SetDeferred("monitoring", false);
    }

    
    private async void _on_DamageArea_body_entered(object body){
         if(body is PlayerController){
             CollisionShape2D collision = (CollisionShape2D)GetNode("CollisionShape2D");
            collision.SetDeferred("disabled", true);
            PlayerController player = body as PlayerController;
            await ToSignal(GetTree(), "physics_frame");
            player.TakeDamage();
        }
    }
}
