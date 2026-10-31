#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEarnedActualEndingBenefitsEntries
{
    internal void AppendEarnedActualEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => _resolve().AppendEarnedActualEndingItems(items);
    internal void ConsumeEarnedActualEnding(global::CardGame.Core.EarnedActualEndingBenefit benefit, bool applied) => _resolve().ConsumeEarnedActualEnding(benefit, applied);
    internal void ConsumeEarnedActualEndingAtBegin(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ConsumeEarnedActualEndingAtBegin(candidate, context);
    internal void ConsumeSkippedEarnedActualEnding(global::CardGame.Core.TurnEndingBoundaryFrame frame) => _resolve().ConsumeSkippedEarnedActualEnding(frame);
    internal bool EarnedActualEndingIssued(global::CardGame.Core.EarnedActualEndingBenefit benefit) => _resolve().EarnedActualEndingIssued(benefit);
    internal bool EarnedActualEndingWasConsumed(long id) => _resolve().EarnedActualEndingWasConsumed(id);
    internal void IssueEarnedActualEnding(global::CardGame.Core.ProgramSkillFrame frame, string skillId, string bindingId) => _resolve().IssueEarnedActualEnding(frame, skillId, bindingId);
    internal bool MatchesEarnedActualEnding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, bool begun) => _resolve().MatchesEarnedActualEnding(candidate, context, begun);
    internal bool MatchesLostHpOwnedGiftEnding(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().MatchesLostHpOwnedGiftEnding(frame);
}
