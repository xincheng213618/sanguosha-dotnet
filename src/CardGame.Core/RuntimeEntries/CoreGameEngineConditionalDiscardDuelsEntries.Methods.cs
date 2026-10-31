#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConditionalDiscardDuelsEntries
{
    internal SkillProgramStepOutcome BeginConditionalDiscardDuel(ProgramSkillFrame supplied) => _resolve().BeginConditionalDiscardDuel(supplied);
    internal bool CanStartConditionalDiscardDuel(CharacterState owner) => _resolve().CanStartConditionalDiscardDuel(owner);
    internal IReadOnlyList<PromptChoice> ConditionalDuelChoices(ProgramSkillFrame f) => _resolve().ConditionalDuelChoices(f);
    internal bool ConditionalDuelParentMatches(ProgramSkillFrame f) => _resolve().ConditionalDuelParentMatches(f);
    internal bool ConditionalDuelSourceCurrent(ProgramSkillFrame f) => _resolve().ConditionalDuelSourceCurrent(f);
    internal void ContinueConditionalDuelPaymentChildren(ProgramSkillFrame f) => _resolve().ContinueConditionalDuelPaymentChildren(f);
    internal void FinishConditionalDiscardDuel(ProgramSkillFrame f, bool issued) => _resolve().FinishConditionalDiscardDuel(f, issued);
    internal void PublishConditionalDuelPayment(ProgramSkillFrame f) => _resolve().PublishConditionalDuelPayment(f);
    internal void ResolveConditionalDuelPayment(PromptChoice selected) => _resolve().ResolveConditionalDuelPayment(selected);
    internal bool ResumeConditionalDiscardDuel(long id) => _resolve().ResumeConditionalDiscardDuel(id);
    internal bool ReturnConditionalDuelMovement(ProgramSkillFrame f) => _resolve().ReturnConditionalDuelMovement(f);
    internal PromptChoice SelectAiConditionalDuel(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiConditionalDuel(decision, f);
}
