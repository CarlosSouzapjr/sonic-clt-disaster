using Godot;
using System;

public partial class CameraPivotController : Node2D
{
	[Export] public float FollowSpeedX = 700f;
	[Export] public float FollowSpeedY = 700f;
	[Export] public float MaxOffsetY = 64f;    // Vertical Limit
	[Export] public float MaxOffsetX = 16f; // Horizontal Limit

	
	[Export] public float LookDelay = 0.5f;   // Time to move camera
	[Export] public float LookOffsetY = 64f;   // How much the camera moves
	[Export] public float LookLerpSpeed = 6f;  // How fast the camera moves (slower = smoother)
	
	[Export] public float LookAheadDistance = 64f;
	[Export] public float LookAheadLerpSpeed  = 6f; // How fast the camera moves (slower = smoother)
	[Export] public float MinSpeedForLookAhead = 40f;

	private float lookUpTimer = 0f;
	private float lookDownTimer = 0f;
	private float currentOffsetY = 0f;
	private float currentLookAheadX = 0f;
	

	
	public Character character;	// Current player character
	public StateMachine FSM;
	
	public void Initialize(Character character, StateMachine FSM)
	{
		this.character = character;
		this.FSM = FSM;
	}
	
	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if (character == null || FSM == null)
			return;

		float deltaF = (float)delta;

		Vector2 pos = GlobalPosition;

		// ================= LOOK INPUT (VERTICAL) =================
		float inputY = Input.GetAxis("ui_up", "ui_down");

		if (FSM.CurrentState is LookUp)
			lookUpTimer += deltaF;
		else
			lookUpTimer = 0f;

		if (FSM.CurrentState is LookDown)
			lookDownTimer += deltaF;
		else
			lookDownTimer = 0f;

		float targetOffsetY = 0f;
		if (lookUpTimer >= LookDelay || lookDownTimer >= LookDelay)
			targetOffsetY = inputY * LookOffsetY;

		currentOffsetY = Mathf.MoveToward(
			currentOffsetY,
			targetOffsetY,
			LookLerpSpeed * LookOffsetY * deltaF
		);

		// ================= LOOK-AHEAD (HORIZONTAL) =================
		float velocityX = character.Velocity.X;
		float speed = Mathf.Abs(velocityX);

		float targetLookAheadX = 0f;

		if (speed > MinSpeedForLookAhead)
		{
			float direction = Mathf.Sign(velocityX);
			float speedFactor = Mathf.Clamp(speed / character.MaxSpeed, 0f, 1f);
			targetLookAheadX = direction * LookAheadDistance * speedFactor;
		}

		currentLookAheadX = Mathf.Lerp(
			currentLookAheadX,
			targetLookAheadX,
			LookAheadLerpSpeed * deltaF
		);

		// ================= CAMERA TARGET =================
		Vector2 target = character.GlobalPosition;
		target.X += currentLookAheadX;
		target.Y += currentOffsetY;

		// ================= FOLLOW + LIMIT =================
		float deltaX = target.X - pos.X;

		if (Mathf.Abs(deltaX) > MaxOffsetX)
		{
			// Out of the limits → moves smoothly
			float desiredX = target.X - Mathf.Sign(deltaX) * MaxOffsetX;
			pos.X = Mathf.MoveToward(pos.X, desiredX, FollowSpeedX * deltaF);
		}
		else
		{
			// In the limits → follow the target in constant speed
			pos.X = target.X;
		}

		float deltaY = target.Y - pos.Y;

		if (Mathf.Abs(deltaY) > MaxOffsetY)
		{
			// Out of vertical limits → smooth catch-up
			float desiredY = target.Y - Mathf.Sign(deltaY) * MaxOffsetY;
			pos.Y = Mathf.MoveToward(pos.Y, desiredY, FollowSpeedY * deltaF);
		}
		else
		{
			// Inside vertical limits → stay glued
			pos.Y = target.Y;
		}


		GlobalPosition = pos;
	}
}
