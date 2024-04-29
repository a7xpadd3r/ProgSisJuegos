
public abstract class StateBase 
{
    public abstract void OnEnterState();
    public abstract void OnExecute(float deltaTime, float turnSpeed = 1);
    public abstract void OnExitState();
}
