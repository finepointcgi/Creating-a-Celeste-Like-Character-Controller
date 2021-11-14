using Godot;
using System;
using System.Collections.Generic;
public class SpellManager : Node
{
    public PackedScene EquipedSpells = ResourceLoader.Load("res://Spells/Fireball.tscn") as PackedScene;
    public List<PackedScene> AvSpells = new List<PackedScene>();

    public void CastSpell(bool faceDirection){
        Spell currentSpell = EquipedSpells.Instance() as Spell;
        if(GameManager.player.stamina >= currentSpell.manaCost){
            currentSpell.SetUp(faceDirection);
            if(faceDirection){
                currentSpell.GlobalPosition = GameManager.player.GetNode<Position2D>("SpellCastLocationLeft").GlobalPosition;
            }else
                currentSpell.GlobalPosition = GameManager.player.GetNode<Position2D>("SpellCastLocationRight").GlobalPosition;
            //currentSpell.Position = GameManager.player.GetNode<Position2D>("SpellCastLocation").Position
            GameManager.GlobalGameManager.AddChild(currentSpell);
            GameManager.player.stamina -= currentSpell.manaCost;
        }
    }
    
}
