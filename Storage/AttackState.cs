using UnityEngine;

public class AttackState : IState
{
    AIController ai;
    private UnitStats targetStats;

    //constructor
    public AttackState (AIController ai)
    {
        this.ai = ai;
    }

    //Stop the navmesh from moving
    public void Enter()
    {
        ai.agent.isStopped = true;

        if(ai.target != null)
        {
            targetStats = ai.target.GetComponent<UnitStats>();
            Debug.Log("Component found");
        }
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

        ai.AttackTarget(targetStats);
            

    }

    public void Exit()
    {

    }

    

    
}
