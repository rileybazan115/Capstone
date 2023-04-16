using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAgent : Agent
{
	[SerializeField] public Perception perception;
	public Testing grid;
	public PathFollower path;
	public StateMachine stateMachine = new StateMachine();

	public BoolRef enemySeen;
	public BoolRef atDestination;
	public FloatRef enemyDistance;
	public FloatRef health;
	public FloatRef timer;

	[Header("Transition Bools")]
	public bool canIdle;
	public bool canPatrol;
	public bool canChase;
	public bool canDie;
	public bool canAttack;
	public bool canEvade;
	public bool canRoam;
	public bool canSeek;

	public string startingState = "";

	//for roam demo
	public Transform roamTransform;

	public GameObject enemy { get; set; }


	private void Start()
	{
		stateMachine.AddState(new IdleState(this, typeof(IdleState).Name));
		stateMachine.AddState(new PatrolState(this, typeof(PatrolState).Name));
		stateMachine.AddState(new ChaseState(this, typeof(ChaseState).Name));
		stateMachine.AddState(new DeathState(this, typeof(DeathState).Name));
		stateMachine.AddState(new AttackState(this, typeof(AttackState).Name));
		stateMachine.AddState(new EvadeState(this, typeof(EvadeState).Name));
		stateMachine.AddState(new RoamState(this, typeof(RoamState).Name));
		stateMachine.AddState(new SeekState(this, typeof(SeekState).Name));

		stateMachine.AddTransition(typeof(IdleState).Name, new Transition(new Condition[] { new BoolCondition(enemySeen, true), new FloatCondition(health, Condition.Predicate.GREATER, 30)}, canChase), typeof(ChaseState).Name);
		stateMachine.AddTransition(typeof(IdleState).Name, new Transition(new Condition[] { new FloatCondition(timer, Condition.Predicate.LESS, 0)}, canPatrol), typeof(PatrolState).Name);
		stateMachine.AddTransition(typeof(IdleState).Name, new Transition(new Condition[] { new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 0)}, canDie), typeof(DeathState).Name);
		stateMachine.AddTransition(typeof(IdleState).Name, new Transition(new Condition[] { new FloatCondition(timer, Condition.Predicate.LESS_EQUAL, 0)}, canRoam), typeof(RoamState).Name);

		stateMachine.AddTransition(typeof(PatrolState).Name, new Transition(new Condition[] { new BoolCondition(enemySeen, true)}, canChase), typeof(ChaseState).Name);
		stateMachine.AddTransition(typeof(PatrolState).Name, new Transition(new Condition[] { new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 0)}, canDie), typeof(DeathState).Name);

		stateMachine.AddTransition(typeof(ChaseState).Name, new Transition(new Condition[] { new BoolCondition(enemySeen, false)}, canSeek), typeof(SeekState).Name);
		stateMachine.AddTransition(typeof(ChaseState).Name, new Transition(new Condition[] { new FloatCondition(enemyDistance, Condition.Predicate.LESS_EQUAL, 2)}, canAttack), typeof(AttackState).Name);
		stateMachine.AddTransition(typeof(ChaseState).Name, new Transition(new Condition[] { new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 30)}, canEvade), typeof(EvadeState).Name);
		stateMachine.AddTransition(typeof(ChaseState).Name, new Transition(new Condition[] { new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 0)}, canDie), typeof(DeathState).Name);

		stateMachine.AddTransition(typeof(AttackState).Name, new Transition(new Condition[] { new FloatCondition(timer, Condition.Predicate.LESS_EQUAL, 0)}, canChase), typeof(ChaseState).Name);
		stateMachine.AddTransition(typeof(AttackState).Name, new Transition(new Condition[] { new BoolCondition(enemySeen, true), new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 30)}, canEvade), typeof(EvadeState).Name);

		stateMachine.AddTransition(typeof(EvadeState).Name, new Transition(new Condition[] { new BoolCondition(enemySeen, false)}, canIdle), typeof(IdleState).Name);
		stateMachine.AddTransition(typeof(EvadeState).Name, new Transition(new Condition[] { new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 0)}, canDie), typeof(DeathState).Name);

		stateMachine.AddTransition(typeof(RoamState).Name, new Transition(new Condition[] { new BoolCondition(atDestination, true)}, canIdle), typeof(IdleState).Name);

		//add more seek state transitions
		stateMachine.AddTransition(typeof(SeekState).Name, new Transition(new Condition[] { new FloatCondition(timer, Condition.Predicate.LESS_EQUAL, 0)}, canIdle), typeof(IdleState).Name);

		//change in inspector
		//stateMachine.SetState(stateMachine.StateFromName(typeof(PatrolState).Name));
		stateMachine.SetState(stateMachine.StateFromName(startingState));
	}

	void Update()
	{
		perception.GetGameObjects(out List<GameObject> enemies, out List<GameObject> gridObjects);

		enemySeen.value = (enemies.Count != 0);
		enemy = (enemies.Count != 0) ? enemies[0] : null;
		enemyDistance.value = (enemy != null) ? (Vector3.Distance(transform.position, enemy.transform.position)) : float.MaxValue;
		timer.value -= Time.deltaTime;

		//this is when the rays hit the grid,a adds them to the list and marks them, not searching, search is in seek
		foreach (GameObject go in gridObjects)
		{
			grid.getGrid().GetXYZ(go.transform.position, out int x, out int y, out int z);
			//grid.getGrid().SetGridObject(x, y, z, 100);
			grid.getGrid().GetGridObject(x, y, z).SetValue(100);
			grid.getGrid().TriggerGridObjectChanged(x, y, z);
		}


		stateMachine.Update();
		//animator.SetFloat("Speed", movement.velocity.magnitude);
	}

	private void OnGUI()
	{
		Vector2 screen = Camera.main.WorldToScreenPoint(transform.position);

		GUI.Label(new Rect(screen.x, Screen.height - screen.y, 600, 40), stateMachine.GetStateName());
	}

	public Transform GetPosition()
	{
		return this.transform;
	}
}
