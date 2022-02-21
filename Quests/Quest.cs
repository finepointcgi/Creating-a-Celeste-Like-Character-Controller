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
    public List<NPCDialouge> FinishDialougeElement;

    public abstract void Update(object obj);
}
