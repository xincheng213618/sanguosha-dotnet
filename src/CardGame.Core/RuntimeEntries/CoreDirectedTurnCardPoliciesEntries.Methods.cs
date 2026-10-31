#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreDirectedTurnCardPoliciesEntries
{
    internal bool DirectedTurnSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => _resolve().DirectedTurnSlashQuotaAfterLimit(query);
    internal bool DirectedTurnTargetProhibited(ProgramDirectedTargetQuery query) => _resolve().DirectedTurnTargetProhibited(query);
    internal bool DistanceCardUseExemption170HasDirectedTurnCardPolicy(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption170HasDirectedTurnCardPolicy(q);
    internal void ExpireDirectedTurnCardPolicies(int turnNumber, int turnSeat) => _resolve().ExpireDirectedTurnCardPolicies(turnNumber, turnSeat);
    internal void GrantProgramDirectedTurnCardPolicy(ProgramSkillFrame frame, ProgramParticipantReference actorReference, ProgramParticipantReference targetReference, IReadOnlyList<CardKind> cardKinds, DirectedTurnCardPolicyEffect effects) => _resolve().GrantProgramDirectedTurnCardPolicy(frame, actorReference, targetReference, cardKinds, effects);
    internal bool HasDirectedTurnCardPolicy(int actorSeat, int targetSeat, CardKind cardKind, DirectedTurnCardPolicyEffect effect) => _resolve().HasDirectedTurnCardPolicy(actorSeat, targetSeat, cardKind, effect);
    internal bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, CardKind cardKind, long? cardUseFrameId = default) => _resolve().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
}
