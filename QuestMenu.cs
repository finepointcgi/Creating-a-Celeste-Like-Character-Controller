using Godot;
using System;

public class QuestMenu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    
    [Export]
    public static PackedScene QuestElementRESLocation;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        QuestElementRESLocation = ResourceLoader.Load("res://QuestElement.tscn") as PackedScene;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void AddQuestElement(Quest quest){
        QuestElement questElement = QuestElementRESLocation.Instance() as QuestElement;
        var questList = GetNode("QuestList") as VBoxContainer;
        questList.AddChild(questElement);
        questElement.SetPosition(new Vector2(0,0));
        quest.InterfaceElement = questElement;
        UpdateQuestElement(questElement, quest.QuestName, quest.GetDescription());

    }

    public void UpdateQuestElement(QuestElement questElement, string title, string desc){
        questElement.updateTitleDesc(title, desc);
    }
}
