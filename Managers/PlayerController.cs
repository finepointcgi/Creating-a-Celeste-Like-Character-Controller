using Godot;
using System;
using System.Collections.Generic;
public class PlayerController : KinematicBody2D
{
    enum PlayerState
    {
        Idle,
        running,
        attacking,
        climbing,
        dashing,
        takingdamage,
        dead,
        walljumpping
    }
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
    private float wallJumpDistance = 75;
    private float wallJumpTimer = .45f;
    private float wallJumpTimerReset = .45f;
    private Vector2 velocity = new Vector2();
    private bool canClimb = true;
    private int climbSpeed = 100;
    private float climbTimer = 5f;
    private float climbTimerReset = 5f;
    [Export]
    public PackedScene GhostPlayerInstance;
    private AnimatedSprite animatedSprite;
    public float MaxHealth = 3;
    public float Health = 3;
    private Vector2 facingDirection = new Vector2(0, 0);
    private bool isTakingDamage = false;
    [Signal]
    public delegate void Death();
    private float mana = 100f;
    private float maxMana = 100f;
    private float manaTimerReset = 2f;
    private float manaTimer = 2f;
    public List<Key> Keys = new List<Key>();
    private PlayerState currentState;
    private bool jumping;
    public bool pauseInput;
    private float damageTimer = .3f;
    private float damageTimerreset = .3f;
    public float XP = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        GameManager.Player = this;
        InterfaceManager.UpdateHealth(MaxHealth, Health);
        currentState = PlayerState.Idle;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _PhysicsProcess(float delta)
    {
        if (!pauseInput && currentState != PlayerState.takingdamage){
            inputManager();
            processGravity(delta);
            }
        switch (currentState)
        {
            case PlayerState.Idle:
                idle(delta);
                break;
            case PlayerState.attacking:
                attack();
                break;
            case PlayerState.climbing:
                processClimb(delta);
                break;
            case PlayerState.running:
                processMovement(delta);
                break;
            case PlayerState.dashing:
                processDash();
                break;
            case PlayerState.takingdamage:
            case PlayerState.dead:
                break;
            case PlayerState.walljumpping:
                processWallJump(delta);
                break;
            default:
                break;
        }
        if (IsOnFloor())
        {
            canClimb = true;
        }
        InterfaceManager.UpdateMana(maxMana, mana);
        InterfaceManager.UpdateHealth(MaxHealth, Health);
        processTimers(delta);
        
        //GD.Print(velocity);
        MoveAndSlide(velocity, Vector2.Up,false,4,0.785398f,true);
    }

    private void inputManager()
    {
        facingDirection = new Vector2(0, 0);

        if (Input.IsActionJustPressed("attack"))
        {
            attack();
        }
        if (Input.IsActionJustPressed("switch_spell"))
        {
            GameManager.MagicController.CycleSpell();
        }
        if (Input.IsActionJustPressed("ui_accept"))
        {
            interact();
        }
        if (Input.IsActionPressed("ui_left"))
        {
            facingDirection.x = -1;
            animatedSprite.FlipH = true;
            currentState = PlayerState.running;
        }
        if (Input.IsActionPressed("ui_right"))
        {
            facingDirection.x = 1;
            animatedSprite.FlipH = false;
            currentState = PlayerState.running;
        }
        if (Input.IsActionPressed("ui_up"))
        {
            facingDirection.y = -1;
            //currentState = PlayerState.running;
        }
        if (Input.IsActionPressed("ui_down"))
        {
            facingDirection.y = 1;
            //currentState = PlayerState.running;
        }
        if (Input.IsActionJustPressed("dash"))
        {
            currentState = PlayerState.dashing;
        }
        if (Input.IsActionJustPressed("jump"))
        {
            if (IsOnFloor())
            {
                jumping = true;
            }
            Node obj = GetNode<RayCast2D>("RaycastLeft").IsColliding()
            ? (Node)GetNode<RayCast2D>("RaycastLeft").GetCollider() : GetNode<RayCast2D>("RaycastRight").IsColliding()
            ? (Node)GetNode<RayCast2D>("RaycastRight").GetCollider() : null;
            if (obj == null) return;

            if (obj is TileMap)
                currentState = PlayerState.walljumpping;

        }
        if (Input.IsActionPressed("climb") && canClimb && (GetNode<RayCast2D>("RaycastLeft").IsColliding() || GetNode<RayCast2D>("RaycastRight").IsColliding() ||
        GetNode<RayCast2D>("RaycastRightClimb").IsColliding() || GetNode<RayCast2D>("RaycastLeftClimb").IsColliding()))
        {
            currentState = PlayerState.climbing;
        }
    }

