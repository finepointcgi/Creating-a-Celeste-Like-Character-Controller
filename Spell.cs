using Godot;
using System;

public abstract class Spell : Node2D
{
    public bool faceDirection;
    [Export]
    public float DamageAmount;
    [Export]
    public float LifeSpan;
    [Export]
    public int Speed;
    [Export]
    public float ManaCost;
    public abstract void CastSpell();
    public abstract void LoadResourcePath();
    public abstract void SetUp(bool faceDirection);

}
