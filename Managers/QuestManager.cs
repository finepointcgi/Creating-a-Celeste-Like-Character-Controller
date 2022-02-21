using System;
using System.IO;
using System.Collections.Generic;
//using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using JsonKnownTypes;

public class QuestManager
{
    public List<int> ActiveQuests = new List<int>();
    public Dictionary<int,Quest> AvalQuests = new Dictionary<int,Quest>();
    public QuestManager(){
        
        LoadQuests("C:/Temp/");
    }

    public void LoadQuests(string path){
        var settings = new JsonSerializerSettings{ TypeNameHandling = TypeNameHandling.All };
        string json = File.ReadAllText(Path.Combine(path,"ActiveQuests.json"));
        ActiveQuests = JsonConvert.DeserializeObject<List<int>>(json);
        json = File.ReadAllText(Path.Combine(path,"Quests.json"));
        AvalQuests = JsonConvert.DeserializeObject<Dictionary<int,Quest>>(json);
    }

    public void AddActiveQuests(Quest questToAdd){
        ActiveQuests.Add(questToAdd.id); 
    }

    public void RemoveActiveQuests(int questToRemove){
        ActiveQuests.Remove(questToRemove);
    }

    public void updateQuests(object obj){
        foreach (var item in ActiveQuests)
        {
            Quest q = AvalQuests[item];
            if(q is KillQuest){
                KillQuest kq = q as KillQuest;
                SlimeEnemy slime = obj as SlimeEnemy;
                if(kq.EnemyID == slime.id){
                    q.Update(obj);
                }
            }else if(q is ItemQuest){
                q.Update(obj);
            }
        }
        
    }

    public void SaveQuests(string path){
        
        string json = JsonConvert.SerializeObject(AvalQuests);
        File.WriteAllText(Path.Combine(path,"Quests.json"), json);
        
        json = JsonConvert.SerializeObject(ActiveQuests);
        File.WriteAllText(Path.Combine(path,"ActiveQuests.json"), json);
    }

    
}
