using UnityEngine;


[CreateAssetMenu(fileName = "State", menuName = "State Machine/Attack")]
public class State_Attack : State
{
    UnitController ai;

    //constructor
    public State_Attack(UnitController ai)
    {
        this.ai = ai;
    }

    //Stop the navmesh from moving
    public override void OnEnter(UnitController ai)
    {
        ai.Agent.isStopped = true;

        if (ai.target != null)
        {
            //targetStats = ai.target.GetComponent<UnitStats>();
            //Debug.Log("Component found");
        }
    }

    public override State Tick(UnitController ai)
    {
        //check for target
        if (ai.target == null)
        {
            return ai.transitionStates.moveState;
        }

        float distance = Vector3.Distance(ai.transform.position, ai.target.transform.position);

        if (distance > ai.data.attackRange)
        {
            return ai.transitionStates.moveState;
        }

        return this;


    }





}
