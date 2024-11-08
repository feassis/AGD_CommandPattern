﻿using Command.Input;
using Command.Main;
using Command.Player;
using UnityEngine;

namespace Command.Actions
{
    public class MinionFortressAction : IAction
    {
        private UnitController actorUnit;
        private UnitController targetUnit;
        private bool isSuccessful;
        public TargetType TargetType => TargetType.Self;

        public void PerformAction(UnitController actorUnit, UnitController targetUnit, bool isSuccessful)
        {
            this.actorUnit = actorUnit;
            this.targetUnit = targetUnit;
            this.isSuccessful = isSuccessful;

            actorUnit.PlayBattleAnimation(ActionType.MinionFortress, CalculateMovePosition(targetUnit), OnActionAnimationCompleted);
        }

        public void OnActionAnimationCompleted()
        {
            PlayAttackSound();

            if (isSuccessful)
                targetUnit.AddMinionShieldyForNTurns(1);
            else
                GameService.Instance.UIService.ActionMissed();
        }

        public Vector3 CalculateMovePosition(UnitController targetUnit) => targetUnit.GetEnemyPosition();

        private void PlayAttackSound()
        {
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.MEDITATE);
        }
    }
}