using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitController : MonoBehaviour
{
    [SerializeField] private UnitData data;
    public UnitData Data => data;
    
    private NavMeshAgent agent;
    public int CurrentHealth { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        // If data is assigned in inspector, load it. 
        // If you spawn units via code, you might call Initialize(data) manually.
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

    /// <summary>
    /// Takes damage from an attack. Returns true if the unit died.
    /// </summary>
    /// <param name="damage">Amount of damage to take</param>
    /// <returns>True if unit died, false otherwise</returns>
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
}