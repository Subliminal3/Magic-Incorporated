using UnityEngine;

[CreateAssetMenu(fileName = "State", menuName = "State Machine/Move")]
public class State_Move : State
{
    UnitController ai;

    public override void OnEnter(UnitController ai)
    {
        ai.Agent.isStopped = false;
    }
    public override State Tick(UnitController ai)
    {
        //Target is reset to default if no target
        if (ai.target == null)
        {
            ai.target = ai.defaultTarget;
        }

        ai.Agent.SetDestination(ai.target.transform.position);

        float distance = Vector3.Distance(ai.transform.position, ai.target.transform.position);

        //transition to attack state if in attack range
        if (distance <= ai.data.attackRange)
        {
            return ai.transitionStates.attackState;
        }

        //Stays in the same state
        return this;
    }

    public override void OnExit(UnitController ai)
    {
        ai.Agent.ResetPath();
    }
    
}
