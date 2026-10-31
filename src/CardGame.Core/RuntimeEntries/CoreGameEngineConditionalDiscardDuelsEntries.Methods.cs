#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConditionalDiscardDuelsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginConditionalDiscardDuel(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginConditionalDiscardDuel(supplied);
    internal bool CanStartConditionalDiscardDuel(global::CardGame.Core.CharacterState owner) => _resolve().CanStartConditionalDiscardDuel(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ConditionalDuelChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ConditionalDuelChoices(f);
    internal bool ConditionalDuelParentMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ConditionalDuelParentMatches(f);
    internal bool ConditionalDuelSourceCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ConditionalDuelSourceCurrent(f);
    internal void ContinueConditionalDuelPaymentChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueConditionalDuelPaymentChildren(f);
    internal void FinishConditionalDiscardDuel(global::CardGame.Core.ProgramSkillFrame f, bool issued) => _resolve().FinishConditionalDiscardDuel(f, issued);
    internal void PublishConditionalDuelPayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishConditionalDuelPayment(f);
    internal void ResolveConditionalDuelPayment(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveConditionalDuelPayment(selected);
    internal bool ResumeConditionalDiscardDuel(long id) => _resolve().ResumeConditionalDiscardDuel(id);
    internal bool ReturnConditionalDuelMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnConditionalDuelMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiConditionalDuel(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiConditionalDuel(decision, f);
}