    private void processTimers(float delta)
    {
        if (isWallJumping)
        {
            wallJumpTimer -= delta;
            if (wallJumpTimer <= 0)
            {
                isWallJumping = false;
                wallJumpTimer = wallJumpTimerReset;
                pauseInput = false;
                currentState = PlayerState.Idle;

            }
        }
        else if (isDashing)
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
                pauseInput = false;
                isDashAvailable = true;
                currentState = PlayerState.Idle;
            }
        }
        else if (isTakingDamage)
        {
            damageTimer -= delta;
            if (damageTimer <= 0)
            {
                CollisionShape2D collision = (CollisionShape2D)GetNode("CollisionShape2D");
                collision.SetDeferred("disabled", false);
                isTakingDamage = false;
                pauseInput = false;
                damageTimer = damageTimerreset;
                currentState = PlayerState.Idle;
            }
        }

        if (mana < 100 && manaTimer <= 0)
        {
            UpdateMana(delta * 1);
            //GD.Print(mana);
        }
        else if (mana != 100)
        {
            manaTimer -= delta * 1;
        }
    }
    private void attack()
    {

        GameManager.MagicController.CastSpell(GameManager.Player.GetNode<AnimatedSprite>("AnimatedSprite").FlipH, mana);
        manaTimer = manaTimerReset;
        currentState = PlayerState.Idle;
    }
    private void interact()
    {
        Node obj = GetNode<RayCast2D>("RaycastLeft").IsColliding()
        ? (Node)GetNode<RayCast2D>("RaycastLeft").GetCollider() : GetNode<RayCast2D>("RaycastRight").IsColliding()
        ? (Node)GetNode<RayCast2D>("RaycastRight").GetCollider() : null;

        if (obj is null)
            return;

        if (obj.Owner is Pickupable)
        {
            if (obj.Owner is MagicPotion)
            {
                MagicPotion potion = obj.Owner as MagicPotion;
                potion.UsePotion();
            }
        }
        else if (obj is NPC)
        {
            NPC npc = obj as NPC;
            npc.setNPCDialouge();
            InterfaceManager.dialougeManger.ShowDialougeElement();
        }
    }
    private void processClimb(float delta)
    {
        velocity.y = climbSpeed * facingDirection.y;
        climbTimer -= delta;
        if (climbTimer <= 0)
        {
            canClimb = false;
            climbTimer = climbTimerReset;
            currentState = PlayerState.Idle;
        }

    }
    private void processGravity(float delta)
    {
        velocity.y += gravity * delta;
        if (velocity.y > gravity)
        {
            velocity.y = gravity;
        }
    }

    private void processMovement(float delta)
    {
        processJump();
        velocity.x = Mathf.Lerp(velocity.x, facingDirection.x * speed, acceleration);
        if (Mathf.Abs(velocity.x) > speed)
        {
            velocity.x = speed * facingDirection.x;
        }

        animatedSprite.Play("Run");
        currentState = PlayerState.Idle;
    }
    private void processJump()
    {
        if (jumping == true && IsOnFloor() == true)
        {
            jumping = false;
            velocity.y = -jumpHeight;
        }
    }
    private void idle(float delta)
    {
        processJump();
        if (velocity.x < 5 && velocity.x > -5)
        {
            animatedSprite.Play("Idle");
        }
        velocity.x = Mathf.Lerp(velocity.x, 0, friction);
    }

    private void processWallJump(float delta)
    {
        if (!isWallJumping)
        {
            pauseInput = true;
            if (animatedSprite.FlipH)
            {
                velocity.y = -wallJumpDistance;
                velocity.x = wallJumpDistance;
                isWallJumping = true;
                animatedSprite.FlipH = false;

            }
            else
            {
                velocity.y = -wallJumpDistance;
                velocity.x = -wallJumpDistance;
                isWallJumping = true;
                animatedSprite.FlipH = true;
            }
        }

    }

    private void processDash()
    {
        if (isDashAvailable && mana >= 10)
        {
            isDashing = true;
            dashTimer = dashTimerReset;
            isDashAvailable = false;
            UpdateMana(-10);
            //GD.Print(mana);
            manaTimer = manaTimerReset;
            pauseInput = true;
            velocity.x = dashSpeed * facingDirection.x;
            velocity.y = dashSpeed * facingDirection.y;
        }

    }
    public void TakeDamage()
    {
        GD.Print("Player Has Taken Damage");
        if (!isTakingDamage)
        {
            if (Health > 0)
            {
                currentState = PlayerState.takingdamage;
                Health -= 1;
                GD.Print("Current Health " + Health);
                InterfaceManager.UpdateHealth(MaxHealth, Health);
                GD.Print("start damaged " + velocity);
                if(animatedSprite.FlipH)
                    facingDirection.x = -1;
                else
                    facingDirection.x = 1;
                velocity = new Vector2(100f * -facingDirection.x, -20);
                GD.Print("end damaged " + velocity);
                isTakingDamage = true;
                pauseInput = true;
                animatedSprite.Play("TakeDamage");
                if (Health <= 0)
                {
                    Health = 0;
                    animatedSprite.Play("Death");
                    GD.Print("Player Has Died!");
                    currentState = PlayerState.dead;
                }
            }
        }
    }

    private void _on_AnimatedSprite_animation_finished()
    {
        if (animatedSprite.Animation == "Death")
        {
            animatedSprite.Stop();
            Hide();
            GD.Print("Animation Finished");
            EmitSignal(nameof(Death));
        }
    }

    public void RespawnPlayer()
    {
        Show();
        Health = 3;
        InterfaceManager.UpdateHealth(MaxHealth, Health);
    }

    public void UpdateMana(float manaAmount)
    {
        mana += manaAmount;
        if (mana >= maxMana)
        {
            mana = maxMana;
        }
        else if (mana <= 0)
        {
            mana = 0;
        }
    }

}
