using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnitController))]
public class AIController : MonoBehaviour
{
    public AIStateMachine stateMachine;
    
    [HideInInspector] public UnitController unitController;
    
    [SerializeField] private AIState currentState;
    private NavMeshAgent agent;

    private void Awake()
    {
        unitController = GetComponent<UnitController>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (stateMachine != null)
        {
            currentState = stateMachine.startingState;
            if (currentState != null)
            {
                currentState.OnEnter(unitController);
            }
        }
    }

    private void Update()
    {
        if (currentState == null || unitController.CurrentHealth <= 0)
        {
            if (unitController.CurrentHealth <= 0)
            {
                agent.isStopped = true;
            }
            return;
        }

        AIState nextState = currentState.Tick(unitController);
        if (nextState != null && nextState != currentState)
        {
            currentState.OnExit(unitController);
            currentState = nextState;
            currentState.OnEnter(unitController);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Awake();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, unitController.data.detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unitController.data.attackRange);
    }
}
