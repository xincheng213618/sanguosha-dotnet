#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreDirectedTurnCardPoliciesEntries
{
    internal bool DirectedTurnSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => _resolve().DirectedTurnSlashQuotaAfterLimit(query);
    internal bool DirectedTurnTargetProhibited(ProgramDirectedTargetQuery query) => _resolve().DirectedTurnTargetProhibited(query);
    internal bool DistanceCardUseExemption170HasDirectedTurnCardPolicy(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption170HasDirectedTurnCardPolicy(q);
    internal void GrantProgramDirectedTurnCardPolicy(ProgramSkillFrame frame, ProgramParticipantReference actorReference, ProgramParticipantReference targetReference, IReadOnlyList<CardKind> cardKinds, DirectedTurnCardPolicyEffect effects, DirectedCardPolicyOptions? options = null) => _resolve().GrantProgramDirectedTurnCardPolicy(frame, actorReference, targetReference, cardKinds, effects, options);
    internal void GrantDirectedCardPolicy(ProgramSkillFrame frame, int actorSeat, int targetSeat, IReadOnlyList<CardKind> cardKinds, DirectedTurnCardPolicyEffect effects, DirectedCardPolicyOptions? options = null, int issuanceSlot = 0) => _resolve().GrantDirectedCardPolicy(frame, actorSeat, targetSeat, cardKinds, effects, options, issuanceSlot);
    internal bool HasDirectedTurnCardPolicy(int actorSeat, int targetSeat, CardKind cardKind, DirectedTurnCardPolicyEffect effect, long? cardUseFrameId = null) => _resolve().HasDirectedTurnCardPolicy(actorSeat, targetSeat, cardKind, effect, cardUseFrameId);
    internal IReadOnlyList<DirectedTurnCardPolicy> ActiveDirectedPolicies() => _resolve().ActiveDirectedPolicies();
    internal IEnumerable<DirectedTurnCardPolicy> DirectedPoliciesForUse(int actor, long? useId = null) => _resolve().DirectedPoliciesForUse(actor, useId);
    internal IReadOnlyList<DirectedTurnCardPolicy> CaptureDirectedPolicies(int actor) => _resolve().CaptureDirectedPolicies(actor);
    internal DeclaredCardPayment CaptureDirectedDeclarationPayment(DeclaredCardPayment paid, CardDeclarationParentReturn parent) => _resolve().CaptureDirectedDeclarationPayment(paid, parent);
    internal void IssueDirectedUse(CardUseFrame use) => _resolve().IssueDirectedUse(use);
    internal IReadOnlyList<DirectedTurnCardPolicy>? GetDirectedPolicySnapshot(int actor, long? useId = null) => _resolve().GetDirectedPolicySnapshot(actor, useId);
    internal bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, CardKind cardKind, long? cardUseFrameId = default) => _resolve().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
}
