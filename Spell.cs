using Godot;
using System;

public abstract class Spell : Node2D
{
    private string resourcePath;
    public PackedScene resource;
    [Export]
    public int damageAmount;
    private int speed;
    [Export]
    public float manaCost;
    public abstract void castSpell();

    public abstract void loadResourcePath();

    public abstract void SetUp(bool facedirection);
}
