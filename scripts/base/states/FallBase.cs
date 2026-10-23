using Godot;
using System;

public partial class FallBase : State
{
	public override void Enter()
	{
		character.AnimPlayer.Play("fall");
	}

	public override void PhysicsUpdate(float delta)
	{
		var input = character.InputData;
		Vector2 velocity = character.Velocity;

		if (character.IsOnFloor())
		{
			fsm.TransitionTo("Walk");
			return;
		}

		if (input.Jump && character.CoyoteTimer > 0f)
		{
			character.CoyoteTimer = 0f;
			fsm.TransitionTo("Jump");
			return;
		}

		if (input.Move.X != 0)
		{
			float targetSpeed = input.Move.X * character.MaxSpeed;
			velocity.X = Mathf.MoveToward(
				velocity.X,
				targetSpeed,
				character.AirAccelerationConst * delta
			);

			character.UpdateFlipH(input.Move.X);
		}

		character.Velocity = velocity;
		character.MoveAndSlide();
	}

}
