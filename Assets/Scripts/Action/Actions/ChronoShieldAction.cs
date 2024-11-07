using Command.Input;
using Command.Main;
using Command.Player;
using UnityEngine;

namespace Command.Actions
{
    public class ChronoShieldAction : IAction
    {
        public TargetType TargetType => TargetType.Self;

        public Vector3 CalculateMovePosition(UnitController targetUnit) => targetUnit.GetEnemyPosition();

        private UnitController actorUnit;
        private UnitController targetUnit;
        private bool isSuccessful;

        public void PerformAction(UnitController actorUnit, UnitController targetUnit, bool isSuccessful)
        {
            this.actorUnit = actorUnit;
            this.targetUnit = targetUnit;
            this.isSuccessful = isSuccessful;

            actorUnit.PlayBattleAnimation(ActionType.ChronoShield, CalculateMovePosition(targetUnit), OnActionAnimationCompleted);
        }

        public void OnActionAnimationCompleted()
        {
            PlayAttackSound();

            if(isSuccessful)
            {
                targetUnit.AddInvulnerabilityForNTurns(1);
            }
            else
            {
                GameService.Instance.UIService.ActionMissed();
            }
        }

        private void PlayAttackSound()
        {
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.MEDITATE);
        }
    }
}