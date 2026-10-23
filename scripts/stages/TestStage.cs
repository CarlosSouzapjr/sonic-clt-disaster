using Godot;

public partial class TestStage : Node2D
{
    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionPressed("debug_restart", allowEcho: false))
            return;

        GetViewport().SetInputAsHandled();
        GetTree().ReloadCurrentScene();
    }
}
