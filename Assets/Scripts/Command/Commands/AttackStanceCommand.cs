using Command.Actions;
using Command.Main;

public  class AttackStanceCommand : UnitCommand
{
    private bool willHitTarget;

    public AttackStanceCommand(CommandData commandData)
    {
        this.commandData= commandData;
        willHitTarget = WillHitTarget();
    }

    public override bool WillHitTarget() => true;

    public override void Execute()
    {
        GameService.Instance.ActionService.GetActionByType(ActionType.AttackStance)
            .PerformAction(actorUnit, targetUnit, willHitTarget);
    }

    public override void Undo()
    {
       if(willHitTarget)
        {
            targetUnit.UnpowerUp();
        }
    }
}
