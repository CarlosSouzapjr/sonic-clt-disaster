using Godot;
using System;

public partial class Walk : State
{
	private float acceleration;
	
	public override void Enter()
	{
		character.AnimPlayer.Play("walk");
	}

	public override void PhysicsUpdate(float delta)
	{
		Vector2 velocity = character.Velocity;

		if (!character.IsOnFloor())
		{
			fsm.TransitionTo("Fall");
			return;
		}

		Vector2 input = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		input = input.Round(); // Keeps the input with integer values (avoid diagonal bugs)
		
		// Jump
		if (Input.IsActionJustPressed("ui_accept") && character.IsOnFloor())
		{
			fsm.TransitionTo("Jump");
			return;
		}

		// Horizontal Movement
		if (input.X != 0)
		{
			// Max speed with direction
			float targetSpeed = input.X * character.MaxSpeed;
			
			// Acceleration
			velocity.X = Mathf.MoveToward(
				velocity.X,
				targetSpeed,
				character.AccelerationConst * delta
			);
			
			
			if (Math.Sign(input.X) != Math.Sign(velocity.X) && Math.Abs(velocity.X) >= character.BreakSpeed){
				fsm.TransitionTo("Break");
				return;
			}
			
			if(Math.Abs(velocity.X) >= character.MaxSpeed){
				fsm.TransitionTo("Run");
				return;
			}
			
			//GD.Print(velocity.X);
			character.UpdateFlipH(input.X);
			
		} else {
			// Friction Dessacceleration (no input break)
			velocity.X = Mathf.MoveToward(
			velocity.X,
			0,
			character.FrictionDesaccelerationConst * delta
			);
		}
		
		if(velocity.X == 0){
			fsm.TransitionTo("Idle");
			return;
		}
		
		

		character.Velocity = velocity;
		character.MoveAndSlide();
	}
}
