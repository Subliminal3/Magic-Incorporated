using UnityEngine;

public class MoveState : IState
{
    AIController ai;

    public MoveState(AIController ai)
    {
        this.ai = ai; 
    }

    public void Enter()
    {
        ai.agent.isStopped = false;
    }

    public void Tick()
    {
        if(ai.target == null)
        {
            ai.ChangeState(ai.Idle);
            return;

        }

        ai.agent.SetDestination(ai.target.position);

        float distance = Vector3.Distance(ai.transform.position, ai.target.position);

        if(distance <= ai.attackRange)
        {
            ai.ChangeState(ai.Attack);
            Debug.Log(distance);
        }
    }

    public void Exit()
    {
        ai.agent.ResetPath();
    }

    
}
