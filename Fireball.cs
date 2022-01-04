using Godot;
using System;

public class FireBall : Spell
{
    public string ResourcePath = "res://Spells/FireBall.tscn";
    AnimationPlayer player;
    [Export]
    public bool ableToMove;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetNode<AnimationPlayer>("AnimationPlayer");
        player.Play("cast");
    }

    public override void _PhysicsProcess(float delta)
    {
        if(ableToMove){
            if(!player.IsPlaying()){
                player.Play("idle");
            }
            if(faceDirection)
                Position -= (Transform.x * delta * Speed);
            else
                Position += (Transform.x * delta * Speed);
            LifeSpan -= delta;
            if(LifeSpan < 0){
                QueueFree();
            }
        }
    }

    public override void SetUp(bool facedirection){
        GetNode<Sprite>("Sprite").FlipH = facedirection;
        faceDirection = facedirection;
    }

    public override void CastSpell()
    {
        throw new NotImplementedException();
    }
    public override void LoadResourcePath()
    {
        throw new NotImplementedException();
    }

    public void _on_Area2D_body_entered(object body){
        player.Play("finish");
        if(body is SlimeEnemy){
            SlimeEnemy slime = body as SlimeEnemy;
            slime.TakeDamage(DamageAmount);
        }
        
    }
}
