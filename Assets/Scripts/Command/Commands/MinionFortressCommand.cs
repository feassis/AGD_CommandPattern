using Command.Actions;
using Command.Main;

public class MinionFortressCommand : UnitCommand
{
    private bool willHitTarget;

    public MinionFortressCommand(CommandData commandData)
    {
        this.commandData = commandData;
        willHitTarget = WillHitTarget();
    }

    public override void Execute()
    {
        GameService.Instance.ActionService.GetActionByType(ActionType.MinionFortress)
        .PerformAction(actorUnit, targetUnit, willHitTarget);
    }

    public override void Undo()
    {
        targetUnit.SubtractMinionShieldForNTurns(1);
    }

    public override bool WillHitTarget()
    {
        return true;
    }
}