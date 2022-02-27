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
    public List<InterfaceSelectionObject> InterfaceSelectionObjects;
    [Export]
    public PackedScene InterfaceSelectableObject;

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
    public async override void _PhysicsProcess(float delta)
    {
        await ToSignal(GetTree(), "physics_frame");
        
        if(GameManager.GlobalGameManager.GamePaused && isDialougeUp){
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
                InterfaceSelectionObject selectionObject = Selections[currentSelectionIndex].interfaceSelectionObject;
                if(selectionObject.AcceptQuest)
                    GameManager.QuestManager.AddActiveQuests(GameManager.QuestManager.AvalQuest.Where(x => x.Id == currentDialougeOpen.Quest).FirstOrDefault());
                dispayNextDialougeElement(selectionObject.SelectionIndex);
            }
        }
    }
    public async void ShowDialougeElement(){
        GetNode<Popup>("Popup").Popup_();
        GetNode<Label>("Popup/Label").Text = DialougeHeader;
        GameManager.Player.pauseInput = true;
        WriteDialouge(npcDialouge[0]);        
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
        foreach (var item in dialouge.InterfaceSelectionObjects)
        {
            InterfaceSelection interfaceSelection = InterfaceSelectableObject.Instance() as InterfaceSelection;
            interfaceSelection.interfaceSelectionObject = InterfaceSelectionObjects.Where(x => x.ID == item).FirstOrDefault();
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
        isDialougeUp = false;
        await ToSignal(GetTree(), "physics_frame");
        GameManager.Player.pauseInput = false;
    }

    private void dispayNextDialougeElement(int index){
        if(npcDialouge.ElementAtOrDefault(index) == null || index == -1){
            shutdownDialouge();
        }else{
            WriteDialouge(npcDialouge[index]);
        }
    }

}
