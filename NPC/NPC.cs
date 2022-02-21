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
    public NPCInterfaceHolder npcInterface = new NPCInterfaceHolder() ;
    [Export]
    private string npcName;
    private List<int> questIndex = new List<int>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        npcName = "Bob";
        LoadNPC("C:/Temp/");
        

        foreach (NPCDialouge item in npcInterface.NPCDialouges)
        {
            if(item.QuestID != -1){
                questIndex.Add(item.QuestID);
            }
        }
        
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
        InterfaceManager.dialougeManger.npcDialouge = npcInterface.NPCDialouges;
        InterfaceManager.dialougeManger.interfaceSelectionObjects = npcInterface.InterfaceSelections;
        InterfaceManager.dialougeManger.DialougeHeader = npcName;
    }

    public void SaveNPC(string path){

        string json = JsonConvert.SerializeObject(npcInterface);
        System.IO.File.WriteAllText(System.IO.Path.Combine(path,$"NPC_{npcName}_Dialouge.json"), json);
    }

    public void LoadNPC(string path){
        string json = System.IO.File.ReadAllText(System.IO.Path.Combine(path, $"NPC_{npcName}_Dialouge.json"));
        npcInterface = JsonConvert.DeserializeObject<NPCInterfaceHolder>(json);
    }
}
