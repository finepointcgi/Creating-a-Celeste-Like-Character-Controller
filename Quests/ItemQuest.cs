using System;
using System.Collections.Generic;
using Godot;
using JsonKnownTypes;

[JsonKnownThisType("ItemQuest")]
public class ItemQuest : Quest
{
    public int RequiredItemID;
    public int NumberToCollect;
    public int NumberLeftToCollect;

    public ItemQuest(int id, int rewardXP, bool accepted = false, bool Completed = false, List<NPCDialouge> finishDialouge = null, int itemid = 0){
        this.id = id;
        this.rewardXP = rewardXP;
        this.RequiredItemID = itemid;
        this.FinishDialougeElement = finishDialouge;
    }

    public bool CheckIfItemIsCorrectItem(Pickupable item) => item.id == RequiredItemID ? true : false;

    public override string GetDescription() => Description;

    public override void Update(object obj){
        if(obj is Pickupable){
            Pickupable p = obj as Pickupable;
            if(p.id == RequiredItemID){
                NumberLeftToCollect -= 1;
                if(NumberLeftToCollect <= 0){
                    this.Completed = true;
                    if(CompletedDescription != ""){
                        InterfaceManager.QuestMenu.UpdateQuestElement(InterfaceElement, QuestName, CompletedDescription);
                    }else{
                        GameManager.QuestManager.RemoveActiveQuests(id);
                        GameManager.QuestManager.AvalQuests[id].InterfaceElement.QueueFree();
                    }
                    
                } else{
                    Description = Description.Replace("{number}", NumberLeftToCollect.ToString());
                    InterfaceManager.QuestMenu.UpdateQuestElement(InterfaceElement, QuestName,Description);
                }
            }
        }
    }

    
}
