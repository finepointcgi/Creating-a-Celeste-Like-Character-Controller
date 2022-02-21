using System;
using System.Collections.Generic;
using Godot;
using JsonKnownTypes;

[JsonKnownThisType("KillQuest")]
public class KillQuest : Quest
{
    public int EnemyID;
    public int NumberToKill;
    public int numberLeftToKill;
   
    public KillQuest(int id, int rewardXP, bool accepted = false, bool Completed = false, List<NPCDialouge> finishDialouge = null, int enemyID = 0, int numberToKill = 0){
        this.id = id;
        this.rewardXP = rewardXP;
        EnemyID = enemyID;
        NumberToKill = numberToKill;
        this.FinishDialougeElement = finishDialouge;
        numberLeftToKill = NumberToKill;
    }
    
    public override void Update(object obj){
        if(obj is SlimeEnemy){
            numberLeftToKill -= 1;
            if(numberLeftToKill <= 0){
                this.Completed = true;
                InterfaceManager.QuestMenu.UpdateQuestElement(InterfaceElement, QuestName, CompletedDescription);
            }else{
                Description = Description.Replace("{number}", numberLeftToKill.ToString());
                InterfaceManager.QuestMenu.UpdateQuestElement(InterfaceElement, QuestName,Description);
            }
        }
    }
    public override string GetDescription() => Description.Replace("{number}", numberLeftToKill.ToString());
}
