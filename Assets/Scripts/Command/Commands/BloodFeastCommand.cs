using Command.Actions;
using Command.Main;

public class BloodFeastCommand : UnitCommand
{
    private bool willHitTarget;

    int minionAmount = 0;
    int healAmount = 0;
    public BloodFeastCommand(CommandData commandData)
    {
        this.commandData = commandData;
        willHitTarget = WillHitTarget();
    }

    public override void Execute()
    {
        minionAmount = targetUnit.GetMinionCount();
        healAmount = targetUnit.GetBloodFeastAmount();
        GameService.Instance.ActionService.GetActionByType(ActionType.BloodFeast)
        .PerformAction(actorUnit, targetUnit, willHitTarget);
    }

    public override void Undo()
    {
        targetUnit.TakeDamage(healAmount);
        
        for (int i = 0; i < minionAmount; i++)
        {
            targetUnit.SpawnMinion();
        }
    }

    public override bool WillHitTarget() => true;
}