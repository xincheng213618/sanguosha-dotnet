#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateTurnHoldsEntries
{
    internal SkillProgramStepOutcome HoldOwnerHandUntilTurnEnd(ProgramSkillFrame f) => _resolve().HoldOwnerHandUntilTurnEnd(f);
    internal IReadOnlyList<PrivateTurnHoldSnapshot>? ProjectPrivateTurnHolds(int owner, int viewer, bool revealAll) => _resolve().ProjectPrivateTurnHolds(owner, viewer, revealAll);
    internal bool ResumePrivateTurnHold(long id) => _resolve().ResumePrivateTurnHold(id);
    internal void ReturnPrivateTurnHoldsAtTurnEnd() => _resolve().ReturnPrivateTurnHoldsAtTurnEnd();
    internal bool HasActualTurnEndMovementPrelude { get => _resolve().HasActualTurnEndMovementPrelude; }
}
