using Command.Actions;
using Command.Main;
using UnityEngine;

public class BerserkAttackCommand : UnitCommand
{
    private bool willHitTarget;
    private const float hitChance = 0.66f;

    public BerserkAttackCommand(CommandData commandData)
    {
        this.commandData= commandData;
        willHitTarget = WillHitTarget();
    }

    public override bool WillHitTarget() => Random.Range(0f, 1f) < hitChance;

    public override void Execute()
    {
        GameService.Instance.ActionService.GetActionByType(ActionType.BerserkAttack)
            .PerformAction(actorUnit, targetUnit, willHitTarget);
    }

    public override void Undo()
    {
        if(willHitTarget)
        {
            targetUnit.RestoreHealth(actorUnit.CurrentPower * 2);
        }
    }
}
