#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepDiscardPaymentsEntries
{
    internal bool CanRunPrepDiscard(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunPrepDiscard(c, trigger, context);
    internal SkillProgramStepOutcome FinishPrepDiscard(ProgramSkillFrame f, bool completed) => _resolve().FinishPrepDiscard(f, completed);
    internal bool IsImmediatePrepDiscard(ProgramSkillFrame f) => _resolve().IsImmediatePrepDiscard(f);
    internal bool MatchesActualPrepDiscard(ProgramSkillFrame f) => _resolve().MatchesActualPrepDiscard(f);
    internal void PayPrepDiscard(ProgramSkillFrame f, int payer, bool ownerCost) => _resolve().PayPrepDiscard(f, payer, ownerCost);
    internal bool PrepDiscardCanContinue(ProgramSkillFrame f) => _resolve().PrepDiscardCanContinue(f);
    internal IReadOnlyList<PromptChoice> PrepDiscardCards(ProgramSkillFrame f, int payer, IReadOnlyList<int>? selected = default) => _resolve().PrepDiscardCards(f, payer, selected);
    internal IReadOnlyList<PromptChoice> PrepDiscardChoices(ProgramSkillFrame f) => _resolve().PrepDiscardChoices(f);
    internal Dictionary<string, string> PrepDiscardParameters(ProgramSkillFrame f, string step) => _resolve().PrepDiscardParameters(f, step);
    internal CardConversionSource PrepDiscardSource(ProgramSkillFrame f) => _resolve().PrepDiscardSource(f);
    internal SkillProgramStepOutcome PublishPrepDiscard(ProgramSkillFrame f) => _resolve().PublishPrepDiscard(f);
    internal void ResolvePrepDiscardChoice(ProgramSkillFrame f, PromptChoice choice) => _resolve().ResolvePrepDiscardChoice(f, choice);
    internal SkillProgramStepOutcome ResolvePrepDiscardOrEnding(ProgramSkillFrame supplied, string endingBinding) => _resolve().ResolvePrepDiscardOrEnding(supplied, endingBinding);
    internal PromptChoice SelectAiPrepDiscard(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiPrepDiscard(decision, f);
    internal bool TryReturnPrepDiscardMovement(ProgramSkillFrame f) => _resolve().TryReturnPrepDiscardMovement(f);
    internal long PrepDiscardSequence { get => _resolve().PrepDiscardSequence; }
}
