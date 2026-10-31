#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateTurnHoldsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome HoldOwnerHandUntilTurnEnd(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HoldOwnerHandUntilTurnEnd(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PrivateTurnHoldSnapshot>? ProjectPrivateTurnHolds(int owner, int viewer, bool revealAll) => _resolve().ProjectPrivateTurnHolds(owner, viewer, revealAll);
    internal bool ResumePrivateTurnHold(long id) => _resolve().ResumePrivateTurnHold(id);
    internal void ReturnPrivateTurnHoldsAtTurnEnd() => _resolve().ReturnPrivateTurnHoldsAtTurnEnd();
    internal bool HasActualTurnEndMovementPrelude { get => _resolve().HasActualTurnEndMovementPrelude; }
}
