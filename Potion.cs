using Godot;
using System;

public class Potion : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public float StaminaGainAmmount = 20;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Idle");
    }

    private void _on_Area2D_body_entered(object body){
        if(body is PlayerController){
            GetNode<RichTextLabel>("Sprite/RichTextLabel").Show();
        }
    }

    private void _on_Area2D_body_exited(object body){
        if(body is PlayerController){
            GetNode<RichTextLabel>("Sprite/RichTextLabel").Hide();
        }
    }

    public void UseItem(){
        GameManager.player.UpdateStamina(StaminaGainAmmount);
        QueueFree();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
