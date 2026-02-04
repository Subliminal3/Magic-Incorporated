using UnityEngine;
using UnityEngine.AI;

public abstract class AIController : MonoBehaviour
{

    [Header("Detection Settings")]
    public float detectionRadius = 10f;
    

    [Header("Shared")]
    public NavMeshAgent agent;
    public float attackRange = 2f;
    public Transform target;

    public IState currentState; //protected

    //define the states
    private IdleState idle;
    private MoveState move;
    private AttackState attack;

    //Create read only versions for States to access
    public IState Idle => idle;
    public IState Move => move;
    public IState Attack => attack;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        idle = new IdleState(this);
        move = new MoveState(this);
        attack = new AttackState(this);

        ChangeState(idle);
    }
    protected virtual void Update()
    {

        FindTarget();
        currentState?.Tick();

        Debug.Log("Current State = " + currentState);
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    protected virtual void FindTarget()
    {
        
    }
}
