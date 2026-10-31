#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenGongMouMobileRuntime
{
    internal bool ValidMouChenGongGift(ProgramSkillFrame f)
    {
        if (f.MouChenGongGift is not { } r || !MouChenGongGiftRoot(f) || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            return false;
        var gifts = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongGiftPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Gift is not { } gift)
            return r.Stage == MouChenGongGiftStage.GiftChoice && gifts.Length == 0;
        if (gift.From.OwnerSeat != f.OwnerSeat || gift.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || gift.Sequence <= 0 || gifts is not [var paid] || paid != new MouChenGongGiftPaidEvent(f.Id, r.RecipientSeat, gift.CardId, gift.PrintedKind, gift.From, gift.To, gift.Sequence) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == gift.Sequence && m.CardId == gift.CardId && m.CardKind == gift.PrintedKind && m.From == gift.From && m.To == gift.To && m.Reason.Value == MouChenGongGiftReason) != 1)
            return false;
        return r.Stage == MouChenGongGiftStage.GiftChildren;
    }

    internal bool ValidMouChenGongRelease(ProgramSkillFrame f)
    {
        if (f.MouChenGongRelease is not { } r || !MouChenGongReleaseRoot(f) || !Enum.IsDefined(r.Stage) || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            return false;
        var accepted = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongReleaseAcceptedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var damage = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongReleaseDamageIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var cleared = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongCeClearedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.TargetSeat is not { } target || r.FrozenX is not { } x)
            return r.Stage == MouChenGongReleaseStage.TargetChoice && r.TargetSeat is null && r.FrozenX is null && !r.Cleared && accepted.Length == 0 && damage.Length == 0 && cleared.Length == 0;
        return _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && target != f.OwnerSeat && x > 0 && accepted is [var accept] && accept == new MouChenGongReleaseAcceptedEvent(f.Id, target, x) && damage is [var issue] && issue == new MouChenGongReleaseDamageIssuedEvent(f.Id, f.OwnerSeat, target, x) && (r.Cleared ? r.Stage == MouChenGongReleaseStage.ClearChildren && cleared is [var clear] && clear.OwnerSeat == f.OwnerSeat && clear.FrozenX == x && clear.ClearedCount >= 0 : r.Stage == MouChenGongReleaseStage.Damage && cleared.Length == 0);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29600)]
    internal bool PermitAcceptedMouChenGong(ProgramSkillFrame f)
    {
        if (f.MouChenGongGift is { Gift: not null })
            return ValidMouChenGongGift(f);
        if (f.SkillId == MouChenGongComposition.Mingce && f.ActivationId == "gift-and-plan" && f.TriggerId is null && f.SelectedTargetSeats is [var recipient])
            return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongMingceStartedEvent>().Any(e => e.FrameId == f.Id && e.Source == MouChenGongSource(f) && e.GameplayHash == f.GameplayHash && e.RecipientSeat == recipient) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongGiftPaidEvent>().Any(e => e.FrameId == f.Id && e.RecipientSeat == recipient);
        return f.MouChenGongRelease is { FrozenX: > 0 } && ValidMouChenGongRelease(f);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29601)]
    internal bool PermitIssuedMouChenGong(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && PermitMouChenGongZhichiAfterLoss(f);
    internal void SealMouChenGongMovements(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongGift = f.MouChenGongGift is { } gift ? gift with { After = after } : null });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= after))
        {
            var signed = new MouChenGongMovementSignedEvent(id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongMovementSignedEvent>().Contains(signed))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(signed);
        }
    }

    internal bool MouChenGongMovementSigned(ProgramSkillFrame f, CardMovementRecord m) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongMovementSignedEvent>().Count(e => e == new MouChenGongMovementSignedEvent(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)) == 1;
    internal bool IsMouChenGongMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op == SkillProgramEffectOp.ResolveMouChenGongMingceGift && f.MouChenGongGift is { Stage: MouChenGongGiftStage.GiftChildren } && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidMouChenGongGift(f);
    internal bool DrainMouChenGongChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal bool MouChenGongFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouChenGongGift is { Gift: not null } r && ValidMouChenGongGift(f))
        {
            if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f))
                return true;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && r.Gift is { PrintedKind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Kind == HpChangeKind.Recovery;
            if (f.PendingMovementContinuation is { } pending && IsMouChenGongMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending) && child is CardsMovedTriggerWindowFrame moved)
                return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && MouChenGongMovementSigned(f, m));
        }
        else if (f.MouChenGongRelease is { Stage: MouChenGongReleaseStage.Damage, FrozenX: > 0, TargetSeat: not null } && ValidMouChenGongRelease(f) && f.AttackAttempt is { } a && f.AttackReturn is not null && a.SourceSeat == f.OwnerSeat && !a.SourceLess)
            return _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        else
            return false;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouChenGongObserverSuffix(int index) => _resolutionStackOperations._resolutionStack[index + 1] is DyingFrame original && index + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index + 1, original) || _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && this.MouChenGongFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? MouChenGongObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.MouChenGongFirstChild(f, child), (n) => this.MouChenGongObserverSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 740)]
    internal bool IsMouChenGongDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && MouChenGongObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 520)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 510)]
    internal bool IsMouChenGongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return index >= 0 && child > index + 1 && MouChenGongFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && MouChenGongObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29600)]
    internal bool ResumeMouChenGong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id)
            return false;
        if (f.MouChenGongGift is not null)
        {
            ContinueMouChenGongGift(id);
            return true;
        }

        if (f.MouChenGongRelease is not null)
        {
            ContinueMouChenGongRelease(id);
            return true;
        }

        return false;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29600)]
    internal bool ReturnMouChenGongMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouChenGong(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29600)]
    internal bool AdvanceMouChenGongChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouChenGongObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
