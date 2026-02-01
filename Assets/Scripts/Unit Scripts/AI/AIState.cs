using UnityEngine;

public abstract class AIState : ScriptableObject
{
    public virtual void OnEnter(UnitController controller) { }
    public abstract AIState Tick(UnitController controller);
    public virtual void OnExit(UnitController controller) { }
}
