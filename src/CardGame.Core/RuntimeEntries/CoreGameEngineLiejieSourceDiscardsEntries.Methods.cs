#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLiejieSourceDiscardsEntries
{
    internal void AssertLiejieSourceDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertLiejieSourceDiscard(frame, paused);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLiejieSourceDiscard(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginLiejieSourceDiscard(input, effect);
    internal void CompleteLiejieSourceDiscard(global::CardGame.Core.ProgramSkillFrame frame, int count) => _resolve().CompleteLiejieSourceDiscard(frame, count);
    internal global::CardGame.Core.ProgramLiejieCostColorFrozenEvent ExactLiejiePaidColors(global::CardGame.Core.ProgramSkillFrame frame, string bind) => _resolve().ExactLiejiePaidColors(frame, bind);
    internal bool ExactLiejieParent(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExactLiejieParent(frame);
    internal void FreezeLiejieDiscardColors(global::CardGame.Core.ProgramSkillFrame frame, string sourceBind, global::System.Collections.Generic.IReadOnlyList<int> selectedIds, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation> selectedLocations) => _resolve().FreezeLiejieDiscardColors(frame, sourceBind, selectedIds, selectedLocations);
    internal bool HasLiejieSourceDiscardDamageObserver(long id) => _resolve().HasLiejieSourceDiscardDamageObserver(id);
    internal bool IsAtomicLiejieCostMove(global::CardGame.Core.ProgramSkillFrame frame, string sourceBind) => _resolve().IsAtomicLiejieCostMove(frame, sourceBind);
    internal bool IsLiejieAwaitedMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsLiejieAwaitedMovement(frame, effect, pending);
    internal bool IsLiejieDiscardDestination(global::CardGame.Core.CardMovementRecord movement) => _resolve().IsLiejieDiscardDestination(movement);
    internal bool IsLiejieSourceDiscardProgramDying() => _resolve().IsLiejieSourceDiscardProgramDying();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LiejieDiscardChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().LiejieDiscardChoices(frame);
    internal bool LiejieInitialClauseFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().LiejieInitialClauseFirstChild(frame, child);
    internal bool LiejieSourceDiscardFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().LiejieSourceDiscardFirstChild(frame, child);
    internal global::CardGame.Core.ProgramSkillFrame? LiejieSourceDiscardObserverRoot() => _resolve().LiejieSourceDiscardObserverRoot();
    internal void PublishLiejieDiscard(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishLiejieDiscard(frame);
    internal void ResolveLiejieDiscardChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveLiejieDiscardChoice(selected);
    internal global::CardGame.Core.Card ResolveLiejieSlot(int source, global::CardGame.Core.PromptChoice choice) => _resolve().ResolveLiejieSlot(source, choice);
    internal bool ResumeLiejieSourceDiscard(long id) => _resolve().ResumeLiejieSourceDiscard(id);
    internal bool ReturnLiejieSourceDiscardMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnLiejieSourceDiscardMovement(frame);
    internal global::CardGame.Core.PromptChoice SelectAiLiejieDiscardChoice(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiLiejieDiscardChoice(decision);
    internal long LiejieMoveSequence { get => _resolve().LiejieMoveSequence; }
}
