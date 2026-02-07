using UnityEngine;


[CreateAssetMenu(fileName = "State", menuName = "State Machine/Attack")]
public class State_Attack : State
{
    UnitController ai;
    private float nextAttackTime;

    //Stop the navmesh from moving
    public override void OnEnter(UnitController ai)
    {
        ai.Agent.isStopped = true;
        nextAttackTime = 0f; // attack immediately on enter

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
        float interval = 1f / Mathf.Max(ai.data.attackSpeed, 0.01f);

        if(Time.time >= nextAttackTime)
        {
            ai.DealDamage(ai.data.attackDamage);
            nextAttackTime = Time.time + interval;
        }

        return this;


    }






}
