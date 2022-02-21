using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using JsonKnownTypes;

[JsonConverter(typeof(JsonKnownTypesConverter<Quest>))]
public abstract class Quest
{
    public int id;
    public int rewardXP;
    public bool accepted;
    public bool Completed;
    public string QuestName;
    public string Description;
    public string CompletedDescription;
    public List<NPCDialouge> FinishDialougeElement;
    [JsonIgnore]
    public QuestElement InterfaceElement;

    public abstract void Update(object obj);
    public abstract string GetDescription();
}
