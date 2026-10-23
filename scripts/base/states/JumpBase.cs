using Godot;
using System;

public partial class JumpBase : State
{
	public override void Enter()
	{
		Vector2 velocity = character.Velocity;
		velocity.Y = character.JumpForce;
		character.Velocity = velocity;
		
		character.AnimPlayer.Play("jump");
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

		if (input.Move.X != 0)
		{
			float targetSpeed = input.Move.X * character.MaxSpeed;
			velocity.X = Mathf.MoveToward(
				velocity.X,
				targetSpeed,
				character.AirAccelerationConst * delta
			);
		}

		character.Velocity = velocity;
		character.MoveAndSlide();
	}

}
