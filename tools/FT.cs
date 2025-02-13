using Godot;
using System;

public partial class FT : Node
{
    public static FT Instance; //Godot's singletons don't really work outside of GDScript, so this is a workaround
    public DebugLine debugLine;
    public DebugText debugText;
    public Invoke invoke;
	public override void _EnterTree() {
        Instance = this;
    }
    public override void _Ready() {
        //Instance tools
        debugText = new();
        debugText.Ready();

        debugLine = new DebugLine();
        AddChild(debugLine); //DebugLine inherits from a node, so we add it to the tree
        
        invoke = new();
    }
    public override void _Process(double delta) {
        invoke.Process(delta);
    }
}
