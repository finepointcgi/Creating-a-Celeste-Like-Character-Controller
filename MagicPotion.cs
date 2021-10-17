using Godot;
using System;

public class MagicPotion : Pickupable
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public float MagicGainAmount = 20f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Bounce");
    }

    public void _on_Area2D_body_entered(object body){
        if(body is PlayerController){
            GetNode<RichTextLabel>("Node2D/RichTextLabel").Show();
        }
    }

    public void _on_Area2D_body_exited(object body){
        if(body is PlayerController){
            GetNode<RichTextLabel>("Node2D/RichTextLabel").Hide();
        }
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void UsePotion(){
        GameManager.Player.UpdateMana(MagicGainAmount);
        QueueFree();
    }
}
