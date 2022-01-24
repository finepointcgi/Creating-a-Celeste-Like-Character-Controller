using Godot;
using System;
using System.Collections.Generic;
public class MagicController : Node
{
    public Spell EquippedSpell;
    public List<Spell> AvSpells = new List<Spell>();
    private int currentCount;
    // Called when the node enters the scene tree for the first time.
    public MagicController()
    {
        IceKnife iceKnife = new IceKnife();
        iceKnife.SpellScene = ResourceLoader.Load(iceKnife.ResourcePath) as PackedScene;
        AvSpells.Add(iceKnife);
        HealingSpell healingSpell = new HealingSpell();
        healingSpell.SpellScene = ResourceLoader.Load(healingSpell.ResourcePath) as PackedScene;
        AvSpells.Add(healingSpell);
        FireBall fireBall = new FireBall();
        fireBall.SpellScene = ResourceLoader.Load(fireBall.ResourcePath) as PackedScene;
        AvSpells.Add(fireBall);
        EquippedSpell = AvSpells[2];
        InterfaceManager.SetSpellSprite(EquippedSpell.InterfaceTexture);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    public void CastSpell(bool faceDirection, float mana){
        
        Spell currentSpell = EquippedSpell.SpellScene.Instance() as Spell;  
        if(currentSpell.ManaCost <= mana){  
            currentSpell.SetUp(faceDirection);
            if(faceDirection)
                currentSpell.GlobalPosition = GameManager.Player.GetNode<Position2D>("SpellCastLeft").GlobalPosition;
            else
                currentSpell.GlobalPosition = GameManager.Player.GetNode<Position2D>("SpellCastRight").GlobalPosition;
            GameManager.GlobalGameManager.AddChild(currentSpell);
            GameManager.Player.UpdateMana(-currentSpell.ManaCost);    
        }else{
            currentSpell.QueueFree();
        }
    }

    public void CycleSpell(){
        currentCount += 1;
        if(AvSpells.Count - 1 < currentCount){
           currentCount = 0;
        }
        EquippedSpell = AvSpells[currentCount];
        InterfaceManager.SetSpellSprite(EquippedSpell.InterfaceTexture);
    }
}
