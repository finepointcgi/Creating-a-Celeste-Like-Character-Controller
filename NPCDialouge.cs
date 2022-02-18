using System;
using System.Collections.Generic;
public class NPCDialouge
{
    public int Index;
    public List<InterfaceSelectionObject> InterfaceSelectionObjects;
    public List<NPCDialouge> NPCDialouges;
    public string DisplayText;
    public Quest Quest;

    public NPCDialouge(List<InterfaceSelectionObject> interfaceSelectionObjects, string displayText, int index, List<NPCDialouge> dialouges = null, Quest quest = null){
        InterfaceSelectionObjects = interfaceSelectionObjects;
        DisplayText = displayText;
        Index = index;
        if(dialouges != null){
            NPCDialouges = dialouges;
        }
        if(quest != null){
            Quest = quest;
        }
    }
}
