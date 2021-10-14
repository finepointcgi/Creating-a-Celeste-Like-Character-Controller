using Godot;
using System;

public class InterfaceManager : CanvasLayer
{
    public static DialougeManger dialougeManger;
    public static ProgressBar StaminaBar;
    public static ProgressBar HealthBar;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        StaminaBar = GetNode("Control/Stamina") as ProgressBar;
        HealthBar = GetNode("Control/Health") as ProgressBar;
        dialougeManger = GetNode("DialougeManger") as DialougeManger;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public static void UpdateHealth(float maxHealth, float Health){
        HealthBar.Value = (Health / maxHealth) * HealthBar.MaxValue;
    }

    public static void UpdateStamina(float maxStamina, float Stamina){
        StaminaBar.Value = (Stamina / maxStamina) * StaminaBar.MaxValue;

    }


}
