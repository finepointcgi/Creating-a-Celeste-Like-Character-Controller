using System.Collections.Generic;
using JsonKnownTypes;
using Newtonsoft.Json;

[JsonConverter(typeof(JsonKnownTypesConverter<Quest>))]
public abstract class Quest
{
    public int Id;
    public int RewardXP;
    public bool Accepted;
    public string Title;
    public string Desc;
    public bool Completed;
    public string CompletedDesc;
    public QuestElement QuestElement;
    public List<NPCDialouge> FinishDialougeElement;

    public abstract void Update(object obj);
    
}