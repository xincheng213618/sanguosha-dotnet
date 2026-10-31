#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetRedirectEntries
{
    internal bool IsExactSoleTargetRedirectReturn(ProgramCardTriggerWindowFrame frame) => _resolve().IsExactSoleTargetRedirectReturn(frame);
    internal bool IsSoleTargetAfterProtectionRedirectFact(CardUseFrame use, SoleTargetAfterProtectionRedirectedEvent fact, IReadOnlyList<int> before) => _resolve().IsSoleTargetAfterProtectionRedirectFact(use, fact, before);
    internal void QueueSoleTargetRedirect(CardUseFrame use, SoleTargetProtectionReceipt receipt) => _resolve().QueueSoleTargetRedirect(use, receipt);
    internal void RecordSoleTargetAfterProtectionRedirect(ProgramSkillFrame redirector, long useId, int previousSeat, int newSeat) => _resolve().RecordSoleTargetAfterProtectionRedirect(redirector, useId, previousSeat, newSeat);
    internal IReadOnlyList<ProgramCardTriggerCandidate> SoleTargetRedirectCandidates(ProgramCardTriggerWindowFrame frame, CardUseFrame use, IReadOnlyList<int> targets) => _resolve().SoleTargetRedirectCandidates(frame, use, targets);
    internal bool TryBeginSoleTargetRedirect(ProgramCardTriggerWindowFrame frame) => _resolve().TryBeginSoleTargetRedirect(frame);
    internal bool TryReturnSoleTargetRedirect(ProgramCardTriggerWindowFrame frame) => _resolve().TryReturnSoleTargetRedirect(frame);
}
