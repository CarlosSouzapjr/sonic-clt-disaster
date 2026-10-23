using Godot;
using System;

public partial class Break : State
{
	// Called when the node enters the scene tree for the first time.
	public override void Enter()
	{
		character.AnimPlayer.Play("break");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void PhysicsUpdate(float delta)
	{
		var input = character.InputData;
		Vector2 velocity = character.Velocity;

		velocity.X = Mathf.MoveToward(
			velocity.X,
			0,
			character.DesaccelerationConst * delta
		);

		character.Velocity = velocity;
		character.MoveAndSlide();

		if (velocity.X == 0)
		{
			fsm.TransitionTo("Idle");
			return;
		}

		if (input.Move.X == 0 ||
		   (Math.Sign(input.Move.X) == Math.Sign(velocity.X) &&
			Math.Abs(velocity.X) < character.BreakSpeed))
		{
			fsm.TransitionTo("Walk");
			return;
		}
		
		if (!character.IsOnFloor())
		{
			fsm.TransitionTo("Fall");
			return;
		}
		
		if (input.Jump)
		{
			fsm.TransitionTo("Jump");
			return;
		}
		
	}

}
