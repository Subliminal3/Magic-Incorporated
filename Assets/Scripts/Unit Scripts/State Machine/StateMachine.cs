using UnityEngine;

[CreateAssetMenu(fileName = "StateMachine", menuName = "State Machine/State Machine")]
public class StateMachine : ScriptableObject
{
    public State startingState;
}
