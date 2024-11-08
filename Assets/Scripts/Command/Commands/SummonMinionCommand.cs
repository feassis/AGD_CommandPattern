using Command.Actions;
using Command.Main;

public class SummonMinionCommand : UnitCommand
{
    private bool willHitTarget;
    public SummonMinionCommand(CommandData commandData)
    {
        this.commandData = commandData;
        willHitTarget = WillHitTarget();
    }

    public override void Execute()
    {
        GameService.Instance.ActionService.GetActionByType(ActionType.SummonMinion)
        .PerformAction(actorUnit, targetUnit, willHitTarget);
    }

    public override void Undo()
    {
        int atk = actorUnit.GetAllMinionsAttackPower();
        targetUnit.RestoreHealth(atk);
        actorUnit.KillLastMinion();

    }

    public override bool WillHitTarget()
    {
        return true;
    }
}