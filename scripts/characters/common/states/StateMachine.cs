// Finite State Machine code (FSM)
using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : Node
{
	[Export] public NodePath initialState;
	
	private Dictionary<string, State> _states;
	private State _currentState;
	
	public State CurrentState => _currentState; // Read only of _currentState
	
	public Character character { get; set; }
	
	public void Initialize(Character character)
	{
		this.character = character;

		_states = new Dictionary<string, State>();
		foreach (Node node in GetChildren())
		{
			if (node is State s)
			{
				_states[node.Name] = s;
				s.fsm = this;
				s.character = character;
			}
		}

		_currentState = GetNode<State>(initialState);
		//_currentState.Enter();
	}
	
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print(_currentState.Name);
		_currentState.Update((float) delta);
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		_currentState.PhysicsUpdate((float) delta);
	}
	
	// Input Handler
	public override void _UnhandledInput(InputEvent @event)
	{
		_currentState.HandleInput(@event);
	}
	
	// State transition function
	public void TransitionTo(string key)
	{
		
 	// If state not exists
	if (!_states.ContainsKey(key))
	{
		GD.PrintErr($"FSM: state '{key}' not found.");
		return;
	}
	
	// If the state selected is already the current one
	if (_currentState == _states[key])
		return;
	
	// Exiting the previous state and entering in the next one
	_currentState.Exit();
	_currentState = _states[key];
	_currentState.Enter();
	}
}
