using Godot;
using System;
using System.Collections.Generic;
public class NPC : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private List<Quest> npcQuests;
    private List<NPCDialouge> npcDialouge;
    private string npcName;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        InterfaceSelectionObject obj = new InterfaceSelectionObject(1, "I will help", true);
        InterfaceSelectionObject obj2 = new InterfaceSelectionObject(2, "I cant help");
        InterfaceSelectionObject obj3 = new InterfaceSelectionObject(-1, "ok");

        
        npcQuests = new List<Quest>{
            new KillQuest(1, 100, 1, 1)
        };

        npcDialouge = new List<NPCDialouge>{
            new NPCDialouge(new List<InterfaceSelectionObject>(){obj,obj2}, "HELP I NEED YOU TO HELP ME! The slimes they got my daugher! I need you to hunt a slime for me!", 0, null, npcQuests[0]),
            new NPCDialouge(new List<InterfaceSelectionObject>(){obj3}, "Let me know when you have killed a slime! Thank you so much for your help!", 1),
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
}
