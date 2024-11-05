using Command.Player;
using System.Data;
/// <summary>
/// An abstract class representing a unit-related command.
/// </summary>
public abstract class UnitCommand : ICommand
{
    // Fields to store information related to the command.
    public CommandData commandData;

    // References to the actor and target units, accessible by subclasses.
    protected UnitController actorUnit;
    protected UnitController targetUnit;

    /// <summary>
    /// Abstract method to execute the unit command. Must be implemented by concrete subclasses.
    /// </summary>
    public abstract void Execute();

    public abstract void Undo();

    /// <summary>
    /// Abstract method to determine whether the command will successfully hit its target.
    /// Must be implemented by concrete subclasses.
    /// </summary>
    public abstract bool WillHitTarget();

    public void SetActorUnit(UnitController actorUnit) => this.actorUnit = actorUnit;

    public void SetTargetUnit(UnitController targetUnit) => this.targetUnit = targetUnit;
}
