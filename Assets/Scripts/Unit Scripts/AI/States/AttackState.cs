using UnityEngine;

[CreateAssetMenu(fileName = "AttackState", menuName = "AI/States/Attack")]
public class AttackState : AIState
{
    public AIState ifTargetIsDead;
    public AIState ifTargetOutOfRange;
    private float lastAttackTime;

    public override AIState Tick(UnitController controller)
    {
        if (controller.Target == null || controller.Target.CurrentHealth <= 0)
        {
            controller.Target = null;
            controller.Agent.isStopped = false;
            return ifTargetIsDead; 
        }

        FaceTarget(controller);
        
        if (Time.time - lastAttackTime >= controller.data.attackCooldown)
        {
            PerformAttack(controller);
            lastAttackTime = Time.time;
        }
        
        if (controller.Target == null)
        {
            return ifTargetIsDead;
        }
        float distanceToTarget = Vector3.Distance(controller.transform.position, controller.Target.transform.position);
        if (distanceToTarget > controller.data.attackRange + 1f) 
        {
            controller.Agent.isStopped = false;
            return ifTargetOutOfRange;
        }
        return this;
    }

    private void FaceTarget(UnitController controller)
    {
        if (controller.Target == null) return;
        
        Vector3 direction = (controller.Target.transform.position - controller.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    private void PerformAttack(UnitController controller)
    {
        if (controller.Target == null) return;
        
        bool enemyDied = controller.Target.TakeDamage(controller.data.attackDamage);
        
        if (enemyDied)
        {
            Debug.Log($"{controller.name} defeated {controller.Target.name}!");
            controller.Target = null;
        }
    }
}
