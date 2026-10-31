#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDelegatedJudgmentChoicesEntries
{
    internal void BeginDelegatedJudgmentChoice(JudgmentFrame j, JudgmentTriggerCandidate candidate, SkillProgram program, SkillProgramTrigger trigger, IReadOnlyList<Card> cards) => _resolve().BeginDelegatedJudgmentChoice(j, candidate, program, trigger, cards);
    internal PromptChoice DelegatedJudgmentChoice(JudgmentFrame j, string action, string label, int? card = default) => _resolve().DelegatedJudgmentChoice(j, action, label, card);
    internal IReadOnlyList<PromptChoice> DelegatedJudgmentChoices(JudgmentFrame j) => _resolve().DelegatedJudgmentChoices(j);
    internal IEnumerable<CardSnapshot> DelegatedJudgmentPrivateCards(int viewer) => _resolve().DelegatedJudgmentPrivateCards(viewer);
    internal bool IsDelegatedJudgment(SkillProgramTrigger trigger) => _resolve().IsDelegatedJudgment(trigger);
    internal bool IsDelegatedJudgmentPromptValid(JudgmentFrame j, PendingDecision? p) => _resolve().IsDelegatedJudgmentPromptValid(j, p);
    internal void PublishDelegatedJudgmentChoice(JudgmentFrame j) => _resolve().PublishDelegatedJudgmentChoice(j);
    internal bool ResolveAiDelegatedJudgmentChoice() => _resolve().ResolveAiDelegatedJudgmentChoice();
    internal bool TryResolveDelegatedJudgmentChoice(PromptChoice selected) => _resolve().TryResolveDelegatedJudgmentChoice(selected);
    internal bool ValidDelegatedJudgmentDraft(JudgmentFrame j, DelegatedJudgmentDraft d) => _resolve().ValidDelegatedJudgmentDraft(j, d);
}
