#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreDirectedTurnCardPoliciesRuntime
{
    internal void GrantProgramDirectedTurnCardPolicy(ProgramSkillFrame frame, ProgramParticipantReference actorReference, ProgramParticipantReference targetReference, IReadOnlyList<CardKind> cardKinds, DirectedTurnCardPolicyEffect effects)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        var actorSeat = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, actorReference);
        var targetSeat = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, targetReference);
        if (actorSeat == targetSeat || effects == DirectedTurnCardPolicyEffect.None)
            throw new InvalidOperationException("A directed turn policy requires distinct participants and effects.");
        var effectIndex = frame.InstructionIndex - 1;
        var source = global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame);
        var normalizedKinds = cardKinds.Distinct().Order().ToArray();
        var existing = _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicies.SingleOrDefault(item => item.ParentFrameId == frame.Id && item.EffectIndex == effectIndex);
        if (existing is not null)
        {
            if (existing.Source != source || existing.ActorSeat != actorSeat || existing.TargetSeat != targetSeat || !existing.CardKinds.SequenceEqual(normalizedKinds) || existing.Effects != effects)
                throw new InvalidOperationException("A directed-policy grant key changed its meaning.");
            return;
        }

        var policy = new DirectedTurnCardPolicy(++_coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicySequence, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, effectIndex, source, actorSeat, targetSeat, Array.AsReadOnly(normalizedKinds), effects);
        _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicies.Add(policy);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DirectedTurnCardPolicyGrantedEvent(policy));
    }

    internal bool HasDirectedTurnCardPolicy(int actorSeat, int targetSeat, CardKind cardKind, DirectedTurnCardPolicyEffect effect) => _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicies.Any(item => item.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && item.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && item.ActorSeat == actorSeat && item.TargetSeat == targetSeat && (item.CardKinds.Count == 0 || ProgramCardKindMatch.Contains(item.CardKinds, cardKind)) && item.Effects.HasFlag(effect) && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[item.Source.OwnerSeat], item.Source.SkillId, item.Source.SkillInstanceId));
    internal bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, CardKind cardKind, long? cardUseFrameId = null) => _coreProgramTargetRulesOperations.TargetRules.IsDirectedProhibited(new(actorSeat, targetSeat, cardKind, cardUseFrameId));
    [ProgramTargetRule(ProgramTargetRulePoint.DirectedProhibition, 90)]
    internal bool DirectedTurnTargetProhibited(ProgramDirectedTargetQuery query) => HasDirectedTurnCardPolicy(query.ActorSeat, query.TargetSeat, query.Kind, DirectedTurnCardPolicyEffect.ForbidTarget);
    internal void ExpireDirectedTurnCardPolicies(int turnNumber, int turnSeat) => _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicies.RemoveAll(item => item.TurnNumber == turnNumber && item.TurnSeat == turnSeat);
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 170)]
    internal bool DistanceCardUseExemption170HasDirectedTurnCardPolicy(ProgramCardDistanceQuery q) => HasDirectedTurnCardPolicy(q.Actor.Seat, q.Target.Seat, q.Kind, DirectedTurnCardPolicyEffect.IgnoreDistance);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 100)]
    internal bool DirectedTurnSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => HasDirectedTurnCardPolicy(query.Actor.Seat, query.Target.Seat, query.Kind, DirectedTurnCardPolicyEffect.BypassSlashLimit);
}
