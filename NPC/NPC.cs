using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
public class NPC : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private List<NPCDialouge> npcDialouge;
    public NPCInterfaceHolder npcInterface = new NPCInterfaceHolder() ;
    [Export]
    private string npcName;
    public int index = 0;
    private List<int> questIndex = new List<int>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        InterfaceSelectionObject obj = new InterfaceSelectionObject(1, 1, "I will help", true);
        InterfaceSelectionObject obj2 = new InterfaceSelectionObject(2, 2, "I cant help");
        InterfaceSelectionObject obj3 = new InterfaceSelectionObject(3, -1, "ok");
        npcInterface.InterfaceSelections.Add(obj);
        npcInterface.InterfaceSelections.Add(obj2);
        npcInterface.InterfaceSelections.Add(obj3);

        npcDialouge = new List<NPCDialouge>{
            new NPCDialouge(new List<int>(){1,2}, "HELP I NEED YOU TO HELP ME! The slimes they got my daugher! I need you to hunt a slime for me!", 
            0, null, 1),
            new NPCDialouge(new List<int>(){3}, "Let me know when you have killed a slime! Thank you so much for your help!", 1),
            new NPCDialouge(new List<int>(){3}, "Please help me", 2)
        };
        npcInterface.NPCDialouges = npcDialouge;
        npcName = "Bob";

        foreach (NPCDialouge item in npcDialouge)
        {
            if(item.QuestID != -1){
                questIndex.Add(item.QuestID);
            }
        }
        SaveNPC("C:/Temp/");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void setNPCDialouge(){
        
    }

    public void InteractWithNPC(){
        var manager = GameManager.QuestManager.ActiveQuests;
        List<int> activeQuests = GameManager.QuestManager.ActiveQuests.Intersect(questIndex).ToList();
        foreach (var item in activeQuests)
        {
            if(GameManager.QuestManager.AvalQuests[item].Completed){
                InterfaceManager.dialougeManger.npcDialouge = GameManager.QuestManager.AvalQuests[item].FinishDialougeElement;
                GameManager.QuestManager.RemoveActiveQuests(item);
                GameManager.QuestManager.AvalQuests[item].Completed = true;
                return;
            }
        }
        InterfaceManager.dialougeManger.npcDialouge = npcDialouge;
        InterfaceManager.dialougeManger.interfaceSelectionObjects = npcInterface.InterfaceSelections;
        InterfaceManager.dialougeManger.DialougeHeader = npcName;
    }

    public void SaveNPC(string path){

        string json = JsonConvert.SerializeObject(npcInterface);
        System.IO.File.WriteAllText(System.IO.Path.Combine(path,$"NPC_{npcName}_Dialouge.json"), json);
    }
}
