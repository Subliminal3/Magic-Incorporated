using UnityEngine;

[CreateAssetMenu(fileName = "StateMachine", menuName = "State Machine/State Machine")]
public class TransitionStates : ScriptableObject
{

    [Header("Transition States")]
    public State idleState;
    public State attackState;
    public State moveState;
}
