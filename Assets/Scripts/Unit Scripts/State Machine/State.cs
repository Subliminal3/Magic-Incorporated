using Unity.VisualScripting;
using UnityEngine;

public abstract class State : ScriptableObject
{
    [Header("Visuals")]
    [Tooltip("The exact name of the Animation State node in the Animator window.")]
    [SerializeField] protected string animationName;
    
    public abstract State Tick(UnitController controller);
    public virtual void OnEnter(UnitController controller)
    {
        controller.PlayAnimation(animationName);
    }
    public virtual void OnExit(UnitController controller) { }
}
