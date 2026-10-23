// Base Class for State Machines
using Godot;
using System;

public abstract partial class State : Node
{	
	public StateMachine fsm { get; internal set; }  // Finite State Machine of the state
	public Character character { get; internal set; } // Current player character
	
	public virtual void Enter(){}
	public virtual void Exit(){}
	
	public virtual void Update(float delta){}
	public virtual void PhysicsUpdate(float delta){}
	public virtual void HandleInput(InputEvent @event){}

}
