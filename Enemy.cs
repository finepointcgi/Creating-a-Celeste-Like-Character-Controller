using Godot;
using System;

public abstract class Enemy : KinematicBody2D
{
    public enum EnemyState
    {
        Idle,
        Moving,
        Attacking,
        Damaged,
        IdledAfterPlayerDamage,
        Dieing
    }

    public Sprite sprite;
    [Export]
    public float health;
    public Vector2 velocity;
    public int gravity = 200;
    [Export]
    public int speed = 30;
    public AnimationPlayer animationPlayer;
    public EnemyState currentState;
    public int facingDirection = 0;
    [Export]
    public float friction;
    [Export]
    public float acceleration;
    [Export]
    public float jumpHeight;
    public bool isInAir;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

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
            case EnemyState.IdledAfterPlayerDamage:
                IdledAfterPlayerDamage(delta);
                break;
            case EnemyState.Dieing:
                Die(delta);
                break;
            default:
                break;
        }
    }
    
    public virtual void Idle(){

    }

    public virtual void Moving(float delta){

    }

    public virtual void Attacking(float delta){

    }

    public virtual void IdledAfterPlayerDamage(float delta){

    }
    public virtual void Die(float delta){

    }
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            QueueFree();
        }
    }
}
