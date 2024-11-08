using Command.Input;
using System.Collections.Generic;

namespace Command.Actions
{
    public class ActionService
    {
        private Dictionary<ActionType, IAction> actions;

        public ActionService() => CreateActions();

        private void CreateActions()
        {
            actions = new Dictionary<ActionType, IAction>();
            actions.Add(ActionType.Attack, new AttackAction());
            actions.Add(ActionType.Heal, new HealAction());
            actions.Add(ActionType.AttackStance, new AttackStanceAction());
            actions.Add(ActionType.Cleanse, new CleanseAction());
            actions.Add(ActionType.Meditate, new MeditateAction());
            actions.Add(ActionType.BerserkAttack, new BerserkAttackAction());
            actions.Add(ActionType.ThirdEye, new ThirdEyeAction());
            actions.Add(ActionType.ChronoShield, new ChronoShieldAction());
            actions.Add(ActionType.TimeRipple, new TimeRippleAction());
            actions.Add(ActionType.SummonMinion, new SummonMinionAction());
            actions.Add(ActionType.MinionFortress, new MinionFortressAction());
            actions.Add(ActionType.BloodFeast, new BloodFeastAction());
        }

        public IAction GetActionByType(ActionType type)
        {
            if (actions.ContainsKey(type))
                return actions[type];
            else
                throw new System.Exception($"No Action found for the type {type} in the dictionary");
        }

        public TargetType GetTargetTypeForAction(ActionType actionType) => actions[actionType].TargetType;
    }
}