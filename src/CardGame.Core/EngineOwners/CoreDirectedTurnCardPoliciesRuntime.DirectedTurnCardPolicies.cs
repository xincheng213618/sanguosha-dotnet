#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreDirectedTurnCardPoliciesRuntime
{
    internal void GrantProgramDirectedTurnCardPolicy(ProgramSkillFrame frame, ProgramParticipantReference actorReference, ProgramParticipantReference targetReference, IReadOnlyList<CardKind> cardKinds, DirectedTurnCardPolicyEffect effects, DirectedCardPolicyOptions? options = null) =>
        GrantDirectedCardPolicy(frame, _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, actorReference),
            _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, targetReference), cardKinds, effects, options);

    internal void GrantDirectedCardPolicy(ProgramSkillFrame frame, int actorSeat, int targetSeat,
        IReadOnlyList<CardKind> cardKinds, DirectedTurnCardPolicyEffect effects, DirectedCardPolicyOptions? options = null)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        if (actorSeat < 0 || actorSeat >= _coreMatchParticipantsPlayersAccess._players.Count ||
            targetSeat < 0 || targetSeat >= _coreMatchParticipantsPlayersAccess._players.Count ||
            actorSeat == targetSeat || effects == DirectedTurnCardPolicyEffect.None)
            throw new InvalidOperationException("A directed turn policy requires distinct participants and effects.");
        if (effects.HasFlag(DirectedTurnCardPolicyEffect.BypassUseLimit) && cardKinds.Count > 0)
            throw new InvalidOperationException("An all-use quota allowance requires all card kinds.");
        options ??= new();
        var effectIndex = frame.InstructionIndex - 1;
        var source = CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame);
        var normalizedKinds = cardKinds.Distinct().Order().ToArray();
        var existing = _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicies.SingleOrDefault(item => item.ParentFrameId == frame.Id && item.EffectIndex == effectIndex);
        if (existing is not null)
        {
            if (existing.Source != source || existing.ActorSeat != actorSeat || existing.TargetSeat != targetSeat || !existing.CardKinds.SequenceEqual(normalizedKinds) || existing.Effects != effects ||
                existing.RequiresEffectiveSource != options.RequiresEffectiveSource || existing.CaptureOnUse != options.CaptureOnUse)
                throw new InvalidOperationException("A directed-policy grant key changed its meaning.");
            return;
        }

        var lifetime = _ruleLifetimes.CreateApplicationLifetime(frame.OwnerSeat, targetSeat, options.Until,
            new(frame.OwnerSeat, targetSeat, IssuedTurnNumber: _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber))
            ?? throw new InvalidOperationException("A directed policy requires a valid boundary lifetime.");
        var policy = new DirectedTurnCardPolicy(++_coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicySequence, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, effectIndex, source, actorSeat, targetSeat, Array.AsReadOnly(normalizedKinds), effects)
        { Until = lifetime.Until, RequiresEffectiveSource = options.RequiresEffectiveSource, CaptureOnUse = options.CaptureOnUse };
        _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicies.Add(policy);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DirectedTurnCardPolicyGrantedEvent(policy));
    }

    internal IReadOnlyList<DirectedTurnCardPolicy> ActiveDirectedPolicies() => Array.AsReadOnly(
        _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicies
            .Where(IsDirectedSourceQualified).ToArray());

    private bool IsDirectedSourceQualified(DirectedTurnCardPolicy item) => !item.RequiresEffectiveSource ||
        _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[item.Source.OwnerSeat],
            item.Source.SkillId, item.Source.SkillInstanceId);

    internal IReadOnlyList<DirectedTurnCardPolicy> CaptureDirectedPolicies(int actorSeat) => Array.AsReadOnly(
        _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicies
            .Where(item => item.CaptureOnUse && item.ActorSeat == actorSeat && IsDirectedSourceQualified(item)).ToArray());

    internal bool HasDirectedTurnCardPolicy(int actorSeat, int targetSeat, CardKind cardKind, DirectedTurnCardPolicyEffect effect, long? cardUseFrameId = null)
    {
        var flags = DirectedTurnCardPolicyEffect.None;
        foreach (var item in DirectedPoliciesForUse(actorSeat, cardUseFrameId))
            if (item.TargetSeat == targetSeat && (item.CardKinds.Count == 0 || ProgramCardKindMatch.Contains(item.CardKinds, cardKind)))
                flags |= item.Effects;
        return flags.HasFlag(effect) && (effect == DirectedTurnCardPolicyEffect.ForbidTarget ||
            !flags.HasFlag(DirectedTurnCardPolicyEffect.ForbidTarget));
    }

    internal IEnumerable<DirectedTurnCardPolicy> DirectedPoliciesForUse(int actorSeat, long? cardUseFrameId = null)
    {
        var accepted = GetDirectedPolicySnapshot(actorSeat, cardUseFrameId);
        if (accepted is not null)
            foreach (var item in accepted) yield return item;
        foreach (var item in _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicies)
            if (item.ActorSeat == actorSeat && (!item.CaptureOnUse || accepted is null) && IsDirectedSourceQualified(item))
                yield return item;
    }
    internal bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, CardKind cardKind, long? cardUseFrameId = null) => _coreProgramTargetRulesOperations.TargetRules.IsDirectedProhibited(new(actorSeat, targetSeat, cardKind, cardUseFrameId));
    [ProgramTargetRule(ProgramTargetRulePoint.DirectedProhibition, 90)]
    internal bool DirectedTurnTargetProhibited(ProgramDirectedTargetQuery query) => HasDirectedTurnCardPolicy(query.ActorSeat, query.TargetSeat, query.Kind, DirectedTurnCardPolicyEffect.ForbidTarget, query.ExistingUseFrameId);
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 170)]
    internal bool DistanceCardUseExemption170HasDirectedTurnCardPolicy(ProgramCardDistanceQuery q) => HasDirectedTurnCardPolicy(q.Actor.Seat, q.Target.Seat, q.Kind, DirectedTurnCardPolicyEffect.IgnoreDistance, q.CardUseFrameId);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 100)]
    internal bool DirectedTurnSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) =>
        HasDirectedTurnCardPolicy(query.Actor.Seat, query.Target.Seat, query.Kind, DirectedTurnCardPolicyEffect.BypassSlashLimit) ||
        HasDirectedTurnCardPolicy(query.Actor.Seat, query.Target.Seat, query.Kind, DirectedTurnCardPolicyEffect.BypassUseLimit);
    [ProgramTargetRule(ProgramTargetRulePoint.QuotaAllowance, 90)]
    internal bool DirectedUseQuotaAllowed(ProgramTargetQuotaQuery query) =>
        HasDirectedTurnCardPolicy(query.ActorSeat, query.TargetSeat, CardKind.Slash, DirectedTurnCardPolicyEffect.BypassUseLimit);
}
