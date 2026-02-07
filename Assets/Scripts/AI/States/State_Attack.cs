using UnityEngine;


[CreateAssetMenu(fileName = "State", menuName = "State Machine/Attack")]
public class State_Attack : State
{
    UnitController ai;

    //Stop the navmesh from moving
    public override void OnEnter(UnitController ai)
    {
        ai.Agent.isStopped = true;
        ai.nextAttackTime = 0;
        ai.isAttacking = true;
    }

    public override State Tick(UnitController ai)
    {
        //check for target
        if (ai.target == null)
            return ai.transitionStates.moveState;

        //Check if they move out of attack range
        float distance = Vector3.Distance(ai.transform.position, ai.target.transform.position);
        if (distance > ai.data.attackRange)
            return ai.transitionStates.moveState;

        //Reduce target hp on a timer
        ai.DealDamage();

        

        if(ai.target.isDead)
            return ai.transitionStates.moveState;

        return this;


    }

    public override void OnExit(UnitController ai)
    {
        ai.isAttacking = false;
    }





}
