using Command.Actions;

/**  This script demonstrates implementation of the Observer Pattern.
*  If you're interested in learning about Observer Pattern, 
*  you can find a dedicated course on Outscal's website.
*  Link: https://outscal.com/courses
**/

namespace Command.Events
{
    public class EventService
    {
        public GameEventController<int> OnBattleSelected { get; private set; }
        public GameEventController<ActionType> OnActionSelected { get; private set; }
        public GameEventController<MinionController> OnMinionDeath { get; private set; }
        public GameEventController OnReplayButtonClicked { get; private set; }

        public EventService()
        {
            OnBattleSelected = new GameEventController<int>();
            OnActionSelected = new GameEventController<ActionType>();
            OnMinionDeath = new GameEventController<MinionController>();
            OnReplayButtonClicked = new GameEventController();
        }
    }
}