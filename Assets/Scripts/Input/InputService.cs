using Command.Main;
using Command.Player;
using Command.Actions;

namespace Command.Input
{
    public class InputService
    {
        private MouseInputHandler mouseInputHandler;

        private InputState currentState;
        private ActionType selectedActionType;
        private TargetType targetType;

        public InputService()
        {
            mouseInputHandler = new MouseInputHandler(this);
            SetInputState(InputState.INACTIVE);
            SubscribeToEvents();
        }

        public void SetInputState(InputState inputStateToSet) => currentState = inputStateToSet;

        private void SubscribeToEvents() => GameService.Instance.EventService.OnActionSelected.AddListener(OnActionSelected);

        public void UpdateInputService()
        {
            if(currentState == InputState.SELECTING_TARGET)
                mouseInputHandler.HandleTargetSelection(targetType);
        }

        public void OnActionSelected(ActionType selectedActionType)
        {
            this.selectedActionType = selectedActionType;
            SetInputState(InputState.SELECTING_TARGET);
            TargetType targetType = SetTargetType(selectedActionType);
            ShowTargetSelectionUI(targetType);
        }

        private void ShowTargetSelectionUI(TargetType selectedTargetType)
        {
            int playerID = GameService.Instance.PlayerService.ActivePlayerID;
            GameService.Instance.UIService.ShowTargetOverlay(playerID, selectedTargetType);
        }

        private TargetType SetTargetType(ActionType selectedActionType) => targetType = GameService.Instance.ActionService.GetTargetTypeForAction(selectedActionType);

        public void OnTargetSelected(UnitController targetUnit)
        {
            // Set the input state to EXECUTING_INPUT when a target unit is selected.
            SetInputState(InputState.EXECUTING_INPUT);

            // Create a UnitCommand based on the selected target unit.
            UnitCommand commandToProcess = CreateUnitCommand(targetUnit);

            // This line passes the created command to the GameService for further Processsing.
            GameService.Instance.ProcessUnitCommand(commandToProcess);
        }

        private CommandData CreateCommandData(UnitController targetUnit)
        {
            // Create CommandData with the necessary information for a UnitCommand.
            // It includes the ActiveUnit's ID, TargetUnit's ID, ActivePlayer's ID, and the TargetPlayer's ID.
            return new CommandData(
                GameService.Instance.PlayerService.ActiveUnitID,
                targetUnit.UnitID,
                GameService.Instance.PlayerService.ActivePlayerID,
                targetUnit.Owner.PlayerID
            );
        }

        private UnitCommand CreateUnitCommand(UnitController targetUnit)
        {
            // Create the necessary CommandData based on the target unit.
            CommandData commandData = CreateCommandData(targetUnit);

            // Based on the selected command type, create and return the corresponding UnitCommand.
            switch (selectedActionType)
            {
                case ActionType.Attack:
                    return new AttackCommand(commandData);
                case ActionType.Heal:
                    return new HealCommand(commandData);
                case ActionType.AttackStance:
                    return new AttackStanceCommand(commandData);
                case ActionType.Cleanse:
                    return new CleanseCommand(commandData);
                case ActionType.BerserkAttack:
                    return new BerserkAttackCommand(commandData);
                case ActionType.Meditate:
                    return new MeditateCommand(commandData);
                case ActionType.ThirdEye:
                    return new ThirdEyeCommand(commandData);
                default:
                    // If the selectedCommandType is not recognized, throw an exception.
                    throw new System.Exception($"No Command found of type: {selectedActionType}");
            }
        }
    }
}