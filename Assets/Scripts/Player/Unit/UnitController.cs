using UnityEngine;
using Command.Main;
using Command.Actions;
using System.Collections;
using System;
using Object = UnityEngine.Object;

namespace Command.Player
{
    public class UnitController
    {
        public PlayerController Owner { get; private set; }
        private UnitScriptableObject unitScriptableObject;
        private UnitView unitView;

        public int UnitID { get; private set; }
        public UnitType UnitType => unitScriptableObject.UnitType;
        public int CurrentHealth { get; private set; }
        public UnitUsedState UsedState { get; private set; }

        private int turnsOfInvulnerability = 0;
        
        private UnitAliveState aliveState;
        private Vector3 originalPosition;
        public int CurrentPower;
        public int CurrentMaxHealth;

        public UnitController(PlayerController owner, UnitScriptableObject unitScriptableObject, Vector3 unitPosition, int unitId, int playerID)
        {
            Owner = owner;
            this.unitScriptableObject = unitScriptableObject;
            UnitID = playerID % 2 == 0 ?  unitId : -unitId;
            originalPosition = unitPosition;

            InitializeView(unitPosition);
            InitializeVariables();
        }

        private void InitializeView(Vector3 positionToSet)
        {
            unitView = Object.Instantiate(unitScriptableObject.UnitPrefab);
            unitView.Controller = this;
            unitView.transform.position = positionToSet;
            unitView.SetUnitIndicator(false);
        }

        private void InitializeVariables()
        {
            CurrentMaxHealth = CurrentHealth = unitScriptableObject.MaxHealth;
            CurrentPower = unitScriptableObject.Power;
            SetAliveState(UnitAliveState.ALIVE);
            SetUsedState(UnitUsedState.NOT_USED);
        }

        public void StartUnitTurn()
        {
            if(turnsOfInvulnerability >0)
            {
                turnsOfInvulnerability--;
            }

            unitView.SetUnitIndicator(true);
            GameService.Instance.UIService.ShowActionOverlay(Owner.PlayerID);
            GameService.Instance.UIService.ShowActionSelectionView(unitScriptableObject.executableCommands);
            GameService.Instance.UIService.SetActionContainerAlignment(Owner.PlayerID);
        }

        public void ProcessUnitCommand(UnitCommand commandToProcess) => GameService.Instance.CommandInvoker.ProcessCommand(commandToProcess);

        private void SetAliveState(UnitAliveState stateToSet) => aliveState = stateToSet;

        public void SetUsedState(UnitUsedState stateToSet) => UsedState = stateToSet;

        public bool IsAlive() => aliveState == UnitAliveState.ALIVE;

        public void TakeDamage(int damageToTake)
        {
            if(turnsOfInvulnerability > 0)
            {
                return;
            }

            CurrentHealth -= damageToTake;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                UnitDied();
            }
            else
                unitView.PlayAnimation(UnitAnimations.HIT);

            unitView.UpdateHealthBar((float) CurrentHealth / CurrentMaxHealth);
        }

        public void AddInvulnerabilityForNTurns(int turns)
        {
            turnsOfInvulnerability += turns;
        }

        public void SubtractInvulnerabilityForNTurns(int turns)
        {
            turnsOfInvulnerability -= turns;
        }

        public void PowerUp()
        {
            CurrentPower += (int)(CurrentPower * 0.4f);
        }

        public void UnpowerUp()
        {
            var previousPower = CurrentPower/1.4f;

            CurrentPower -= (int)(CurrentPower - previousPower);
        }

        public void RestoreHealth(int healthToRestore)
        {
            if (turnsOfInvulnerability > 0)
            {
                return;
            }

            CurrentHealth = CurrentHealth + healthToRestore > CurrentMaxHealth ? CurrentMaxHealth : CurrentHealth + healthToRestore;
            unitView.UpdateHealthBar((float)CurrentHealth / CurrentMaxHealth);
        }

        private void UnitDied()
        {
            SetAliveState(UnitAliveState.DEAD);
            unitView.PlayAnimation(UnitAnimations.DEATH);
        }

        public void PlayBattleAnimation(ActionType actionType, Vector3 battlePosition, Action callback)
        {
            GameService.Instance.UIService.ResetBattleBackgroundOverlay();
            MoveToBattlePosition(battlePosition, callback, true, actionType);
        }

        private void MoveToBattlePosition(Vector3 battlePosition, Action callback = null,  bool shouldPlayActionAnimation = true, ActionType actionTypeToExecute = ActionType.None)
        {
            float moveTime = Vector3.Distance(unitView.transform.position, battlePosition) / unitScriptableObject.MovementSpeed;
            unitView.StartCoroutine(MoveToPositionOverTime(battlePosition, moveTime, callback, shouldPlayActionAnimation, actionTypeToExecute));
        }

        private IEnumerator MoveToPositionOverTime(Vector3 targetPosition, float time, Action callback, bool shouldPlayActionAnimation, ActionType actionTypeToExecute)
        {
            float elapsedTime = 0;
            Vector3 startingPosition = unitView.transform.position;

            while (elapsedTime < time)
            {
                unitView.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            unitView.transform.position = targetPosition;

            if (shouldPlayActionAnimation)
                PlayActionAnimation(actionTypeToExecute);

            if (callback != null)
                callback.Invoke();
        }

        private void PlayActionAnimation(ActionType actionType)
        {
            if (actionType == ActionType.None)
                return;

            for (int i = 0; i < unitScriptableObject.executableCommands.Count; i++)
            {
                if(actionType == unitScriptableObject.executableCommands[i])
                {
                    unitView.PlayAnimation(i % 2 == 0 ? UnitAnimations.ACTION1 : UnitAnimations.ACTION2);
                    break;
                }
            }
        }

        public void OnActionExecuted()
        {
            MoveToBattlePosition(originalPosition, null, false);
            SetUsedState(UnitUsedState.USED);
            Owner.OnUnitTurnEnded();
            unitView.SetUnitIndicator(false);
        }

        public void ResetStats() => CurrentPower = unitScriptableObject.Power;

        public void Revive()
        {
            SetAliveState(UnitAliveState.ALIVE);
            unitView.PlayAnimation(UnitAnimations.IDLE);
        }

        public void Destroy() => UnityEngine.Object.Destroy(unitView.gameObject);

        public void ResetUnitIndicator() => unitView.SetUnitIndicator(false);

        public Vector3 GetEnemyPosition() 
        {
            if (Owner.PlayerID == 1)
                return unitView.transform.position + unitScriptableObject.EnemyBattlePositionOffset;
            else
                return unitView.transform.position - unitScriptableObject.EnemyBattlePositionOffset;
        }
    }

    public enum UnitUsedState
    {
        USED,
        NOT_USED
    }

    public enum UnitAliveState
    {
        ALIVE,
        DEAD
    }
}