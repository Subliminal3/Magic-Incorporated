using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnitController))]
public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private LayerMask enemyLayer; // Layer to detect enemy units
    [SerializeField] private float detectionRange = 10f; // How far to detect enemies
    [SerializeField] private float attackRange = 2f; // How close to attack
    [SerializeField] private float attackCooldown = 1.5f; // Time between attacks
    
    [Header("AI Behavior")]
    [SerializeField] private float patrolRadius = 5f; // Radius for random patrolling
    [SerializeField] private float patrolWaitTime = 2f; // Time to wait at patrol point
    
    private UnitController unitController;
    private NavMeshAgent agent;
    private UnitController target;
    private float lastAttackTime;
    private float lastPatrolTime;
    private Vector3 patrolPoint;
    private bool hasPatrolPoint = false;
    
    private enum AIState
    {
        Patrol,
        Chase,
        Attack,
        Dead
    }
    
    private AIState currentState = AIState.Patrol;

    private void Awake()
    {
        unitController = GetComponent<UnitController>();
        agent = GetComponent<NavMeshAgent>();
        lastAttackTime = -attackCooldown; // Allow immediate attack if enemy is in range
        SetNewPatrolPoint();
    }

    private void Update()
    {
        if (unitController.CurrentHealth <= 0)
        {
            currentState = AIState.Dead;
            agent.isStopped = true;
            return;
        }

        switch (currentState)
        {
            case AIState.Patrol:
                HandlePatrol();
                break;
            case AIState.Chase:
                HandleChase();
                break;
            case AIState.Attack:
                HandleAttack();
                break;
        }
    }

    private void HandlePatrol()
    {
        // Look for enemies while patrolling
        target = FindNearestEnemy();
        
        if (target != null)
        {
            currentState = AIState.Chase;
            return;
        }

        // Handle patrolling logic
        if (!hasPatrolPoint || Vector3.Distance(transform.position, patrolPoint) < agent.stoppingDistance)
        {
            if (Time.time - lastPatrolTime > patrolWaitTime)
            {
                SetNewPatrolPoint();
            }
        }
        
        agent.SetDestination(patrolPoint);
    }

    private void HandleChase()
    {
        if (target == null || target.CurrentHealth <= 0)
        {
            target = null;
            currentState = AIState.Patrol;
            return;
        }

        // Check if enemy is in attack range
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        
        if (distanceToTarget <= attackRange)
        {
            currentState = AIState.Attack;
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
        }
    }

    private void HandleAttack()
    {
        if (target == null || target.CurrentHealth <= 0)
        {
            ResetState();
            return;
        }

        // Always face the target
        FaceTarget();
        
        // Attack if cooldown is ready
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
        
        if (target == null) { ResetState(); return; }
        // Check if target moved out of range
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToTarget > attackRange + 1f) // Add small buffer
        {
            currentState = AIState.Chase;
            agent.isStopped = false;
        }
    }

    private void ResetState()
    {
        target = null;
        currentState = AIState.Patrol;
        agent.isStopped = false;
    }

    private UnitController FindNearestEnemy()
    {
        // Find all units in detection range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);
        
        UnitController nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;
        
        foreach (var hitCollider in hitColliders)
        {
            // Make sure it's not this unit and has UnitController
            if (hitCollider.transform != transform && hitCollider.GetComponent<UnitController>() != null)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                
                // Check if this enemy is closer than the current nearest
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = hitCollider.GetComponent<UnitController>();
                }
            }
        }
        
        return nearestEnemy;
    }

    private void SetNewPatrolPoint()
    {
        // Generate random point within patrol radius
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * patrolRadius;
        randomPoint.y = transform.position.y; // Keep at same height
        
        patrolPoint = randomPoint;
        hasPatrolPoint = true;
        lastPatrolTime = Time.time;
    }

    private void FaceTarget()
    {
        if (target == null) return;
        
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    private void PerformAttack()
    {
        if (target == null) return;
        
        // Apply damage based on this unit's attack damage
        bool enemyDied = target.TakeDamage(unitController.Data.attackDamage);
        
        if (enemyDied)
        {
            Debug.Log($"{name} defeated {target.name}!");
            target = null;
            currentState = AIState.Patrol;
        }
    }

    // Visualize detection and attack ranges in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        if (hasPatrolPoint)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(patrolPoint, 0.5f);
        }
    }
}