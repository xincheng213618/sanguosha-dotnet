#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetRedirectEntries
{
    internal bool IsExactSoleTargetRedirectReturn(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => _resolve().IsExactSoleTargetRedirectReturn(frame);
    internal bool IsSoleTargetAfterProtectionRedirectFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SoleTargetAfterProtectionRedirectedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before) => _resolve().IsSoleTargetAfterProtectionRedirectFact(use, fact, before);
    internal void QueueSoleTargetRedirect(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SoleTargetProtectionReceipt receipt) => _resolve().QueueSoleTargetRedirect(use, receipt);
    internal void RecordSoleTargetAfterProtectionRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int previousSeat, int newSeat) => _resolve().RecordSoleTargetAfterProtectionRedirect(redirector, useId, previousSeat, newSeat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> SoleTargetRedirectCandidates(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().SoleTargetRedirectCandidates(frame, use, targets);
    internal bool TryBeginSoleTargetRedirect(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => _resolve().TryBeginSoleTargetRedirect(frame);
    internal bool TryReturnSoleTargetRedirect(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => _resolve().TryReturnSoleTargetRedirect(frame);
}
