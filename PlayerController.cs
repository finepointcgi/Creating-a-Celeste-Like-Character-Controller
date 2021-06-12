using Godot;
using System;

public class PlayerController : KinematicBody2D
{
    private int speed = 100;
    private int gravity = 200;
    private float friction = .1f;
    private float acceleration = .5f;
    private int jumpHeight = 100;
    private int dashSpeed = 500;
    private bool isDashing = false;
    private float dashTimer = .2f;
    private float dashTimerReset = .2f;
    private bool isDashAvailable = true;
    private bool isWallJumping = false;
    private float wallJumpTimer = .45f;
    private float wallJumpTimerReset = .45f;
    private Vector2 velocity = new Vector2();
    private bool canClimb = true;
    private int climbSpeed = 100;
    private bool isClimbing = false;
    private float climbTimer = 5f;
    private float climbTimerReset = 5f;
    private bool isInAir = false;
    [Export]
    public PackedScene GhostPlayerInstance;
    private AnimatedSprite animatedSprite;
    
    public int Health = 3;
    private int facingDirection = 0;
    private bool isTakingDamage = false;
    [Signal]
    public delegate void Death();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Health > 0){
            if (!isDashing && !isWallJumping)
            {
                processMovement(delta);
            }

            if (IsOnFloor())
            {
                if (Input.IsActionJustPressed("jump"))
                {
                    velocity.y = -jumpHeight;
                    animatedSprite.Play("Jump");
                    isInAir = true;
                }else{
                    isInAir = false;
                }
                canClimb = true;
                isDashAvailable = true;
            }

            if (canClimb)
            {
                processClimb(delta);
            }

            if(!IsOnFloor()){
                processWallJump(delta);
            }
            if (isDashAvailable)
            {
                processDash(delta);
            }

            if (isDashing)
            {
                dashTimer -= delta;
                GhostPlayer ghost = GhostPlayerInstance.Instance() as GhostPlayer;
                Owner.AddChild(ghost);
                ghost.GlobalPosition = this.GlobalPosition;
                ghost.SetHValue(animatedSprite.FlipH);

                if (dashTimer <= 0)
                {
                    isDashing = false;
                    velocity = new Vector2(0, 0);
                }
            }
            else if (!isClimbing)
            {
                velocity.y += gravity * delta;
            }else {
                climbTimer -= delta;
                if(climbTimer <= 0){
                    isClimbing = false;
                    canClimb = false;
                    climbTimer = climbTimerReset;
                }
            }


        MoveAndSlide(velocity, Vector2.Up);
        }
    }

    private void processClimb(float delta)
    {
        if (Input.IsActionPressed("climb") && (GetNode<RayCast2D>("RaycastLeft").IsColliding() || GetNode<RayCast2D>("RaycastRight").IsColliding() ||
        GetNode<RayCast2D>("RaycastRightClimb").IsColliding() || GetNode<RayCast2D>("RaycastLeftClimb").IsColliding()))
        {
            
            if (canClimb && !isWallJumping)
            {
                isClimbing = true;
                if (Input.IsActionPressed("ui_up"))
                {
                    velocity.y = -climbSpeed;
                }
                else if (Input.IsActionPressed("ui_down"))
                {
                    velocity.y = climbSpeed;
                }
                else
                {
                    velocity = new Vector2(0, 0);
                }
            }
            else
            {
                isClimbing = false;
            }
        }
        else
        {
            isClimbing = false;
        }
    }

    private void processMovement(float delta)
    {
        facingDirection = 0;
        if(!isTakingDamage){
            if (Input.IsActionPressed("ui_left"))
            {
                facingDirection -= 1;
                animatedSprite.FlipH = true;
            }
            if (Input.IsActionPressed("ui_right"))
            {
                facingDirection += 1;
                animatedSprite.FlipH = false;
            }
        }
        if (facingDirection != 0)
        {
            velocity.x = Mathf.Lerp(velocity.x, facingDirection * speed, acceleration);
            
            if(!isInAir)
                animatedSprite.Play("Run");
        }
        else
        {
            velocity.x = Mathf.Lerp(velocity.x, 0, friction);
            if(velocity.x < 5 && velocity.x > -5){
                if(!isInAir)
                    animatedSprite.Play("Idle");
                isTakingDamage = false;
            }
        }
    }
    private void processDash(float delta)
    {
        if (Input.IsActionJustPressed("dash"))
        {
            if (Input.IsActionPressed("ui_left"))
            {
                velocity.x = -dashSpeed;
                isDashing = true;
            }
            if (Input.IsActionPressed("ui_right"))
            {
                velocity.x = dashSpeed;
                isDashing = true;
            }
            if (Input.IsActionPressed("ui_up"))
            {
                velocity.y = -dashSpeed;
                isDashing = true;
            }
            if (Input.IsActionPressed("ui_right") && Input.IsActionPressed("ui_up"))
            {
                velocity.x = dashSpeed;
                velocity.y = -dashSpeed;
                isDashing = true;
            }
            if (Input.IsActionPressed("ui_left") && Input.IsActionPressed("ui_up"))
            {
                velocity.x = -dashSpeed;
                velocity.y = -dashSpeed;
                isDashing = true;
            }

            dashTimer = dashTimerReset;
            isDashAvailable = false;
        }
    }
    private void processWallJump(float delta)
    {
        if (Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RaycastLeft").IsColliding())
        {
            velocity.y = -jumpHeight;
            velocity.x = jumpHeight;
            isWallJumping = true;
            animatedSprite.FlipH = false;

        }
        else if (Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RaycastRight").IsColliding())
        {
            velocity.y = -jumpHeight;
            velocity.x = -jumpHeight;
            isWallJumping = true;
            animatedSprite.FlipH = true;
        }
        if (isWallJumping)
        {
            wallJumpTimer -= delta;
            if (wallJumpTimer <= 0)
            {
                isWallJumping = false;
                wallJumpTimer = wallJumpTimerReset;
            }
        }
    }

    public void TakeDamage(){
        GD.Print("Player Has Taken Damage");
        Health -= 1;
        GD.Print("Current Health " + Health);
        velocity = MoveAndSlide(new Vector2(500f * -facingDirection, -80), Vector2.Up);
        isTakingDamage = true;
        animatedSprite.Play("TakeDamage");
        if(Health <= 0){
            Health = 0;
            animatedSprite.Play("Death");
            GD.Print("Player Has Died!");
        }
    }

    private void _on_AnimatedSprite_animation_finished(){
        if(animatedSprite.Animation == "Death"){
            animatedSprite.Stop();
            Hide();
            GD.Print("Animation Finished");
            EmitSignal(nameof(Death));
        }
    }

    public void RespawnPlayer(){
        Show();
        Health = 3;
    }


}
