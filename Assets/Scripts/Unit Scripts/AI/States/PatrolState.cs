using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "PatrolState", menuName = "AI/States/Patrol")]
public class PatrolState : AIState
{
    public float patrolWaitTime = 2f;
    public AIState ifTargetFound;

    //private Vector3 patrolPoint;
    //private bool hasPatrolPoint;
    //private float lastPatrolTime;

    public override void OnEnter(UnitController controller)
    {
        controller.lastPatrolTime = -patrolWaitTime; 
        controller.hasPatrolPoint = false;
    }

    public override AIState Tick(UnitController controller)
    {
        controller.Target = controller.FindNearestEnemy();
        if (controller.Target != null)
        {
            controller.hasPatrolPoint = false; // Reset for next time we enter patrol state
            return ifTargetFound;
        }
        if (!controller.hasPatrolPoint)
        {
            SetNewPatrolPoint(controller);
        }

        float distanceFromPatrolPoint = Vector3.Distance(controller.transform.position, controller.patrolPoint);
        Debug.Log(distanceFromPatrolPoint);
        if (distanceFromPatrolPoint < controller.Agent.stoppingDistance)
        {
            if (Time.time - controller.lastPatrolTime > patrolWaitTime)
            {
                SetNewPatrolPoint(controller);
            }
        }

        if(controller.hasPatrolPoint)
        {
            controller.Agent.SetDestination(controller.patrolPoint);
        }

        return this; // Stay in patrol state
    }

    private void SetNewPatrolPoint(UnitController controller)
    {
        Vector3 randomDirection = Random.insideUnitSphere * controller.data.detectionRange;
        randomDirection += controller.transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, controller.data.detectionRange, 1))
        {
            controller.patrolPoint = hit.position;
            controller.hasPatrolPoint = true;
            controller.lastPatrolTime = Time.time;
        }
    }
}
