using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshMovement))]
public class NavMeshMovement : Movement
{
	public NavMeshAgent navMeshAgent;

	//pathfinding
	int currentPathIndex;
	private List<Vector3> pathVectorList;

	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		navMeshAgent.speed = movementData.maxSpeed;
		navMeshAgent.acceleration = movementData.maxForce;
		navMeshAgent.angularSpeed = movementData.turnRate;

		HandleMovement();

		/*if (Input.GetMouseButtonDown(0))
		{
			SetTargetPosition(Utils.GetMouseWorldPosition());
		}*/
	}

	public override void ApplyForce(Vector3 force)
	{

	}

	public override void MoveTowards(Vector3 target)
	{
		navMeshAgent.SetDestination(target);
	}

	public override void Resume()
	{
		navMeshAgent.isStopped = false;
	}

	public override void Stop()
	{
		navMeshAgent.isStopped = true;
	}

	public override Vector3 destination
	{ 
		get => navMeshAgent.destination;
		set => navMeshAgent.destination = value;
	}

	//pathfinding

	//not being used
	private void HandleMovement()
	{
		if (pathVectorList != null)
		{
			Vector3 targetPosition = pathVectorList[currentPathIndex];
			if (Vector3.Distance(transform.position, targetPosition) > 1f)
			{
				Vector3 moveDir = (targetPosition - transform.position).normalized;

				float distanceBefore = Vector3.Distance(transform.position, targetPosition);
				transform.position = transform.position + moveDir * maxSpeed * Time.deltaTime;
				MoveTowards(targetPosition);
			}
			else
			{
				currentPathIndex++;
				if (currentPathIndex >= pathVectorList.Count)
				{
					StopMoving();
				}
			}
		}
	}

	private void StopMoving()
	{
		pathVectorList = null;
	}

	public Vector3 GetPosition()
	{
		return transform.position;
	}

	public void SetTargetPosition(Vector3 targetPosition)
	{
		currentPathIndex = 0;
		pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);

		if (pathVectorList != null && pathVectorList.Count > 1)
		{
			pathVectorList.RemoveAt(0);
		}
	}
}
