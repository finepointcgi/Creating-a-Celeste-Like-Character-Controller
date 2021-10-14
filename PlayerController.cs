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
    
    public float Health = 3;
    public float MaxHealth = 3;
    private Vector2 facingDirection = new Vector2(0,0);
    private bool isTakingDamage = false;
    [Signal]
    public delegate void Death();
    private AnimationPlayer animationPlayer;
    [Export]
    public bool canAttack = true;
    [Export]
    public bool isAttacking = false;

    private float stamina = 100f;
    private float maxStamina = 100f;
    private float staminaTimerReset = 2f;
    private float staminaTimer = 2f;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        InterfaceManager.UpdateHealth(MaxHealth,Health);

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    
    public override void _PhysicsProcess(float delta)
    {
        InterfaceManager.UpdateStamina(maxStamina, stamina);
        if(Health > 0 && GameManager.GlobalGameManager.GamePaused != true){
            if (!isDashing && !isWallJumping)
            {
                processMovement(delta);
            }
            if(Input.IsActionJustPressed("ui_accept")){
                if(GetNode<RayCast2D>("RaycastLeft").IsColliding()){
                    Node obj = (Node)GetNode<RayCast2D>("RaycastLeft").GetCollider();
                    InteractWithItem(obj);
                }else if(GetNode<RayCast2D>("RaycastRight").IsColliding()){
                    Node obj = (Node)GetNode<RayCast2D>("RaycastRight").GetCollider();
                    InteractWithItem(obj);
                }else if(GetNode<RayCast2D>("ItemPickupLeft").IsColliding()){
                    Node obj = (Node)GetNode<RayCast2D>("ItemPickupLeft").GetCollider();
                    InteractWithItem(obj);
                }else if(GetNode<RayCast2D>("ItemPickupRight").IsColliding()){
                    Node obj = (Node)GetNode<RayCast2D>("ItemPickupRight").GetCollider();
                    InteractWithItem(obj);
                }
                
            }

            if (IsOnFloor())
            {
                if(GetNode<RayCast2D>("RaycastDown").IsColliding() && Input.IsActionPressed("ui_down") && Input.IsActionJustPressed("jump")){
                    Position = new Vector2(Position.x, Position.y + 2);
                }else if (Input.IsActionJustPressed("jump"))
                {
                    
                    velocity.y = -jumpHeight;
                    
                       animationPlayer.Play("Jump");
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
                if(stamina <= 100 && staminaTimer <= 0){
                    stamina += delta * 1;
                    GD.Print(stamina);
                }else if(stamina != 100){
                    staminaTimer -= delta;
                    GD.Print(staminaTimer);
                }

            }else {
                climbTimer -= delta;
                if(climbTimer <= 0){
                    isClimbing = false;
                    canClimb = false;
                    climbTimer = climbTimerReset;
                }
            }
                if(Input.IsActionJustPressed("attack")){
                    attack();
                }
            
        //GD.Print(velocity);
        
        MoveAndSlide(velocity,Vector2.Up);
        }
    }

    private void attack(){
        animationPlayer.Play("Attack");
        canAttack = false;
        isAttacking = true;
    }
    private void InteractWithItem(Node obj){
         
         if(obj is NPC){
            showNPCDialouge(obj);
        }else if(obj is Potion){
            Potion potion = obj as Potion;
            potion.UseItem();
        }
    }

    private void showNPCDialouge(Node obj)
    {
        NPC npc = obj as NPC;
        npc.setNPCDialouge();
        InterfaceManager.dialougeManger.ShowDialougeElement();
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
            if(Input.IsActionPressed("ui_up")){
                facingDirection.y = -1;
            }
            if(Input.IsActionPressed("ui_down")){
                facingDirection.y = 1;
            }
            if(Input.IsActionJustPressed("dash")){
                if(isDashAvailable && stamina > 10){
                    isDashing = true;
                    dashTimer = dashTimerReset;
                    isDashAvailable = false;
                    UpdateStamina(-10);
                    staminaTimer = staminaTimerReset;
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
            
            if(!isInAir)
                animationPlayer.Play("Run");
        }
        else
        {
            velocity.x = Mathf.Lerp(velocity.x, 0, friction);
            if(velocity.x < 5 && velocity.x > -5){
                if(!isInAir && !isAttacking)
                    animationPlayer.Play("Idle");
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

    public async void TakeDamage(){
        GD.Print("Player Has Taken Damage");
        
        if(Health > 0){
            isTakingDamage = true;
            Health -= 1;
            animatedSprite.Play("TakeDamage");
            GD.Print("Current Health " + Health);
            velocity = new Vector2(0,0);
            //MoveAndSlide(velocity,Vector2.Up);
            await ToSignal(GetTree(), "idle_frame");
            velocity = new Vector2(500f * -facingDirection.x, -80);
            GD.Print(velocity);
            GD.Print("Play Animation");
            InterfaceManager.UpdateHealth(MaxHealth,Health);
            
            if(Health <= 0){
                Health = 0;
                animatedSprite.Play("Death");
                GD.Print("Player Has Died!");
            }
        }
        
        
    }

    private void _on_Punch_body_entered(object body){
        if(body is NPC){
            NPC npc = body as NPC;
            npc.TakeDamage(1);
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

    public void UpdateStamina(float funStamina){
        stamina += funStamina;
        if(stamina >= maxStamina){
            stamina = maxStamina;
        }else if(stamina <= 0){
            stamina = 0;
        }
    }

}
