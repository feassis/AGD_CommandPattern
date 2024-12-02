using Command.Actions;
using Command.Main;
using UnityEngine;

public class CleanseCommand : UnitCommand
{
    private const float hitChance = 0.2f;
    private bool willHitTarget;

    private int preCleansePower;

    public CleanseCommand(CommandData commandData)
    {
        this.commandData= commandData;
        willHitTarget= WillHitTarget();
    }

    public override bool WillHitTarget() => Random.Range(0f, 1f) < hitChance;

    public override void Execute()
    {
        if(willHitTarget)
        {
            preCleansePower = targetUnit.CurrentPower;
        }

        GameService.Instance.ActionService.GetActionByType(ActionType.Cleanse)
            .PerformAction(actorUnit, targetUnit, willHitTarget);
    }

    public override void Undo()
    {
        if (willHitTarget)
        {
            targetUnit.CurrentPower = preCleansePower;
        }
    }
}
