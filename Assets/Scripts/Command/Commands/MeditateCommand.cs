using Command.Actions;
using Command.Main;
using UnityEngine;
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

    public override void Undo()
    {
        if(willHitTarget)
        {
            var healthToDecrease = Mathf.RoundToInt(targetUnit.CurrentMaxHealth - targetUnit.CurrentHealth / 1.2f);
            targetUnit.CurrentMaxHealth -= healthToDecrease;
            targetUnit.TakeDamage(healthToDecrease);
        }
    }
}
