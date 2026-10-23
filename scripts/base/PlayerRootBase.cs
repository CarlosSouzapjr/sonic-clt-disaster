using Godot;
using System;

public partial class PlayerRootBase : Node
{
	public Character character { get; private set; }
	public StateMachine FSM { get; private set; }
	public PlayerInputController InputController { get; private set; }
	public CameraPivot cameraPivot { get; private set; }

	public override void _Ready()
	{
		character = GetNode<Character>("CharacterBody");
		FSM = GetNode<StateMachine>("FSM");
		InputController = GetNode<PlayerInputController>("PlayerInputController");
		cameraPivot = GetNode<CameraPivot>("CameraPivot");
		
		FSM.Initialize(character);
		InputController.Initialize(character);
		cameraPivot.Initialize(character, FSM);
		
	}
}
