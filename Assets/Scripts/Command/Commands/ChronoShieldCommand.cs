using Command.Actions;
using Command.Main;

public class ChronoShieldCommand : UnitCommand
{
    private bool willHitTarget;

    public ChronoShieldCommand(CommandData commandData)
    {
        this.commandData = commandData;
        willHitTarget = WillHitTarget();
    }

    public override bool WillHitTarget() => true;

    public override void Execute() => GameService.Instance.ActionService.GetActionByType(ActionType.ChronoShield)
        .PerformAction(actorUnit, targetUnit, willHitTarget);

    public override void Undo()
    {
        if (willHitTarget)
        {
            targetUnit.SubtractInvulnerabilityForNTurns(1);
        }
    }
}
