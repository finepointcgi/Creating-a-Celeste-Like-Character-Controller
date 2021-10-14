using Godot;
using System;
using System.Collections.Generic;
public class NPC : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private List<NPCDialouge> npcDialouge;
    private string npcName;
    private int health = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        InterfaceSelectionObject obj = new InterfaceSelectionObject(1, "I will help");
        InterfaceSelectionObject obj2 = new InterfaceSelectionObject(2, "I cant help");
        InterfaceSelectionObject obj3 = new InterfaceSelectionObject(-1, "ok");
        npcDialouge = new List<NPCDialouge>{
            new NPCDialouge(new List<InterfaceSelectionObject>(){obj,obj2}, "HELP I NEED YOU TO HELP ME", 0),
            new NPCDialouge(new List<InterfaceSelectionObject>(){obj3}, "Thank you so much for your help!", 1),
            new NPCDialouge(new List<InterfaceSelectionObject>(){obj3}, "Please help me", 2)
        };
        npcName = "Bob";
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void setNPCDialouge(){
        InterfaceManager.dialougeManger.npcDialouge = npcDialouge;
        InterfaceManager.dialougeManger.DialougeHeader = npcName;
    }

    public void TakeDamage(int amount){
        health -= amount;
        QueueFree();
    }
}
