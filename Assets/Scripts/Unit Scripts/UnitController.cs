using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitController : MonoBehaviour
{
    public UnitData data;
    private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;
    
    public UnitController Target { get; set; }
    public LayerMask enemyLayer;
    public int CurrentHealth { get; private set; }
    public Vector3 patrolPoint { get; set; }
    public bool hasPatrolPoint { get; set; }
    public float lastPatrolTime { get; set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        if (data != null) Initialize(data);
    }

    public void Initialize(UnitData unitData)
    {
        data = unitData;
        CurrentHealth = data.maxHealth;

        // Configure the Agent based on the Template
        agent.speed = data.speed;
        agent.angularSpeed = data.angularSpeed;
        agent.acceleration = data.acceleration;
        agent.stoppingDistance = data.stoppingDistance;
    }

    public bool TakeDamage(int damage)
    {
        if (CurrentHealth <= 0)
            return false; // Already dead

        CurrentHealth -= damage;
        Debug.Log($"{name} takes {damage} damage. Health: {CurrentHealth}/{data.maxHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    private void Die()
    {
        Debug.Log($"{name} has been defeated!");
        agent.isStopped = true;
        // You can add death animations, particle effects, etc. here
        // For now, we'll just disable the unit
        gameObject.SetActive(false);
    }

    public void MoveTo(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }
    
    // Helper to check if we reached the destination
    // NavMeshAgent can be tricky; pathPending is true while calculating the path.
    public bool HasReachedDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public UnitController FindNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, data.detectionRange, enemyLayer);
        UnitController nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;
        
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform != transform && hitCollider.GetComponent<UnitController>() != null)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = hitCollider.GetComponent<UnitController>();
                }
            }
        }
        return nearestEnemy;
    }
}