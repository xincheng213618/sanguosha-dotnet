#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreDirectedTurnCardPoliciesEntries
{
    internal bool DirectedTurnSlashQuotaAfterLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().DirectedTurnSlashQuotaAfterLimit(query);
    internal bool DirectedTurnTargetProhibited(global::CardGame.Core.ProgramDirectedTargetQuery query) => _resolve().DirectedTurnTargetProhibited(query);
    internal bool DistanceCardUseExemption170HasDirectedTurnCardPolicy(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption170HasDirectedTurnCardPolicy(q);
    internal void ExpireDirectedTurnCardPolicies(int turnNumber, int turnSeat) => _resolve().ExpireDirectedTurnCardPolicies(turnNumber, turnSeat);
    internal void GrantProgramDirectedTurnCardPolicy(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference actorReference, global::CardGame.Core.ProgramParticipantReference targetReference, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> cardKinds, global::CardGame.Core.DirectedTurnCardPolicyEffect effects) => _resolve().GrantProgramDirectedTurnCardPolicy(frame, actorReference, targetReference, cardKinds, effects);
    internal bool HasDirectedTurnCardPolicy(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.DirectedTurnCardPolicyEffect effect) => _resolve().HasDirectedTurnCardPolicy(actorSeat, targetSeat, cardKind, effect);
    internal bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default) => _resolve().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
}
