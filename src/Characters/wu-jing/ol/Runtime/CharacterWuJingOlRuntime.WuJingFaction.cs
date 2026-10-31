#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuJingOlRuntime
{
    internal const string WuJingFactionBind = "heji-native-faction";
    internal bool WuJingFactionTarget(ProgramSkillFrame f) => f.WuJing is { TargetSeat: var target } && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && target != f.OwnerSeat && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(f.OwnerSeat, SkillProgramCardTargetRestriction.SelfOnly) && CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(kind => !_coreCardUseModulesOperations.IsCardUseForbidden(f.OwnerSeat, kind, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(f.OwnerSeat, target, kind) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[target]));
    internal void BeginWuJingFaction(ProgramSkillFrame f)
    {
        var r = f.WuJing!;
        var policy = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], CardKind.Slash);
        if (r.Stage != WuJingStage.UseChoice || policy is null || _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null || !WuJingFactionTarget(f))
            throw new InvalidOperationException("Heji faction request lost its exact same-target Slash need.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuJing = r with { Stage = WuJingStage.FactionRequest, FactionPolicy = policy } });
        var seats = _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(f.OwnerSeat, policy.FactionId);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = _kernelCoreMainOperations.CreateFactionCardRequestHandle(f.Id, FactionCardRequestPurpose.AssistedProgramUse, f.OwnerSeat, seats, policy.FactionId, targetSeat: r.TargetSeat, programSkillFrameId: f.Id, policySource: policy, assistedResultBind: WuJingFactionBind);
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashRequestedEvent(f.Id, f.OwnerSeat, seats, policy.SkillId, IsActiveUse: true, r.TargetSeat));
        _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
    }

    internal bool ValidateWuJingFactionParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame f)
    {
        if (f.WuJing is not { FactionPolicy: { } policy } r)
            return false;
        if (r.Operation != SkillProgramEffectOp.UseHejiAtCompletedSingleTarget || r.Stage is not (WuJingStage.FactionRequest or WuJingStage.IssuingUse or WuJingStage.NativeUse) || r.Source != WuJingSource(f) || r.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || pending.ProgramSkillFrameId != f.Id || pending.ResolutionId != f.Id || pending.OwnerSeat != f.OwnerSeat || pending.TargetSeat != r.TargetSeat || pending.PolicySource != policy || pending.AssistedResultBind != WuJingFactionBind || !pending.IsAssistedProgramUse || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<FactionSlashRequestedEvent>().Count(e => e.ResolutionId == f.Id && e.OwnerSeat == f.OwnerSeat && e.TargetSeat == r.TargetSeat && e.SkillId == policy.SkillId) != 1)
            throw new InvalidOperationException("Heji faction Slash lost its exact original policy, target or requested return.");
        return true;
    }

    internal bool HasWuJingFactionDistance(int actor, int target) => _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { IsAssistedProgramUse: true, ProgramSkillFrameId: { } id } pending && pending.OwnerSeat == actor && pending.TargetSeat == target && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == id)is { WuJing: not null } owner && ValidateWuJingFactionParent(pending, owner) && WuJingFactionTarget(owner);
    internal bool PrepareWuJingFactionNative(ProgramSkillFrame f, NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> cards, CardKind kind)
    {
        if (!ValidateWuJingFactionParent(pending, f))
            return false;
        if (f.WuJing!.Stage != WuJingStage.FactionRequest || cards.Count == 0 || provider.Seat == f.OwnerSeat || !pending.CandidateSeats.Contains(provider.Seat) || cards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.Id).OwnerSeat != provider.Seat))
            throw new InvalidOperationException("Heji faction Slash lost its real provider-owned materials.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WuJing = f.WuJing with { Stage = WuJingStage.IssuingUse, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Selection = new(kind, cards.Count == 2 && _coreAdvancedWeaponsOperations.HasWeaponAbility(provider, CardKind.ZhangbaSerpentSpear), false) { CardIds = cards.Select(c => c.Id).ToArray() } } });
        return true;
    }

    internal bool FinishWuJingFactionDeclined(long id, NativeOwners.FactionCardRequestHandle pending)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuJing: { FactionPolicy: not null } } f || f.Id != id)
            return false;
        ValidateWuJingFactionParent(pending, f);
        FinishWuJing(f, false);
        return true;
    }
}
