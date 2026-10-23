using Godot;
using System;

public partial class RunBase : State
{
	public override void Enter()
	{
		//character.AnimPlayer.Play("run");
	}

	public override void PhysicsUpdate(float delta)
	{
		var input = character.InputData;
		Vector2 velocity = character.Velocity;

		// ===== FALL CHECK =====
		if (!character.IsOnFloor())
		{
			fsm.TransitionTo("Fall");
			return;
		}

		// ===== JUMP =====
		if (input.Jump && character.IsOnFloor())
		{
			fsm.TransitionTo("Jump");
			return;
		}
		
		if(character.AnimationFinished()){
			character.AnimPlayer.Play("run");
		}

		float dir = input.Move.X;

		// ===== HORIZONTAL MOVEMENT =====
		if (dir != 0)
		{
			float targetSpeed = dir * character.MaxSpeed;

			velocity.X = Mathf.MoveToward(
				velocity.X,
				targetSpeed,
				character.AccelerationConst * delta
			);

			character.UpdateFlipH(dir);

			// ===== BREAK (reverse direction) =====
			if (Math.Sign(dir) != Math.Sign(velocity.X) &&
				Math.Abs(velocity.X) >= character.BreakSpeed)
			{
				fsm.TransitionTo("Break");
				return;
			}
		}
		else
		{
			// ===== FRICTION =====
			velocity.X = Mathf.MoveToward(
				velocity.X,
				0,
				character.FrictionDesaccelerationConst * delta
			);
		}

		// ===== STATE TRANSITIONS =====
		if (Math.Abs(velocity.X) < character.MaxSpeed)
		{
			fsm.TransitionTo("Walk");
			return;
		}

		character.Velocity = velocity;
		character.MoveAndSlide();
	}
}
