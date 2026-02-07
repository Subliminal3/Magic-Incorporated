using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitController : MonoBehaviour
{
    //[Header("Input Data")]
    //public StateMachine stateMachine;

    public TransitionStates transitionStates;

    [Header("Game Data")]
    public UnitData data;
    
    public UnitController target { get; set; }
    public int CurrentHealth { get; set; }
    public LayerMask enemyLayer;
    public UnitController defaultTarget;
    public bool isDead = false;
    public float nextAttackTime;
    public bool isAttacking = false;

    private State currentState;
    private NavMeshAgent agent;
    private bool cleanedUp = false;
    //if the unit is attacking it wont search for more enemies near by



    [SerializeField] State startingState;
    public NavMeshAgent Agent => agent;
    /*private Animator animator;
    public Animator Animator => animator;*/
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
        //animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //set target to default on start
        target = defaultTarget;
        if (data != null) Initialize(data);
        //if (stateMachine != null)
        //{
            currentState = startingState;
            if (currentState != null)
            {
                currentState.OnEnter(this);
            }
        //}
    }

    private void Update()
    {
        //if(IsDead()) return;

        //Runs change state and calls tick of 'startingState'
        if(currentState != null)
            ChangeState(currentState.Tick(this));

        //Need to put this on a timer so it doesnt run so often
        if(!isAttacking && currentState != null)
            target = FindNearestEnemy();
    }

    public void ChangeState(State nextState)
    {
        //Handle State Machine Tick
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

    /*public void PlayAnimation(string animationName, float transitionDuration = 0.1f)
    {
        if (string.IsNullOrEmpty(animationName)) { return; }
        int hash = Animator.StringToHash(animationName);
        Animator.CrossFade(hash, transitionDuration);
    }*/

    public void DealDamage()
    {
        if (target == null) return;

        float interval = 1f / Mathf.Max(data.attackSpeed, 0.01f);

        if (Time.time >= nextAttackTime)
        {
            target.TakeDamage(data.attackDamage);
            nextAttackTime = Time.time + interval;
        }

        if (target.isDead)
            target = defaultTarget;
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (CurrentHealth == 0)
            Die();
    }

    public void Die()
    {
        //check if dead
        if (CurrentHealth <= 0)
        {
            //isAttacking = false;   
            Debug.Log($"{name} has been defeated!");
            isDead = true;
            agent.isStopped = true;
            gameObject.SetActive(false);
            //add death anim here
        }
    }


//The forcecleanup method ensures the exit statement is called when the unit is destroyed (killed)
/*    void OnDisable() => ForceCleanup();
    void OnDestroy() => ForceCleanup();

    void ForceCleanup()
    {
        if (cleanedUp) return;
        cleanedUp = true;

        // ensure state exits even if destroyed
        currentState?.OnExit(this);

    }*/
    



    public void MoveTo(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }

    /*public bool HasReachedDestination()
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
    }*/


    

    public UnitController FindNearestEnemy()
    {
        
        Collider[] hits = Physics.OverlapSphere(transform.position, data.detectionRange, enemyLayer);

        UnitController nearest = defaultTarget;
        float nearestSqr = float.PositiveInfinity;
        Vector3 origin = transform.position;

        foreach (var hit in hits)
        {
            UnitController unit = hit.GetComponentInParent<UnitController>();
            if (unit == null || unit.transform == transform)
                continue;

            //Target found
            float sqr = (unit.transform.position - origin).sqrMagnitude;
            if (sqr < nearestSqr)
            {
                nearestSqr = sqr;
                nearest = unit;

            }
        }

        return nearest;
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