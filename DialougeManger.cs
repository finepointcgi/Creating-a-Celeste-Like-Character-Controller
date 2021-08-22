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

    public List<InterfaceSelection> Selections = new List<InterfaceSelection>();
    private bool isDialougeUp;
    private int currentSelectionIndex = 0;
    public string DialougeHeader;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public async override void _Process(float delta)
    {
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
                await ToSignal(GetTree(), "idle_frame");
                dispayNextDialougeElement(Selections[currentSelectionIndex].interfaceSelectionObject.SelectionIndex);
            }
        }
    }
    public void ShowDialougeElement(){
        GetNode<Popup>("Popup").Popup_();
        GetNode<Label>("Popup/Label").Text = DialougeHeader;
        WriteDialouge(npcDialouge[0]);
    }

    public void WriteDialouge(NPCDialouge dialouge){
        foreach (Node item in GetNode<Node>("Popup/HBoxContainer").GetChildren())
        {
            item.QueueFree();
        }
        Selections = new List<InterfaceSelection>();
        GetNode<RichTextLabel>("Popup/RichTextLabel").Text = dialouge.DisplayText;
        foreach (var item in dialouge.InterfaceSelectionObjects)
        {
            InterfaceSelection interfaceSelection = InterfaceSelectableObject.Instance() as InterfaceSelection;
            interfaceSelection.interfaceSelectionObject = item;
            GetNode<HBoxContainer>("Popup/HBoxContainer").AddChild(interfaceSelection);
            Selections.Add(interfaceSelection);
            interfaceSelection.SetSelected(false);
        }
        Selections[0].SetSelected(true);
        currentSelectionIndex = 0;
        isDialougeUp = true;
        GameManager.GlobalGameManager.GamePaused = true;    
    }

    private void shutdownDialouge(){
        GetNode<Popup>("Popup").Hide();
        GameManager.GlobalGameManager.GamePaused = false;
        isDialougeUp = false;
    }

    private void dispayNextDialougeElement(int index){
        if(npcDialouge.ElementAtOrDefault(index) == null || index == -1){
            shutdownDialouge();
        }else{
            WriteDialouge(npcDialouge[index]);
        }
    }

}
