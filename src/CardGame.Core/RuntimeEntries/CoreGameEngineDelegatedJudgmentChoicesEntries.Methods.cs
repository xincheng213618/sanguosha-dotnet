#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDelegatedJudgmentChoicesEntries
{
    internal void BeginDelegatedJudgmentChoice(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.JudgmentTriggerCandidate candidate, global::CardGame.Core.SkillProgram program, global::CardGame.Core.SkillProgramTrigger trigger, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().BeginDelegatedJudgmentChoice(j, candidate, program, trigger, cards);
    internal global::CardGame.Core.PromptChoice DelegatedJudgmentChoice(global::CardGame.Core.JudgmentFrame j, string action, string label, int? card = default) => _resolve().DelegatedJudgmentChoice(j, action, label, card);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DelegatedJudgmentChoices(global::CardGame.Core.JudgmentFrame j) => _resolve().DelegatedJudgmentChoices(j);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> DelegatedJudgmentPrivateCards(int viewer) => _resolve().DelegatedJudgmentPrivateCards(viewer);
    internal bool IsDelegatedJudgment(global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().IsDelegatedJudgment(trigger);
    internal bool IsDelegatedJudgmentPromptValid(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.PendingDecision? p) => _resolve().IsDelegatedJudgmentPromptValid(j, p);
    internal void PublishDelegatedJudgmentChoice(global::CardGame.Core.JudgmentFrame j) => _resolve().PublishDelegatedJudgmentChoice(j);
    internal bool ResolveAiDelegatedJudgmentChoice() => _resolve().ResolveAiDelegatedJudgmentChoice();
    internal bool TryResolveDelegatedJudgmentChoice(global::CardGame.Core.PromptChoice selected) => _resolve().TryResolveDelegatedJudgmentChoice(selected);
    internal bool ValidDelegatedJudgmentDraft(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.DelegatedJudgmentDraft d) => _resolve().ValidDelegatedJudgmentDraft(j, d);
}
