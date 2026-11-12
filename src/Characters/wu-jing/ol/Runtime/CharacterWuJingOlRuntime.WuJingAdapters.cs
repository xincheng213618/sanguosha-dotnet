#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuJingOlRuntime
{
    internal bool WuJingRequestIdentity(ProgramSkillFrame f) => f.WuJing is { Operation: SkillProgramEffectOp.UseHejiAtCompletedSingleTarget } r && f.InstructionIndex == 1 && r.Source == WuJingSource(f) && r.GameplayHash == f.GameplayHash && r.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber;
    internal bool IsWuJingHejiRequest(ProgramSkillFrame f) => WuJingRequestIdentity(f) && f.WuJing!.Stage == WuJingStage.UseChoice;
    internal bool HasWuJingZeroSupply(ProgramSkillFrame f)
    {
        if (!IsWuJingHejiRequest(f) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.WuJing!.TargetSeat].IsAlive)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[f.WuJing.TargetSeat];
        return _characterLiuFengBoundaryOperations.HasBoundaryLiuFengProgramSlash(actor, [target.Seat]) || _characterGuoTuOlOperations.HasWeijieProgramSlash(actor, [target.Seat]) || (_characterLiuZhangOlOperations.HasZonghuProgramSlash(actor, [target.Seat]) || _characterHuanShuJunOlOperations.HasHuanpeiProgramSlash(actor, [target.Seat])) || _coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicRules(actor).Select(r => (r.Source, r.Rule.OutputKind)).Concat(_coreChainedStateBasicConversionsOperations.ChainedStateBasicRules(actor).Select(r => (r.Source, r.Rule.OutputKind))).Any(r => CoreNativeCardClassificationsRuntime.IsSlashCard(r.OutputKind) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, Suit.None) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, r.OutputKind, Suit.None) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, r.OutputKind) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, new(0, r.OutputKind, Suit.None, 0), r.Source, r.OutputKind, ignoreDistance: true, noEffectiveRank: true, physicalCardIds: []));
    }

    internal bool WuJingHasPaidAdapter(ProgramSkillFrame f) => WuJingRequestIdentity(f) && (_characterGuoTuOlOperations.HasWeijieSpecializedPayment(f) || (_characterLiuZhangOlOperations.HasZonghuSpecializedPayment(f) || _characterHuanShuJunOlOperations.HasHuanpeiProgramPayment(f)) || f.WuJing!.RoundOncePayment is not null || f.WuJing.DrawFundedPayment is not null || f.WuJing.ChainedPayment is not null || _resolutionStackOperations._resolutionStack.OfType<DrawFundedDistinctBasicFrame>().Any(p => p.Payment.ParentFrameId == f.Id && p.Payment.RequestFrameId == f.Id && p.Payment.Cursor == f.InstructionIndex && p.Payment.Source.OwnerSeat == f.OwnerSeat && p.Payment.TargetSeat == f.WuJing.TargetSeat && p.OriginalDecision.SkillPrompt?.SkillId == f.SkillId && _coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(p.Payment)) || _resolutionStackOperations._resolutionStack.OfType<ChainedStateBasicFrame>().Any(p => p.Payment.ParentFrameId == f.Id && p.Payment.RequestFrameId == f.Id && p.Payment.Cursor == f.InstructionIndex && p.Payment.Source.OwnerSeat == f.OwnerSeat && p.Payment.TargetSeat == f.WuJing.TargetSeat && p.OriginalDecision.SkillPrompt?.SkillId == f.SkillId && _coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(p.Payment)));
    internal void BeginWuJingRoundOnce(ProgramSkillFrame f, CardConversionSource source)
    {
        if (!IsWuJingHejiRequest(f) || !_coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessRules(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], CardKind.Slash).Any(entry => entry.Source == source) || !_coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramTargetLegal(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.WuJing!.TargetSeat, source, true))
            throw new InvalidOperationException("Heji cardless Slash lost its own available independent round quota.");
        var acceptance = _slashRequests.Accept(f, f.OwnerSeat, f.WuJing!.TargetSeat);
        f = _slashExecution.PrepareAcceptedUse(f, acceptance);
        var paid = _coreRoundOnceCardlessBasicOperations.ConsumeRoundOnceCardlessBasic(source, false);
        var r = f.WuJing!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuJing = r with { Stage = WuJingStage.IssuingUse, RoundOncePayment = paid, Selection = new(CardKind.Slash, false, false) { Conversions = [source] }, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
        var id = _coreNativeCardUseOperations.BeginCardUse(new(0, CardKind.Slash, Suit.None, 0), actor.Seat, [target.Seat], CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, CardKind.Slash), physicalCardIds: [], conversionSource: source, programSlashRequest: acceptance);
        _coreRoundOnceCardlessNativeUsesOperations.IssueRoundOnceCardlessNativeUse(id, source);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, target.Seat, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, target.Seat, IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, target);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal bool IsWuJingHejiZeroUse(CardUseFrame use) => use.WuJingHeji is { Origin.Materials.Count: 0 } && IsWuJingHejiUse(use);
}
