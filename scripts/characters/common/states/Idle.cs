using Godot;
using System;

public partial class Idle : State
{
	public override void Enter()
	{
		character.Velocity = Vector2.Zero;
		character.AnimPlayer.Play("idle");
	}

	public override void PhysicsUpdate(float delta)
	{
		var input = character.InputData;

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

		if (input.Move.Y < 0)
		{
			fsm.TransitionTo("LookUp");
			return;
		}

		if (input.Move.Y > 0)
		{
			fsm.TransitionTo("LookDown");
			return;
		}

		if (character.EdgeDetection() != 0)
		{
			fsm.TransitionTo("Balance");
			return;
		}
	}
}
