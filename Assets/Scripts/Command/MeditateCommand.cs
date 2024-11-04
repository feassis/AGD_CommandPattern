using Command.Actions;
using Command.Main;

public class MeditateCommand : UnitCommand
{
    private bool willHitTarget;

    public MeditateCommand(CommandData commandData)
    {
        this.commandData= commandData;
        willHitTarget= WillHitTarget();
    }

    public override bool WillHitTarget() => true;

    public override void Execute()
    {
        GameService.Instance.ActionService.GetActionByType(ActionType.Meditate)
            .PerformAction(actorUnit, targetUnit, willHitTarget);
    }
}
