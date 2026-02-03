using UnityEngine;

[CreateAssetMenu(fileName = "ChaseState", menuName = "State Machine/States/Chase")]
public class ChaseState : State
{
    public State ifTargetIsDead;
    public State ifTargetInRange;

    public override State Tick(UnitController controller)
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
