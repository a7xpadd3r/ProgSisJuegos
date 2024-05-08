using System.Collections.Generic;
using UnityEngine;

public class EventQueue : MonoBehaviour
{
    private List<ICommand> _currentCommands = new();
    private Stack<IDeletableCommand> _undoableCommands = new();

    private Dictionary<string, ICommand> _commandsOnDemand = new();

    public static EventQueue Instance { get; private set; }
    private static EventQueue _instance;

    public static EventQueue GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (Instance != null) 
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // no time to fix this
        //DontDestroyOnLoad(gameObject);
    }

    private void LateUpdate()
    {
        if (_currentCommands.Count == 0) return;

        foreach (var command in _currentCommands)
        {
            command.Execute();

            if (command is IDeletableCommand undoableCommand)
                _undoableCommands.Push(undoableCommand);
        }

        _currentCommands.Clear();
    }

    public void EnqueueCommand(ICommand command)
    {
        if (_currentCommands.Contains(command)) return;

        _currentCommands.Add(command);
    }

    public void EnqueueOnDemandCommand(string id, ICommand command)
    {
        if (_commandsOnDemand.ContainsKey(id)) return;

        _commandsOnDemand.Add(id, command);
        print("New OnDemand command: ID: '" + id + "'.");
    }

    public bool ExecuteOnDemandCommand(string id)
    {
        bool executed = false;
        ICommand command = _commandsOnDemand.GetValueOrDefault(id);

        if (command != null) 
        {
            command.Execute();
            _commandsOnDemand.Remove(id);
            executed = true;
        }

        print("OnDemandCommand: ID: '" + id + "' - execution = " + executed);
        return executed;
    }

    public ICommand GetOnDemandCommand(string id) 
    {
        return _commandsOnDemand.GetValueOrDefault(id);
    }

    public void UndoLatest()
    {
        if (_undoableCommands.Count == 0) return;
        IDeletableCommand command = _undoableCommands.Pop();
        command.Undo();
    }
}
