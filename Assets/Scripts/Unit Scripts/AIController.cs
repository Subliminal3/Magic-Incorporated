using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIController : UnitController
{
    public AIStateMachine stateMachine;
    
    private AIState currentState;
    private NavMeshAgent agent;

    protected override void Start()
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

        base.Start();
    }

    private void Update()
    {
        if (currentState == null || CurrentHealth <= 0)
        {
            if (CurrentHealth <= 0)
            {
                agent.isStopped = true;
            }
            return;
        }

        AIState nextState = currentState.Tick(this);
        if (nextState != null && nextState != currentState)
        {
            currentState.OnExit(this);
            currentState = nextState;
            currentState.OnEnter(this);
        }
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
