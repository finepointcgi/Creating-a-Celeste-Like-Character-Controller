using System;
using System.Collections.Generic;
using Godot;
public class KillQuest : Quest
{
    public int EnemyID;
    public int NumberToKill;

    public KillQuest(int id, int xp, int enemyID, int numberToKill){
        this.id = id;
        rewardXP = xp;
        EnemyID = enemyID;
        NumberToKill = numberToKill;
        
    }
    
    
}
