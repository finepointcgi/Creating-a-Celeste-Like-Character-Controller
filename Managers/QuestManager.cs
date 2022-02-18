using System;
using System.IO;
using System.Collections.Generic;
//using Godot;
using Newtonsoft.Json;
public class QuestManager
{
    public List<Quest> ActiveQuests;
    public List<Quest> AvalQuests;
    public QuestManager(){
        KillQuest quest = new KillQuest(1,1, finishDialouge: 
        new NPCDialouge(
            new List<InterfaceSelectionObject>{
                new InterfaceSelectionObject(1, "ok")
                },"Thank you so much for defeating the enemy!",1),
            enemyID:1, 
            numberToKill:1
        );
        AvalQuests.Add(quest);
    }

    public void LoadActiveQuests(string path){
        string json = File.ReadAllText(path);
        ActiveQuests = JsonConvert.DeserializeObject<List<Quest>>(json);
    }

    public void AddActiveQuests(Quest questToAdd){
        ActiveQuests.Add(questToAdd);
        
    }

    public void RemoveActiveQuests(Quest questToRemove){
        ActiveQuests.Remove(questToRemove);
    }

    public void updateQuest(){
        
    }

    public void SaveActiveQuests(string path){
        string json = JsonConvert.SerializeObject(path);
        File.WriteAllText(path, json);
    }
}
