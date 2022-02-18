using System;
using System.Collections.Generic;
using Godot;
public class ItemQuest : Quest
{
    public int RequiredItemID;

    public ItemQuest(int id, int xp, int itemid){
        this.id = id;
        rewardXP = xp;
        RequiredItemID = itemid;
    }
    public bool CheckIfItemIsCorrectItem(Pickupable item) => item.id == RequiredItemID ? true : false;
    
}
