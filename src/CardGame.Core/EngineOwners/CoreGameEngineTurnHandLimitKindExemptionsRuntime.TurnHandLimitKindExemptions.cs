#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTurnHandLimitKindExemptionsRuntime
{
    void ITurnHandLimitKindExemptionProgramHost.GrantTurnHandLimitCardKindExemption(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds) => GrantProgramTurnHandLimitCardKindExemption(frame, cardKinds);
    internal void GrantProgramTurnHandLimitCardKindExemption(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds, SkillProgramDamageModifierExpiration expiration = SkillProgramDamageModifierExpiration.CurrentTurnEnd)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            return;
        if (_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber <= 0 || !Enum.IsDefined(expiration) ||
            cardKinds.Count == 0 || cardKinds.Any(kind => !Enum.IsDefined(kind)) || cardKinds.Distinct().Count() != cardKinds.Count)
            throw new InvalidOperationException("A hand-limit kind exemption requires an actual turn and distinct known card kinds.");
        var grant = _handLimitExemptions.Grant(frame, NativeHandLimitExemptionRelations.CardKinds, frame.OwnerSeat,
            new(new(CardKinds: cardKinds.Order().ToArray(), Face: HandLimitCardFace.LegacyHandIdentity),
                Expiry: expiration == SkillProgramDamageModifierExpiration.NextOwnerTurnStart
                    ? HandLimitExemptionExpiry.Permanent : HandLimitExemptionExpiry.FrozenTurnCoordinates,
                Turn: new(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber,
                    _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat), NativeExpiration: expiration));
        var policy = ToLegacyKindPolicy(grant);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnHandLimitCardKindExemptionGrantedEvent(policy));
    }

    internal IReadOnlyList<TurnHandLimitCardKindExemption>? GetTurnHandLimitCardKindExemptionsSnapshot(int ownerSeat)
    {
        var policies = _handLimitExemptions.ActiveRelations(NativeHandLimitExemptionRelations.CardKinds, ownerSeat)
            .Select(ToLegacyKindPolicy).ToArray();
        return policies.Length == 0 ? null : Array.AsReadOnly(policies);
    }

    private static TurnHandLimitCardKindExemption ToLegacyKindPolicy(HandLimitExemptionGrant grant)
    {
        var issue = grant.Issue;
        var source = issue.Source;
        return new(grant.Sequence, grant.IssuedTurn.TurnNumber, grant.IssuedTurn.OwnerSeat,
            issue.IssuerFrameId, issue.InstructionIndex,
            new(source.SkillId, source.BindingId, source.OwnerSeat, source.SkillInstanceId),
            Array.AsReadOnly(issue.Spec.Match.CardKinds.ToArray()))
        {
            Expiration = issue.Spec.NativeExpiration ?? throw new InvalidOperationException("A legacy kind projection requires its native expiration.")
        };
    }
}
