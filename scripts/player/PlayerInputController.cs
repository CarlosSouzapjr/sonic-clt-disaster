using Godot;

public partial class PlayerInputController : Node
{
	[Export] public Character character;
	
	public void Initialize(Character character)
	{
		this.character = character;
	}
	
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		if (character == null)
		{
			GD.Print("Input Controller: Character NULL");
			return;
		}

		PlayerInputData input = new PlayerInputData();

		input.Move = Input.GetVector(
			"ui_left",
			"ui_right",
			"ui_up",
			"ui_down"
		);
		
		input.Jump = Input.IsActionJustPressed("jump");
		
		character.InputData = input;
	}
}
