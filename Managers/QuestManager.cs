using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

public class QuestManager
{
    public List<Quest> AvalQuest = new List<Quest>();
    public List<Quest> ActiveQuests = new List<Quest>();

    public QuestManager()
    {
        // KillQuest quest = new KillQuest(1, 1, finishDialouge:
        // new List<NPCDialouge>{
        //     new NPCDialouge(
        //         new List<int>{
        //             1,
        //         }, "Thank you so Much for defeating the Enemy!" , 1
        //     ),
        // },
        // enemyId: 1,
        // numberToKill: 1
        // );
        // AvalQuest.Add(quest);
        LoadQuests();
    }
    public void AddActiveQuests(Quest questToAdd)
    {
        ActiveQuests.Add(questToAdd);
        InterfaceManager.QuestInterfaceManager.AddQuestElement(questToAdd);
    }

    public void RemoveActiveQuest(Quest questToRemove)
    {
        ActiveQuests.Remove(questToRemove);
        if(questToRemove.Completed)
            GameManager.Player.XP += questToRemove.RewardXP;
    }

    public void updateQuest(object obj)
    {
        foreach (var item in ActiveQuests)
        {
            item.Update(obj);
        }
    }

    public void SaveQuests()
    {
        string json = JsonConvert.SerializeObject(AvalQuest);
        var questFile = new File();
        questFile.Open($"user://AvalQuests.json", File.ModeFlags.Write);
        questFile.StoreLine(json);
        questFile.Close();

        json = JsonConvert.SerializeObject(ActiveQuests);
        questFile = new File();
        questFile.Open($"user://ActiveQuests.json", File.ModeFlags.Write);
        questFile.StoreLine(json);
        questFile.Close();
    }

    public void LoadQuests()
    {
        var questFile = new File();
        questFile.Open($"user://AvalQuests.json", File.ModeFlags.Read);
        string json = "";
        while (!questFile.EofReached())
        {
            json += questFile.GetLine();
        }
        AvalQuest = JsonConvert.DeserializeObject<List<Quest>>(json);
        questFile.Close();

        questFile = new File();
        questFile.Open($"user://ActiveQuests.json", File.ModeFlags.Read);
        json = "";
        while (!questFile.EofReached())
        {
            json += questFile.GetLine();
        }
        ActiveQuests = JsonConvert.DeserializeObject<List<Quest>>(json);
        questFile.Close();
    }


}