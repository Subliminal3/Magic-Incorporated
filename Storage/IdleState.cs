using UnityEngine;

public class IdleState : IState
{
    AIController ai;

    public IdleState(AIController ai)
    {
        this.ai = ai;
    }

    public void Enter()
    {
        ai.agent.isStopped = true;
    }

    public void Tick()
    {
        if(ai.target != null)
        {
            ai.ChangeState(ai.Move);
        }
    }

    public void Exit() { }
}
