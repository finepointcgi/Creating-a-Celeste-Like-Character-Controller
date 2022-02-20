using System;
using System.Collections.Generic;
public class NPCDialouge
{
    public int Index;
    public List<int> InterfaceSelectionObjectsID;
    public List<NPCDialouge> NPCDialouges;
    public string DisplayText;
    public int QuestID;

    public NPCDialouge(List<int> interfaceSelectionObjectsID, string displayText, int index, List<NPCDialouge> dialouges = null, int questid = -1){
        InterfaceSelectionObjectsID = interfaceSelectionObjectsID;
        DisplayText = displayText;
        Index = index;
        if(dialouges != null){
            NPCDialouges = dialouges;
        }
        if(questid != 0){
            QuestID = questid;
        }
    }
}
