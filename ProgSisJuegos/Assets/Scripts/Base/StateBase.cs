
public abstract class StateBase 
{
    public abstract void OnEnterState();
    public abstract void OnExecute(float deltaTime);
    public abstract void OnExitState();
}
