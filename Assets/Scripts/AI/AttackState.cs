using UnityEngine;

public class AttackState : IState
{
    AIController ai;

    //constructor
    public AttackState (AIController ai)
    {
        this.ai = ai;
    }

    //Stop the navmesh from moving
    public void Enter()
    {
        ai.agent.isStopped = true;
    }

    public void Tick()
    {
        //check for target
        if(ai.target == null)
        {
            ai.ChangeState(ai.Idle);
            return;
        }

        float distance = Vector3.Distance(ai.transform.position, ai.target.position);

        if (distance > ai.attackRange)
        {
            ai.ChangeState(ai.Move);
        }
            

    }

    public void Exit()
    {

    }

    
}
