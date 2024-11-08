using Command.Input;
using Command.Main;
using Command.Player;
using UnityEngine;

namespace Command.Actions
{
    public class SummonMinionAction : IAction
    {
        private UnitController actorUnit;
        private UnitController targetUnit;
        private bool isSuccessful;
        public TargetType TargetType => TargetType.Enemy;

        public void PerformAction(UnitController actorUnit, UnitController targetUnit, bool isSuccessful)
        {
            this.actorUnit = actorUnit;
            this.targetUnit = targetUnit;
            this.isSuccessful = isSuccessful;

            actorUnit.PlayBattleAnimation(ActionType.Attack, CalculateMovePosition(targetUnit), OnActionAnimationCompleted);
        }

        public Vector3 CalculateMovePosition(UnitController targetUnit) => targetUnit.GetEnemyPosition();


        public void OnActionAnimationCompleted()
        {
            PlayAttackSound();

            if(isSuccessful)
            {
                actorUnit.SpawnMinion();
                int atk = actorUnit.GetAllMinionsAttackPower();

                targetUnit.TakeDamage(atk);
            }
            else
            {
                GameService.Instance.UIService.ActionMissed();
            }

            actorUnit.OnActionExecuted();
        }

        private void PlayAttackSound()
        {
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.MAGIC_BALL);
        }
    }
}