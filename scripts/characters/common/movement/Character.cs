// Base Class for Characters
using Godot;
using System;

public abstract partial class Character : CharacterBody2D
{
	[Export] public float AccelerationConst = 0.046875f * 60f * 60f; // ~= px/s²
	[Export] public float AirAccelerationConst = 0.09375f * 60f * 60f;
	[Export] public float DesaccelerationConst = 0.5f * 60f * 60f * 0.5f;
	[Export] public float GravityForce = 0.21875f * 60f * 60f;
	[Export] public float FrictionDesaccelerationConst = 0.046875f * 60f * 60f;
	[Export] public float MaxSpeed = 6f * 60f; // px/s
	[Export] public float MaxFallSpeed = 6f * 60f;
	[Export] public float BreakSpeed = 4f * 60f; // Speed required for break animation
	[Export] public float JumpHeight = 64f; // px
	[Export] public float CoyoteTimeWindow = 0.12f; // seconds
	
	public float CoyoteTimer = 0f;
	public float JumpForce;
	
	public PlayerInputData InputData;
		
	public Node2D Visual { get; private set; }
	public AnimatedSprite2D AnimSprite { get; private set; }
	public AnimationPlayer AnimPlayer { get; private set; }
	
	public Node2D Sensors { get; private set; }
	public RayCast2D LeftEdgeSensor { get; private set; }
	public RayCast2D RightEdgeSensor { get; private set; }
	
	public override void _Ready()
	{
	 	JumpForce = -Mathf.Sqrt(2f * GravityForce * JumpHeight); // jump = sqrt(2*g*h) -> g = gravity, h = max_heigh
		
		// Getting visual node
		Visual = GetNode<Node2D>("Visual");
		Sensors = GetNode<Node2D>("Sensors");
		
		// Getting visual elements from visual node
		AnimPlayer = Visual.GetNode<AnimationPlayer>("AnimationPlayer");
		AnimSprite = Visual.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		// Getting sensors
		LeftEdgeSensor = Sensors.GetNode<RayCast2D>("LeftEdgeSensor");
		RightEdgeSensor = Sensors.GetNode<RayCast2D>("RightEdgeSensor");
		
		// Making sensors don't collide with CharacterBody
		LeftEdgeSensor.AddException(this);
		RightEdgeSensor.AddException(this);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		
		if (IsOnFloor())
		{
			CoyoteTimer = CoyoteTimeWindow; // reset while in the floor
		}
		else
		{
			CoyoteTimer -= (float)delta;
		}

		// Applying Gravity util get fall speed
		if (!IsOnFloor())
		{
			velocity.Y += GravityForce * (float)delta;
			velocity.Y = Mathf.Min(velocity.Y, MaxFallSpeed);
		}
		Velocity = velocity;
		MoveAndSlide();
	}
	
	public void UpdateFlipH(float direction)
	{
		// direction < 0 → left
		// direction > 0 → right
		// direction = 0 → none

		if (direction == 0)
			return;

		// Normalize direction
	 	int sign = direction < 0 ? -1 : 1;
		
		Vector2 scale = Visual.Scale;
		scale.X = Math.Abs(scale.X) * (direction < 0 ? -1 : 1);
		Visual.Scale = scale;
	}
	
	public bool AnimationFinished(){ // Verify if the current animation is in it's last frame (finished)
		int totalFrames = AnimSprite.SpriteFrames.GetFrameCount(AnimSprite.Animation);
		int currentFrame = AnimSprite.Frame;
		
		return currentFrame == totalFrames-1;
	}
	
	public int EdgeDetection(){
		// Function to identify edge direction
		// Returns:
		// 		-1 if left sensor don't identify the floor
		// 		1 if right sensor don't identify the floor
		// 		0 if there is no edge
		
		if(!LeftEdgeSensor.IsColliding()){
			return -1;
		} else if(!RightEdgeSensor.IsColliding()){
			return 1;
		}
		return 0;
	}

}
