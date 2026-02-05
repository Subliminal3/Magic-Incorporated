using UnityEngine;

[CreateAssetMenu(fileName = "State", menuName = "State Machine/Move")]
public class State_Move : State
{
    UnitController ai;

    public State_Move(UnitController ai)
    {
        this.ai = ai;
    }

    public override void OnEnter(UnitController ai)
    {
        ai.Agent.isStopped = false;
    }
    public override State Tick(UnitController ai)
    {
        //Transition to idle if no target
        if (ai.target == null)
        {
            return ai.transitionStates.idleState;
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
