using System;
using System.Collections.Generic;
using Godot;
public class ItemQuest : Quest
{
    public int RequiredItemID;

    public ItemQuest(int id, int rewardXP, bool accepted = false, bool Completed = false, NPCDialouge finishDialouge = null, int itemid = 0){
        this.id = id;
        this.rewardXP = rewardXP;
        this.RequiredItemID = itemid;
        this.FinishDialougeElement = finishDialouge;
    }

    public bool CheckIfItemIsCorrectItem(Pickupable item) => item.id == RequiredItemID ? true : false;
    
}
