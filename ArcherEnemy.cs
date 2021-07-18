using Godot;
using System;

public class ArcherEnemy : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private PlayerController player;
    private bool active;
    private bool ableToShoot = true;
    private float shootTimer = 1f;
    private float shootTimerReset = 1f;
    [Export]
    public PackedScene Arrow;

    public bool isShooting = false;
    private AnimatedSprite animatedSprite;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (active)
        {
            var angle = GlobalPosition.AngleToPoint(player.GlobalPosition);
            if (Mathf.Abs(angle) > Mathf.Pi / 2)
            {
                animatedSprite.FlipH = false;
            }
            else
            {
                animatedSprite.FlipH = true;
            }
            if (ableToShoot)
            {
                var spaceState = GetWorld2d().DirectSpaceState;
                Godot.Collections.Dictionary result = spaceState.IntersectRay(this.Position, player.Position, new Godot.Collections.Array { this });
                if (result != null)
                {
                    if (result.Contains("collider"))
                    {
                        this.GetNode<Position2D>("ProjectileSpawn").LookAt(player.Position);
                        if (result["collider"] == player)
                        {
                            animatedSprite.Play("Drawback");
                            isShooting = true;
                        }
                    }
                }


            }
            else
            {
                if (!isShooting)
                {
                    animatedSprite.Play("Idle");
                }
            }
        }
        if (shootTimer <= 0)
        {
            ableToShoot = true;
        }
        else
        {
            shootTimer -= delta;
        }
    }
    public void _on_Detection_Radius_body_entered(object body)
    {
        GD.Print("body has entered" + body);
        if (body is PlayerController)
        {
            player = body as PlayerController;
            active = true;
        }
    }

    private void _on_Detection_Radius_body_exited(object body)
    {
        GD.Print("body has exited" + body);
        if (body is PlayerController)
        {
            active = false;
        }
    }

    private void _on_AnimatedSprite_animation_finished()
    {
        if (animatedSprite.Animation == "Drawback")
        {
            animatedSprite.Play("Shoot");
            shootAtPlayer();
        }
        if(animatedSprite.Animation == "Shoot"){
            isShooting = false;
        }
    }

    private void shootAtPlayer()
    {
        GD.Print("shooting");
        Arrow arrow = Arrow.Instance() as Arrow;
        Owner.AddChild(arrow);
        arrow.GlobalTransform = this.GetNode<Position2D>("ProjectileSpawn").GlobalTransform;
        ableToShoot = false;
        shootTimer = shootTimerReset;
    }

}
