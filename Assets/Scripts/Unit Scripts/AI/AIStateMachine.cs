using UnityEngine;

[CreateAssetMenu(fileName = "AIStateMachine", menuName = "AI/State Machine")]
public class AIStateMachine : ScriptableObject
{
    public AIState startingState;
}
