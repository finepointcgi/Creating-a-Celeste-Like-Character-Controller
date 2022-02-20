using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class DialougeManger : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public List<NPCDialouge> npcDialouge;
    [Export]
    public PackedScene InterfaceSelectableObject;
    public List<InterfaceSelectionObject> interfaceSelectionObjects;

    public List<InterfaceSelection> Selections = new List<InterfaceSelection>();
    private bool isDialougeUp;
    private int currentSelectionIndex = 0;
    public string DialougeHeader;
    private NPCDialouge currentDialougeOpen;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public async override void _Process(float delta)
    {
        if(GameManager.GlobalGameManager.GamePaused && isDialougeUp){
             await ToSignal(GetTree(), "idle_frame");
            if(Input.IsActionJustPressed("ui_left")){
                foreach (var item in Selections)
                {
                    item.SetSelected(false);
                }
                currentSelectionIndex -= 1;
                if(currentSelectionIndex < 0){
                    currentSelectionIndex = 0;
                }
                Selections[currentSelectionIndex].SetSelected(true);
            }else if (Input.IsActionJustPressed("ui_right")){
                foreach (var item in Selections)
                {
                    item.SetSelected(false);
                }
                currentSelectionIndex += 1;
                if(currentSelectionIndex > Selections.Count - 1){
                    currentSelectionIndex = Selections.Count - 1;
                }
                Selections[currentSelectionIndex].SetSelected(true);
            }else if (Input.IsActionJustPressed("ui_accept")){
                InterfaceSelectionObject selectedObject = Selections[currentSelectionIndex].interfaceSelectionObject;
                if(selectedObject.AcceptQuest){
                   //GameManager.QuestManager.AvalQuests.Where(d => d.id == currentDialougeOpen.QuestID).FirstOrDefault(); //currentDialougeOpen.Quest.accepted = true; 
                   GameManager.QuestManager.ActiveQuests.Add(currentDialougeOpen.QuestID);
                }
                dispayNextDialougeElement(selectedObject.SelectionIndex);
            }
        }
    }
    public async void ShowDialougeElement(int index){
        GetNode<Popup>("Popup").Popup_();
        GetNode<Label>("Popup/Label").Text = DialougeHeader;
        WriteDialouge(npcDialouge[index]);
        GameManager.Player.pauseInput = true;
        await ToSignal(GetTree(), "idle_frame");

    }

    public void WriteDialouge(NPCDialouge dialouge){
        currentDialougeOpen = dialouge;
        foreach (Node item in GetNode<Node>("Popup/HBoxContainer").GetChildren())
        {
            item.QueueFree();
        }
        Selections = new List<InterfaceSelection>();
        GetNode<RichTextLabel>("Popup/RichTextLabel").Text = dialouge.DisplayText;
        foreach (var item in dialouge.InterfaceSelectionObjectsID)
        {
            InterfaceSelection interfaceSelection = InterfaceSelectableObject.Instance() as InterfaceSelection;
            interfaceSelection.interfaceSelectionObject = interfaceSelectionObjects.Where(x => x.Id == item).FirstOrDefault();
            GetNode<HBoxContainer>("Popup/HBoxContainer").AddChild(interfaceSelection);
            Selections.Add(interfaceSelection);
            interfaceSelection.SetSelected(false);
        }
        Selections[0].SetSelected(true);
        currentSelectionIndex = 0;
        isDialougeUp = true;
        GameManager.GlobalGameManager.GamePaused = true;    
    }

    private async void shutdownDialouge(){
        GetNode<Popup>("Popup").Hide();
        GameManager.GlobalGameManager.GamePaused = false;
        GameManager.Player.pauseInput = false;
        isDialougeUp = false;
        await ToSignal(GetTree(), "idle_frame");
    }

    private void dispayNextDialougeElement(int index){
        if(npcDialouge.ElementAtOrDefault(index) == null || index == -1){
            
            shutdownDialouge();
        }else{
            WriteDialouge(npcDialouge[index]);
        }
    }

}
