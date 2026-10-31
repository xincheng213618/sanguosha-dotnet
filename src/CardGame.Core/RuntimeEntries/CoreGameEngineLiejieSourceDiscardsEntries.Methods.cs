#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLiejieSourceDiscardsEntries
{
    internal void AssertLiejieSourceDiscard(ProgramSkillFrame frame, SkillProgramEffect paused) => _resolve().AssertLiejieSourceDiscard(frame, paused);
    internal SkillProgramStepOutcome BeginLiejieSourceDiscard(ProgramSkillFrame input, SkillProgramEffect effect) => _resolve().BeginLiejieSourceDiscard(input, effect);
    internal void CompleteLiejieSourceDiscard(ProgramSkillFrame frame, int count) => _resolve().CompleteLiejieSourceDiscard(frame, count);
    internal ProgramLiejieCostColorFrozenEvent ExactLiejiePaidColors(ProgramSkillFrame frame, string bind) => _resolve().ExactLiejiePaidColors(frame, bind);
    internal bool ExactLiejieParent(ProgramSkillFrame frame) => _resolve().ExactLiejieParent(frame);
    internal void FreezeLiejieDiscardColors(ProgramSkillFrame frame, string sourceBind, IReadOnlyList<int> selectedIds, IReadOnlyList<CardLocation> selectedLocations) => _resolve().FreezeLiejieDiscardColors(frame, sourceBind, selectedIds, selectedLocations);
    internal bool IsAtomicLiejieCostMove(ProgramSkillFrame frame, string sourceBind) => _resolve().IsAtomicLiejieCostMove(frame, sourceBind);
    internal bool IsLiejieDiscardDestination(CardMovementRecord movement) => _resolve().IsLiejieDiscardDestination(movement);
    internal bool IsLiejieSourceDiscardProgramDying() => _resolve().IsLiejieSourceDiscardProgramDying();
    internal IReadOnlyList<PromptChoice> LiejieDiscardChoices(ProgramSkillFrame frame) => _resolve().LiejieDiscardChoices(frame);
    internal bool LiejieInitialClauseFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().LiejieInitialClauseFirstChild(frame, child);
    internal bool LiejieSourceDiscardFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().LiejieSourceDiscardFirstChild(frame, child);
    internal ProgramSkillFrame? LiejieSourceDiscardObserverRoot() => _resolve().LiejieSourceDiscardObserverRoot();
    internal void PublishLiejieDiscard(ProgramSkillFrame frame) => _resolve().PublishLiejieDiscard(frame);
    internal void ResolveLiejieDiscardChoice(PromptChoice selected) => _resolve().ResolveLiejieDiscardChoice(selected);
    internal Card ResolveLiejieSlot(int source, PromptChoice choice) => _resolve().ResolveLiejieSlot(source, choice);
    internal bool ResumeLiejieSourceDiscard(long id) => _resolve().ResumeLiejieSourceDiscard(id);
    internal bool ReturnLiejieSourceDiscardMovement(ProgramSkillFrame frame) => _resolve().ReturnLiejieSourceDiscardMovement(frame);
    internal PromptChoice SelectAiLiejieDiscardChoice(PendingDecision decision) => _resolve().SelectAiLiejieDiscardChoice(decision);
    internal long LiejieMoveSequence { get => _resolve().LiejieMoveSequence; }
}
