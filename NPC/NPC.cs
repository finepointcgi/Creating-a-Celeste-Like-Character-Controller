using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public class NPC : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public NPCInterfaceHolder nPCInterface = new NPCInterfaceHolder();
    private List<NPCDialouge> npcDialouge;
    [Export]
    private string npcName;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // InterfaceSelectionObject obj = new InterfaceSelectionObject(1, 1, "I will help", true);
        // InterfaceSelectionObject obj2 = new InterfaceSelectionObject(2, 2, "I cant help");
        // InterfaceSelectionObject obj3 = new InterfaceSelectionObject(3, -1, "ok");
        // nPCInterface.InterfaceSelections.Add(obj);
        // nPCInterface.InterfaceSelections.Add(obj2);
        // nPCInterface.InterfaceSelections.Add(obj3);


        // nPCInterface.nPCDialouges = new List<NPCDialouge>{
        //     new NPCDialouge(new List<int>(){1,2}, "HELP I NEED YOU TO HELP ME", 0, quest: 1),
        //     new NPCDialouge(new List<int>(){3}, "Thank you so much for your help!", 1),
        //     new NPCDialouge(new List<int>(){3}, "Please help me", 2)
        // };

        
        //SaveNPCDialouge();
        LoadNPCDialouge();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void setNPCDialouge(){
        List<Quest> activeQuest = GameManager.QuestManager.ActiveQuests;
        InterfaceManager.dialougeManger.DialougeHeader = npcName;
        foreach (var item in activeQuest)
        {
            if(item.Completed){
                if(nPCInterface.nPCDialouges.Any(x => x.Quest == item.Id)){
                    InterfaceManager.dialougeManger.npcDialouge = item.FinishDialougeElement;
                    GameManager.QuestManager.RemoveActiveQuest(item);
                    item.QuestElement.QueueFree();
                
                    return;
                }
            }
        }
        InterfaceManager.dialougeManger.npcDialouge = nPCInterface.nPCDialouges;
        InterfaceManager.dialougeManger.InterfaceSelectionObjects = nPCInterface.InterfaceSelections;
    }

    public void SaveNPCDialouge(){
        string json = JsonConvert.SerializeObject(nPCInterface);
        var npcDialougeFile = new File();
        npcDialougeFile.Open($"user://NPC{npcName}Dialouge.json", File.ModeFlags.Write);
        npcDialougeFile.StoreLine(json);
        npcDialougeFile.Close();
    }

    public void LoadNPCDialouge(){
        var npcDialougeFile = new File();
        npcDialougeFile.Open($"user://NPC{npcName}Dialouge.json", File.ModeFlags.Read);
        string json = "";
        while(!npcDialougeFile.EofReached()){
            json += npcDialougeFile.GetLine();
        }
        nPCInterface = JsonConvert.DeserializeObject<NPCInterfaceHolder>(json);
        npcDialougeFile.Close();
    }
}
