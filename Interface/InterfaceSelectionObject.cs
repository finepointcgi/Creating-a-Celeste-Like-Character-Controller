using System;

public class InterfaceSelectionObject
{
    public string SelectionText;
    public int SelectionIndex;
    public bool AcceptQuest;

    public InterfaceSelectionObject(int index, string selectionText, bool acceptQuest = false){
        SelectionText = selectionText;
        SelectionIndex = index;
        AcceptQuest = acceptQuest;
    }
}
