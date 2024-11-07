using Command.Actions;
using Command.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRippleCommand : UnitCommand
{
    private bool willHitTarget;
    
    private List<ICommand> commands;

    public TimeRippleCommand(CommandData commandData)
    {
        this.commandData = commandData;
        willHitTarget = WillHitTarget();
    }
    
    public override void Execute()
    {
        commands = GameService.Instance.CommandInvoker.GetEveryOneLastAction();
        GameService.Instance.ActionService.GetActionByType(ActionType.TimeRipple)
         .PerformAction(actorUnit, targetUnit, willHitTarget);
    }

    public override void Undo()
    {
        GameService.Instance.StartCoroutine(UndoRotine());
    }

    private IEnumerator UndoRotine()
    {
        foreach (var command in commands)
        {
            GameService.Instance.CommandInvoker.ProcessCommand(command);
            yield return new WaitForSeconds(1.5f);
        }
    }

    public override bool WillHitTarget()
    {
        return true;
    }
}