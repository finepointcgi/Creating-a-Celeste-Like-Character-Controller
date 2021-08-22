using System;
using System.Collections.Generic;
public class NPCDialouge
{
    public int Index;
    public List<InterfaceSelectionObject> InterfaceSelectionObjects;
    public List<NPCDialouge> NPCDialouges;
    public string DisplayText;

    public NPCDialouge(List<InterfaceSelectionObject> interfaceSelectionObjects, string displayText, int index, List<NPCDialouge> dialouges = null){
        InterfaceSelectionObjects = interfaceSelectionObjects;
        DisplayText = displayText;
        Index = index;
        if(dialouges != null){
            NPCDialouges = dialouges;
        }
    }
}
