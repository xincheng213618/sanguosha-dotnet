#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepDiscardPaymentsEntries
{
    internal bool CanRunPrepDiscard(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunPrepDiscard(c, trigger, context);
    internal global::CardGame.Core.SkillProgramStepOutcome FinishPrepDiscard(global::CardGame.Core.ProgramSkillFrame f, bool completed) => _resolve().FinishPrepDiscard(f, completed);
    internal bool IsImmediatePrepDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsImmediatePrepDiscard(f);
    internal bool MatchesActualPrepDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MatchesActualPrepDiscard(f);
    internal void PayPrepDiscard(global::CardGame.Core.ProgramSkillFrame f, int payer, bool ownerCost) => _resolve().PayPrepDiscard(f, payer, ownerCost);
    internal bool PrepDiscardCanContinue(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PrepDiscardCanContinue(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PrepDiscardCards(global::CardGame.Core.ProgramSkillFrame f, int payer, global::System.Collections.Generic.IReadOnlyList<int>? selected = default) => _resolve().PrepDiscardCards(f, payer, selected);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PrepDiscardChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PrepDiscardChoices(f);
    internal global::System.Collections.Generic.Dictionary<string, string> PrepDiscardParameters(global::CardGame.Core.ProgramSkillFrame f, string step) => _resolve().PrepDiscardParameters(f, step);
    internal global::CardGame.Core.CardConversionSource PrepDiscardSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PrepDiscardSource(f);
    internal global::CardGame.Core.SkillProgramStepOutcome PublishPrepDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishPrepDiscard(f);
    internal void ResolvePrepDiscardChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => _resolve().ResolvePrepDiscardChoice(f, choice);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolvePrepDiscardOrEnding(global::CardGame.Core.ProgramSkillFrame supplied, string endingBinding) => _resolve().ResolvePrepDiscardOrEnding(supplied, endingBinding);
    internal global::CardGame.Core.PromptChoice SelectAiPrepDiscard(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiPrepDiscard(decision, f);
    internal bool TryReturnPrepDiscardMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TryReturnPrepDiscardMovement(f);
    internal long PrepDiscardSequence { get => _resolve().PrepDiscardSequence; }
}
