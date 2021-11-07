using Godot;
using System;
using System.Collections.Generic;
public class SpellManager : Node
{
    public PackedScene EquipedSpells;
    public List<PackedScene> AvSpells = new List<PackedScene>();

    public void CastSpell(PackedScene spell,bool faceDirection){
        Fireball currentSpell = spell.Instance() as Fireball;
        if(GameManager.player.stamina >= currentSpell.manaCost){
            currentSpell.SetUp(faceDirection);
            currentSpell.GlobalPosition = faceDirection ? GameManager.player.GetNode<Position2D>("SpellCastLocationLeft").GlobalPosition : GameManager.player.GetNode<Position2D>("SpellCastLocationRight").GlobalPosition;
            //currentSpell.Position = GameManager.player.GetNode<Position2D>("SpellCastLocation").Position
            GameManager.GlobalGameManager.AddChild(currentSpell);
            GameManager.player.stamina -= currentSpell.manaCost;
        }
    }
    
}
