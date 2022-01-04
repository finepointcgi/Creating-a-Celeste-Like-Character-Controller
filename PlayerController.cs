using Godot;
using System;
using System.Collections.Generic;
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
    public float MaxHealth = 3;
    public float Health = 3;
    private Vector2 facingDirection = new Vector2(0,0);
    private bool isTakingDamage = false;
    [Signal]
    public delegate void Death();
    private float mana = 100f;
    private float maxMana = 100f;
    private float manaTimerReset = 2f;
    private float manaTimer = 2f;
    public List<Key> Keys = new List<Key>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        GameManager.Player = this;
        InterfaceManager.UpdateHealth(MaxHealth,Health);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    
    public override void _PhysicsProcess(float delta)
    {
        InterfaceManager.UpdateMana(maxMana,mana);
        InterfaceManager.UpdateHealth(MaxHealth,Health);
        if(Health > 0 && GameManager.GlobalGameManager.GamePaused != true){
            if (!isDashing && !isWallJumping)
            {
                processMovement(delta);
            }
            if(Input.IsActionJustPressed("ui_accept")){
                if(GetNode<RayCast2D>("RaycastLeft").IsColliding()){
                    Node obj = (Node)GetNode<RayCast2D>("RaycastLeft").GetCollider();
                    interactWithItem(obj);
                }else if(GetNode<RayCast2D>("RaycastRight").IsColliding()){
                    Node obj = (Node)GetNode<RayCast2D>("RaycastRight").GetCollider();
                    interactWithItem(obj);
                }
            }

            if (IsOnFloor())
            {
                if(GetNode<RayCast2D>("RaycastDown").IsColliding() && Input.IsActionPressed("ui_down") && Input.IsActionJustPressed("jump")){
                    Position = new Vector2(Position.x, Position.y + 2);
                }else if (Input.IsActionJustPressed("jump"))
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
            // if (isDashAvailable)
            // {
            //     processDash(delta);
            // }

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
            else if (!isClimbing && !isDashing)
            {
                velocity.y += gravity * delta;
                if(velocity.y > gravity){
                    velocity.y = gravity;
                }
            }else {
                climbTimer -= delta;
                if(climbTimer <= 0){
                    isClimbing = false;
                    canClimb = false;
                    climbTimer = climbTimerReset;
                }
            }
            if(mana < 100 && manaTimer <= 0){
                UpdateMana(delta * 1);
                //GD.Print(mana);
            }else if(mana != 100){
                manaTimer -= delta * 1;
            }
            if(Input.IsActionJustPressed("attack")){
                attack();
            }
            if(Input.IsActionJustPressed("switch_spell")){
                GameManager.MagicController.CycleSpell();
            }


        MoveAndSlide(velocity, Vector2.Up);
        }
        else{
            animatedSprite.Play("Idle");
        }
    }

    private void attack(){
        GameManager.MagicController.CastSpell(GameManager.Player.GetNode<AnimatedSprite>("AnimatedSprite").FlipH);

    }
    private void interactWithItem(Node obj){
        if(obj.Owner is Pickupable){
            if(obj.Owner is MagicPotion){
                MagicPotion potion = obj.Owner as MagicPotion;
                potion.UsePotion();
            }
        }else if(obj is NPC){
            NPC npc = obj as NPC;
            npc.setNPCDialouge();
            InterfaceManager.dialougeManger.ShowDialougeElement();
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
        facingDirection = new Vector2(0,0);
        if(!isTakingDamage){
            if (Input.IsActionPressed("ui_left"))
            {
                facingDirection.x -= 1;
                animatedSprite.FlipH = true;
            }
            if (Input.IsActionPressed("ui_right"))
            {
                facingDirection.x += 1;
                animatedSprite.FlipH = false;
            }
            if (Input.IsActionPressed("ui_up"))
            {
                facingDirection.y = -1;
            }
            if (Input.IsActionPressed("ui_down"))
            {
                facingDirection.y = 1;
            }
            if(Input.IsActionJustPressed("dash")){
                if(isDashAvailable && mana >= 10){
                    isDashing = true;
                    dashTimer = dashTimerReset;
                    isDashAvailable = false;
                   UpdateMana(-10);
                    GD.Print(mana);
                    manaTimer = manaTimerReset;
                }
            }
        }
        if (facingDirection.x != 0 || facingDirection.y != 0)
        {
            if(isDashing){
                velocity.x = dashSpeed * facingDirection.x;
                velocity.y = dashSpeed * facingDirection.y;
            }else{
                velocity.x = Mathf.Lerp(velocity.x, facingDirection.x * speed, acceleration);
            }
            
            if(!isInAir && facingDirection.x != 0)
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
        if(Health > 0){
            Health -= 1;
            GD.Print("Current Health " + Health);
            InterfaceManager.UpdateHealth(MaxHealth,Health);
            velocity = MoveAndSlide(new Vector2(500f * -facingDirection.x, -80), Vector2.Up);
            isTakingDamage = true;
            animatedSprite.Play("TakeDamage");
            if(Health <= 0){
                Health = 0;
                animatedSprite.Play("Death");
                GD.Print("Player Has Died!");
            }
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
        InterfaceManager.UpdateHealth(MaxHealth,Health);
    }

    public void UpdateMana(float manaAmount){
        mana += manaAmount;
        if(mana >= maxMana){
            mana = maxMana;
        }else if(mana <= 0){
            mana = 0;
        }
    }

}
