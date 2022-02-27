using Godot;
using System;

public class QuestElement : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public RichTextLabel Title;
    public RichTextLabel Desc;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Title = GetNode("Title") as RichTextLabel;
        Desc = GetNode("Desc") as RichTextLabel;   
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void updateTitleDesc(string title, string desc){
        Title.Text = title;
        Desc.Text = desc;
    }
}
