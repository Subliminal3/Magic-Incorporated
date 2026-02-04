using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitController : MonoBehaviour
{
    [Header("Input Data")]
    public StateMachine stateMachine;

    [Header("Game Data")]
    public UnitData data;
    public UnitController Target { get; set; }
    public LayerMask enemyLayer;
    public int CurrentHealth { get; private set; }
    public Vector3 patrolPoint { get; set; }
    public bool hasPatrolPoint { get; set; }
    public float lastPatrolTime { get; set; }
    public float lastAttackTime { get; set; }


    private State currentState;
    private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;
    private Animator animator;
    public Animator Animator => animator;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (data != null) Initialize(data);
        if (stateMachine != null)
        {
            currentState = stateMachine.startingState;
            if (currentState != null)
            {
                currentState.OnEnter(this);
            }
        }
    }

    private void Update()
    {
        //check if dead
        if (currentState == null || CurrentHealth <= 0)
        {
            if (CurrentHealth <= 0)
            {
                Agent.isStopped = true;
            }
            return;
        }

        //Handle State Machine Tick
        State nextState = currentState.Tick(this);
        if (nextState != null && nextState != currentState)
        {
            currentState.OnExit(this);
            currentState = nextState;
            currentState.OnEnter(this);
        }
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

    public void PlayAnimation(string animationName, float transitionDuration = 0.1f)
    {
        if (string.IsNullOrEmpty(animationName)) { return; }
        int hash = Animator.StringToHash(animationName);
        Animator.CrossFade(hash, transitionDuration);
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

    private void OnDrawGizmosSelected()
    {
        if (data != null)
        {
           Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, data.detectionRange);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, data.attackRange); 
        }
    }
}