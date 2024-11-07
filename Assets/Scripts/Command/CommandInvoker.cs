using Command.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class responsible for invoking and managing commands.
/// </summary>
public class CommandInvoker
{
    // A stack to keep track of executed commands.
    private Stack<ICommand> commandRegistry = new Stack<ICommand>();


    public CommandInvoker() => SubscribeToEvents();

    private void SubscribeToEvents() => GameService.Instance.EventService.OnReplayButtonClicked.AddListener(SetReplayStack);

    public void SetReplayStack()
    {
        GameService.Instance.ReplayService.SetCommandStack(commandRegistry);
        commandRegistry.Clear();
    }


    /// <summary>
    /// Process a command, which involves both executing it and registering it.
    /// </summary>
    /// <param name="commandToProcess">The command to be processed.</param>
    public void ProcessCommand(ICommand commandToProcess)
    {
        GameService.Instance.StartCoroutine(ExecComandRoutine(commandToProcess));   
    }

    private IEnumerator ExecComandRoutine(ICommand commandToProcess)
    {
        yield return ExecuteCommand(commandToProcess);
        RegisterCommand(commandToProcess);
    }

    /// <summary>
    /// Execute a command, invoking its associated action.
    /// </summary>
    /// <param name="commandToExecute">The command to be executed.</param>
    public IEnumerator ExecuteCommand(ICommand commandToExecute)
    {
        commandToExecute.Execute();

        yield return new WaitForSeconds(1.5f);
    }

    /// <summary>
    /// Register a command by adding it to the command registry stack.
    /// </summary>
    /// <param name="commandToRegister">The command to be registered.</param>
    public void RegisterCommand(ICommand commandToRegister) => commandRegistry.Push(commandToRegister);

    private bool RegistryEmpty() => commandRegistry.Count == 0;

    private bool CommandBelongsToActivePlayer()
    {
        return (commandRegistry.Peek() as UnitCommand).commandData.ActorPlayerID == GameService.Instance.PlayerService.ActivePlayerID;
    }

    public void Undo()
    {
        if (!RegistryEmpty() && CommandBelongsToActivePlayer())
            commandRegistry.Pop().Undo();
    }

    public void UndoEveryonesLastTurn()
    {
        if (RegistryEmpty())
        {
            return;
        }

        var currentActorID = (commandRegistry.Peek() as UnitCommand).commandData.ActorUnitID;

        commandRegistry.Pop().Undo();

        while(!RegistryEmpty() && (commandRegistry.Peek() as UnitCommand).commandData.ActorPlayerID != currentActorID)
        {
            commandRegistry.Pop().Undo();
        }
    }

    public List<ICommand> GetEveryOneLastAction()
    {
        if (RegistryEmpty())
        {
            return null;
        }

        var currentActorID = (commandRegistry.Peek() as UnitCommand).commandData.ActorUnitID;

        var commandStackArray = commandRegistry.ToArray() ;

        int index = commandStackArray.Length - 2;

        while(currentActorID != ((commandStackArray[index] as UnitCommand).commandData.ActorUnitID) && index >=0)
        {
            index--;
        }

        List<ICommand> result = new List<ICommand>();

        for(int i = index; i< commandStackArray.Length; i++)
        {
            result.Add(commandStackArray[i]);
        }
        result.Reverse();
        return result;
    }
}