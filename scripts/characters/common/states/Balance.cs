using Godot;
using System;

public partial class Balance : State
{
	public override void Enter()
	{
		character.Velocity = Vector2.Zero;
		character.AnimPlayer.Play("balance");
	}

		public override void PhysicsUpdate(float delta)
	{
		var input = character.InputData;

		if (!character.LeftEdgeSensor.IsColliding())
			character.UpdateFlipH(1);
		else if (!character.RightEdgeSensor.IsColliding())
			character.UpdateFlipH(-1);

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

		if (character.EdgeDetection() == 0)
		{
			fsm.TransitionTo("Idle");
			return;
		}
	}

}
