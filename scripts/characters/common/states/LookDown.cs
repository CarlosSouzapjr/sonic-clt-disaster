using Godot;
using System;

public partial class LookDown : State
{
	public override void Enter()
	{
		character.Velocity = Vector2.Zero;
		character.AnimPlayer.Play("look_down_startup");
	}

	public override void PhysicsUpdate(float delta)
	{
		var input = character.InputData;

		if (character.AnimationFinished())
			character.AnimPlayer.Play("look_down");

		if (input.Jump && character.IsOnFloor())
		{
			fsm.TransitionTo("Jump");
			return;
		}

		if (input.Move.X != 0)
		{
			fsm.TransitionTo("Walk");
			return;
		}

		if (input.Move.Y == 0)
		{
			fsm.TransitionTo("Idle");
			return;
		}

		if (input.Move.Y < 0)
		{
			fsm.TransitionTo("LookUp");
			return;
		}
	}

}
