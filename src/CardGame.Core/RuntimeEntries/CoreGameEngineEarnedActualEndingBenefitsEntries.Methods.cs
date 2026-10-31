#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEarnedActualEndingBenefitsEntries
{
    internal void AppendEarnedActualEndingItems(List<TurnEndingBoundaryItem> items) => _resolve().AppendEarnedActualEndingItems(items);
    internal void ConsumeEarnedActualEnding(EarnedActualEndingBenefit benefit, bool applied) => _resolve().ConsumeEarnedActualEnding(benefit, applied);
    internal void ConsumeEarnedActualEndingAtBegin(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ConsumeEarnedActualEndingAtBegin(candidate, context);
    internal void ConsumeSkippedEarnedActualEnding(TurnEndingBoundaryFrame frame) => _resolve().ConsumeSkippedEarnedActualEnding(frame);
    internal bool EarnedActualEndingIssued(EarnedActualEndingBenefit benefit) => _resolve().EarnedActualEndingIssued(benefit);
    internal bool EarnedActualEndingWasConsumed(long id) => _resolve().EarnedActualEndingWasConsumed(id);
    internal void IssueEarnedActualEnding(ProgramSkillFrame frame, string skillId, string bindingId) => _resolve().IssueEarnedActualEnding(frame, skillId, bindingId);
    internal bool MatchesEarnedActualEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, bool begun) => _resolve().MatchesEarnedActualEnding(candidate, context, begun);
    internal bool MatchesLostHpOwnedGiftEnding(ProgramSkillFrame frame) => _resolve().MatchesLostHpOwnedGiftEnding(frame);
}
