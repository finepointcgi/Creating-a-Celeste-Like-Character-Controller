using Godot;
using System;

public class InterfaceManager : CanvasLayer
{
    public static DialougeManger dialougeManger;
    public static ProgressBar ManaBar;
    public static ProgressBar HealthBar;
    public static TextureRect SpellTextureRect;
    public static QuestInterfaceManager QuestInterfaceManager;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        dialougeManger = GetNode("DialougeManger") as DialougeManger;
        ManaBar = GetNode("MainInterface/ManaBar") as ProgressBar;
        HealthBar = GetNode("MainInterface/HealthBar") as ProgressBar;
        SpellTextureRect = GetNode("MainInterface/MagicSpellPannel/MagicSpellTexture") as TextureRect;
        QuestInterfaceManager = GetNode("QuestInterfaceManager") as QuestInterfaceManager;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public static void UpdateHealth(float maxHealth, float health){
        HealthBar.Value = (health / maxHealth) * HealthBar.MaxValue;
    } 

    public static void UpdateMana(float maxMana, float mana){
        ManaBar.Value = (mana / maxMana) * ManaBar.MaxValue;
    }

    public static void SetSpellSprite(Texture spellImage){
        SpellTextureRect.Texture = spellImage;
    }
}
