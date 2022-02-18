using System;
using System.Collections.Generic;
using Godot;
public class KillQuest : Quest
{
    public int EnemyID;
    public int NumberToKill;

    public KillQuest(int id, int rewardXP, bool accepted = false, bool Completed = false, NPCDialouge finishDialouge = null, int enemyID = 0, int numberToKill = 0){
        this.id = id;
        this.rewardXP = rewardXP;
        EnemyID = enemyID;
        NumberToKill = numberToKill;
        this.FinishDialougeElement = finishDialouge;
    }
    
    
}
