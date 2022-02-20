using System;

public class InterfaceSelectionObject
{
    public int Id;
    public string SelectionText;
    public int SelectionIndex;
    public bool AcceptQuest;

    public InterfaceSelectionObject(int id, int index, string selectionText, bool acceptQuest = false){
        Id = id;
        SelectionText = selectionText;
        SelectionIndex = index;
        AcceptQuest = acceptQuest;
    }
}
