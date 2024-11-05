using Command.Actions;
using Command.Main;

public class ThirdEyeCommand : UnitCommand
{
    private bool willHitTarget;
    private int previousPower;
    private int previousHealth;

    public ThirdEyeCommand(CommandData commandData)
    {
        this.commandData = commandData;
        willHitTarget = WillHitTarget();
    }

    public override bool WillHitTarget() => true;

    public override void Execute()
    {
        if(willHitTarget)
        {
            previousPower = targetUnit.CurrentPower;
            previousHealth = targetUnit.CurrentHealth;
        }

        GameService.Instance.ActionService.GetActionByType(ActionType.ThirdEye)
            .PerformAction(actorUnit, targetUnit, willHitTarget);
    }

    public override void Undo()
    {
        if (willHitTarget)
        {
            targetUnit.RestoreHealth(previousHealth - targetUnit.CurrentHealth);
            targetUnit.CurrentPower = previousPower;
        }
    }
}