using UnityEngine;

[CreateAssetMenu(fileName = "ChaseState", menuName = "AI/States/Chase")]
public class ChaseState : AIState
{
    public AIState ifTargetIsDead;
    public AIState ifTargetInRange;

    public override AIState Tick(UnitController controller)
    {
        if (controller.Target == null || controller.Target.CurrentHealth <= 0)
        {
            controller.Target = null;
            return ifTargetIsDead;
        }

        float distanceToTarget = Vector3.Distance(controller.transform.position, controller.Target.transform.position);
        
        if (distanceToTarget <= controller.data.attackRange)
        {
            controller.Agent.isStopped = true;
            return ifTargetInRange;
        }
        else
        {
            controller.Agent.isStopped = false;
            controller.Agent.SetDestination(controller.Target.transform.position);
        }
        return this;
    }
}
