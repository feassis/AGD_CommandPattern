using Command.Actions;
using Command.Main;

public class ThirdEyeCommand : UnitCommand
{
    private bool willHitTarget;

    public ThirdEyeCommand(CommandData commandData)
    {
        this.commandData = commandData;
        willHitTarget = WillHitTarget();
    }

    public override bool WillHitTarget() => true;

    public override void Execute()
    {
        GameService.Instance.ActionService.GetActionByType(ActionType.ThirdEye)
            .PerformAction(actorUnit, targetUnit, willHitTarget);
    }
}