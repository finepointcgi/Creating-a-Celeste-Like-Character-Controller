using System;
using System.Collections.Generic;
public class NPCDialouge
{
    public int Index;
    public List<int> InterfaceSelectionObjects;
    public List<NPCDialouge> NPCDialouges;
    public string DisplayText;
    public int Quest;

    public NPCDialouge(List<int> interfaceSelectionObjects, string displayText, int index, List<NPCDialouge> dialouges = null, int quest = 0){
        InterfaceSelectionObjects = interfaceSelectionObjects;
        DisplayText = displayText;
        Index = index;
        if(dialouges != null){
            NPCDialouges = dialouges;
        }
        if(quest != 0){
            Quest = quest;
        }
    }
}
