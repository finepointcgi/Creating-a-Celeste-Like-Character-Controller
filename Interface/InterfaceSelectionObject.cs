using System;

public class InterfaceSelectionObject
{
    public int ID;
    public string SelectionText;
    public int SelectionIndex;
    public bool AcceptQuest;

    public InterfaceSelectionObject(int id, int index, string selectionText, bool acceptQuest = false){
        ID = id;
        SelectionText = selectionText;
        SelectionIndex = index;
        AcceptQuest = acceptQuest;
    }
}
