#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaManOlRuntime
{
    internal static CardConversionSource HuaManSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_hua_man_ol_HuaManObservers.HuaManSource(f);
    internal bool HuaManCurrent(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(f) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 560)]
    internal bool CanOfferHuaMan(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (!t.Effects.Any(e => HuaManComposition.Own(e.Op)))
            return true;
        return t.Effects[0].Op switch
        {
            SkillProgramEffectOp.DrawAndCountMansiActualBarbarianDamage => w.DamageFrameId is { } damage && _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == damage)is { } d && d.Amount > 0 && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(f => f.ParentFrameId == damage)is { } window && _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(window).EffectiveCardKind == CardKind.BarbarianAssault,
            SkillProgramEffectOp.AwakenZhanyuanAndOfferXili => c.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && ((_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].HuaManProgress?.MansiCards ?? 0) > 7 || _characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(c.OwnerSeat, c.SkillId, c.BindingId)) && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].HuaManProgress?.Awakened != true && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Preparation } phase && phase.ActorSeat == c.OwnerSeat,
            SkillProgramEffectOp.PaySouyingDirectionalSoleTarget => GetSouyingQualification(w) is { } q && q.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && (q.ActorSeat == c.OwnerSeat || q.TargetSeat == c.OwnerSeat) && _coreCardUseLifecycleOperations.LifecycleCardUse(q.NativeUseId)is { Action: { } a } use && a.ActorSeat == q.ActorSeat && a.ProviderSeat == q.ProviderSeat && a.EffectiveDesignatedTargetSeats.SequenceEqual([q.TargetSeat]) && (q.TargetSeat == c.OwnerSeat || a.PhysicalCards.Any(p => _cardZonesOperations._cardZones.GetLocation(p.CardId) == CardLocation.Processing && !_nativeMaterialClaims.HasRelinquishedMaterial(use.Id, p.CardId))) && HasHuaManDiscard(c.OwnerSeat),
            SkillProgramEffectOp.PayXiliActualOtherOwnerDamage => w.SourceSeat is { } source && source != c.OwnerSeat && source == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && w.Amount > 0 && w.TargetSeat is { } target && HasRuntimeXili(source) && !HasRuntimeXili(target) && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false, IsDelayedJudgmentDamage: false } attack && attack.SourceSeat == source && attack.TargetSeat == target && HasHuaManDiscard(c.OwnerSeat),
            _ => false
        };
    }

    internal IEnumerable<Card> HuaManDiscardCards(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat])).Concat(_coreNativeDamageHandlesOperations.Value.UsesFormalWoodenOx && _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).Any(c => c.Kind == CardKind.WoodenOx) ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[seat]) : []).Where(c => !c.IsGeneralWeapon && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard));
    internal bool HuaManDiscardLocation(int seat, CardLocation at) => at.OwnerSeat == seat && (at.Zone is CardZoneKind.Hand or CardZoneKind.Equipment || at == CardLocation.WoodenOxGrain(seat) && _coreNativeDamageHandlesOperations.Value.UsesFormalWoodenOx && _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).Any(c => c.Kind == CardKind.WoodenOx));
    internal bool HasHuaManDiscard(int seat) => HuaManDiscardCards(seat).Any();

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 340)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 340)]
    internal bool CanContinueHuaMan(ProgramSkillFrame f)
    {
        if (f.HuaMan is not { Paid: true })
            return false;
        return true;
    }
    internal bool HuaManFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.HuaMan is not { } r || !CanContinueHuaMan(root))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (child is HpChangedTriggerWindowFrame hp)
        {
            if (r.Stage == HuaManStage.DiscardChildren && HuaManLionCost(root) && hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.HpBefore == r.NativeOwnerHpBefore && hp.Change.HpAfter == r.NativeOwnerHpAfter && hp.Change.Amount == r.NativeOwnerHpAfter - r.NativeOwnerHpBefore && hp.Change.Amount == 1)
                return true;
            return r.Stage is HuaManStage.MaximumChildren or HuaManStage.RecoveryChildren && hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.TargetSeat == root.OwnerSeat && (r.Stage == HuaManStage.MaximumChildren ? hp.Change.Kind == HpChangeKind.MaximumHp : hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount <= r.RecoveryRequested);
        }

        if (child is RecoveryReplacementFrame lion && r.Stage == HuaManStage.DiscardChildren && HuaManLionCost(root) && lion.ParentFrameId == root.Id && lion.Return.ResumeFrameId == root.Id && lion.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && lion.Attempt.Completion.CardId == r.DiscardedId && lion.Attempt.Completion.MoveReason?.Value == HuaManDiscardReason && lion.Attempt.SourceSeat == root.OwnerSeat && lion.Attempt.TargetSeat == root.OwnerSeat && lion.Attempt.Amount == 1)
            return true;
        return child is RecoveryReplacementFrame recovery && r.Stage == HuaManStage.RecoveryChildren && recovery.ParentFrameId == root.Id && recovery.Return.ResumeFrameId == root.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.TargetSeat == root.OwnerSeat && recovery.Attempt.SourceSeat == root.OwnerSeat && recovery.Attempt.Amount == r.RecoveryRequested && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == root.InstructionIndex;
    }

    internal ProgramSkillFrame? HuaManObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => this.HuaManFirstChild(r, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(n, d) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(n, d) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(n, d))));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 410)]
    internal bool TryAdvanceHuaManSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HuaManObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    internal bool HuaManLionCost(ProgramSkillFrame f) => f.HuaMan is { Stage: HuaManStage.DiscardChildren, DiscardedId: { } id } r && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.SilverLion && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == id && m.From == CardLocation.Equipment(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == HuaManDiscardReason) == 1;
}
