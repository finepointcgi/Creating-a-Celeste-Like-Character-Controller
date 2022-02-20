using System;
using System.IO;
using System.Collections.Generic;
//using Godot;
using Newtonsoft.Json;
public class QuestManager
{
    public List<int> ActiveQuests = new List<int>();
    public Dictionary<int,Quest> AvalQuests = new Dictionary<int,Quest>();
    public QuestManager(){
        KillQuest quest = new KillQuest(1, 1, finishDialouge: 
        new List<NPCDialouge>{
        new NPCDialouge(
            new List<int>{
                1,
                },"Thank you so much for defeating the enemy!",1)
        },
            enemyID: 1, 
            numberToKill:1
        );
        AvalQuests.Add(1,quest);
        SaveActiveQuests("C:/Temp/test.json");
    }

    public void LoadActiveQuests(string path){
        string json = File.ReadAllText(path);
        ActiveQuests = JsonConvert.DeserializeObject<List<int>>(json);
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

    public void SaveActiveQuests(string path){

        string json = JsonConvert.SerializeObject(AvalQuests);
        File.WriteAllText(path, json);
    }
}
