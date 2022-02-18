using System;
using System.Collections.Generic;
using Godot;
public abstract class Quest : Node
{
    public int id;
    public int rewardXP;
    public bool accepted;
    public bool Completed;
    public NPCDialouge FinishDialougeElement;

    

}
