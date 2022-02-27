using System.Collections.Generic;

public class KillQuest : Quest
{
    public int EnemyId;
    public int NumberToKill;
    public int NumberLeftToKill;

    public KillQuest(int id, int rewardXP, bool accepted = false, 
    bool completed = false, List<NPCDialouge> finishDialouge = null, 
    int enemyId = 0, int numberToKill = 0)
    {
        Id = id;
        RewardXP = rewardXP;
        Accepted = accepted;
        Completed = completed;
        FinishDialougeElement = finishDialouge;
        EnemyId = enemyId;
        NumberToKill = numberToKill;
    }

    public override void Update(object obj){
        if(obj is SlimeEnemy){
            SlimeEnemy slime = obj as SlimeEnemy;
            if(slime.id == EnemyId){
                NumberLeftToKill -= 1;
                if(NumberLeftToKill <= 0)
                    Completed = true;
                    InterfaceManager.QuestInterfaceManager.updateQuestElement(QuestElement, Title, CompletedDesc);
            }
        }
    }
}