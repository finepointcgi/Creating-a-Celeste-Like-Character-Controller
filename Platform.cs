using Godot;
using Godot.Collections;

public class Platform : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Array moveLocations;
    private Tween tween;
    private KinematicBody2D platform;
    private int index;
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
       moveLocations = GetNode<Node>("MovementLocations").GetChildren();
       tween = GetNode<Tween>("PlatformObject/Tween");
       platform = GetNode<KinematicBody2D>("PlatformObject");
       _on_Tween_tween_completed(null,null);       
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private void _on_Tween_tween_completed(object obj, NodePath path){
        index += 1;
        if(index > moveLocations.Count - 1){
            index = 0;
        }
        Position2D node = moveLocations[index] as Position2D;
        tween.InterpolateProperty(platform, "position", platform.Position, node.Position, node.Position.DistanceTo(platform.Position) / 30, Tween.TransitionType.Linear, Tween.EaseType.InOut);
        tween.Start();
    }
}
