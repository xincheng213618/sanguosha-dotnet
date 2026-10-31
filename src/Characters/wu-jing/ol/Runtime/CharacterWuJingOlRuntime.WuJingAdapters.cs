#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuJingOlRuntime
{
    internal bool WuJingRequestIdentity(ProgramSkillFrame f) => f.WuJing is { Operation: SkillProgramEffectOp.UseHejiAtCompletedSingleTarget } r && f.InstructionIndex == 1 && r.Source == WuJingSource(f) && r.GameplayHash == f.GameplayHash && r.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuJingStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Operation == r.Operation && e.WindowId == r.WindowId && e.UseId == r.UseId && e.ActionId == r.ActionId && e.TargetSeat == r.TargetSeat) == 1;
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
    internal bool WuJingHejiAdapterActor(ProgramSkillFrame f, int actor, int? target, int cursor, long? parent, long? request) => WuJingRequestIdentity(f) && actor == f.OwnerSeat && target == f.WuJing!.TargetSeat && cursor == f.InstructionIndex && parent == f.Id && request == f.Id && f.WuJing.Stage is WuJingStage.UseChoice or WuJingStage.IssuingUse or WuJingStage.NativeUse && (_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || WuJingHasPaidAdapter(f));
    internal void PrepareWuJingDrawFundedSlash(ProgramSkillFrame f, DrawFundedDistinctBasicPayment p)
    {
        if (!IsWuJingHejiRequest(f) || !_coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(p) || p.Intent != DrawFundedDistinctBasicIntent.AssistedSlash || p.ParentFrameId != f.Id || p.RequestFrameId != f.Id || p.Cursor != f.InstructionIndex || p.Source.OwnerSeat != f.OwnerSeat || p.TargetSeat != f.WuJing!.TargetSeat || !CoreNativeCardClassificationsRuntime.IsSlashCard(p.EffectiveKind))
            throw new InvalidOperationException("Heji draw-funded Slash lost its real once-paid draw, original request or same target.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WuJing = f.WuJing with { Stage = WuJingStage.IssuingUse, DrawFundedPayment = p, Selection = new(p.EffectiveKind, false, false) { Conversions = [p.Source] }, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal void PrepareWuJingChainedSlash(ProgramSkillFrame f, ChainedStateBasicPayment p)
    {
        if (!IsWuJingHejiRequest(f) || !_coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(p) || p.Intent != ChainedStateBasicIntent.AssistedSlash || p.ParentFrameId != f.Id || p.RequestFrameId != f.Id || p.Cursor != f.InstructionIndex || p.Source.OwnerSeat != f.OwnerSeat || p.TargetSeat != f.WuJing!.TargetSeat || !CoreNativeCardClassificationsRuntime.IsSlashCard(p.EffectiveKind))
            throw new InvalidOperationException("Heji chained Slash lost its genuine paid state change, request or same target.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WuJing = f.WuJing with { Stage = WuJingStage.IssuingUse, ChainedPayment = p, Selection = new(p.EffectiveKind, false, false) { Conversions = [p.Source] }, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal bool FinishWuJingPaidAdapter(ProgramSkillFrame f)
    {
        if (!IsWuJingHejiRequest(f))
            return false;
        FinishWuJing(f, false);
        return true;
    }

    internal void PrepareWuJingKenshangNative(ProgramSkillFrame f, ProgramMultiCardViewAsSelection selection)
    {
        if (!IsWuJingHejiRequest(f))
            return;
        if (selection.Cards.Count < 2 || _corePlayCardHistoryOperations.ViewAsRule(selection.Source)?.KenshangUse != true || !WuJingCanUse(f.OwnerSeat, f.WuJing!.TargetSeat, selection.Cards[0], CardKind.Slash, selection.Source, selection.Cards.Select(c => c.Id).ToArray()) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<KenshangRequestFinishedEvent>().Count(e => e.OwnerFrameId == f.Id && e.ActorSeat == f.OwnerSeat && e.Accepted && e.MaterialCount == selection.Cards.Count) != 1)
            throw new InvalidOperationException("Heji Kenshang must finish its own actual private HE material request.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WuJing = f.WuJing with { Stage = WuJingStage.IssuingUse, Selection = new(CardKind.Slash, false, false) { CardIds = selection.Cards.Select(c => c.Id).ToArray(), Conversions = [selection.Source] }, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal bool WuJingZeroPaymentProof(ProgramSkillFrame f, WuJingReceipt r, CardUseFrame? use = null)
    {
        if (_characterGuoTuOlOperations.HasWeijieSpecializedPayment(f, use) || (_characterLiuZhangOlOperations.HasZonghuSpecializedPayment(f, use) || _characterHuanShuJunOlOperations.HasHuanpeiProgramPayment(f, use)))
            return true;
        if (r.Selection is not { CardIds.Count: 0, Conversions: [var source] } || source.OwnerSeat != f.OwnerSeat || new[]
        {
            r.RoundOncePayment is not null,
            r.DrawFundedPayment is not null,
            r.ChainedPayment is not null
        }.Count(v => v) != 1)
            return false;
        if (r.RoundOncePayment is { } quota)
            return quota.Source == source && quota.ActualTurnNumber == r.ActualTurn && quota.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && quota.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<RoundOnceCardlessBasicQuotaPaidEvent>().Count(e => e.Receipt == quota) == 1 && (use is null || _coreRoundOnceCardlessNativeUsesOperations.IsRoundOnceCardlessBasicUse(use.Id));
        if (r.DrawFundedPayment is { } draw)
            return draw.Source == source && draw.ParentFrameId == f.Id && draw.RequestFrameId == f.Id && draw.TargetSeat == r.TargetSeat && draw.Cursor == f.InstructionIndex && _coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(draw) && (use is null || use.DrawFundedDistinctBasicUse?.Payment == draw);
        return r.ChainedPayment is { } chain && chain.Source == source && chain.ParentFrameId == f.Id && chain.RequestFrameId == f.Id && chain.TargetSeat == r.TargetSeat && chain.Cursor == f.InstructionIndex && _coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(chain) && (use is null || use.ChainedStateBasicUse?.Payment == chain);
    }

    internal void BeginWuJingRoundOnce(ProgramSkillFrame f, CardConversionSource source)
    {
        if (!IsWuJingHejiRequest(f) || !_coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessRules(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], CardKind.Slash).Any(entry => entry.Source == source) || !_coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramTargetLegal(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.WuJing!.TargetSeat, source, true))
            throw new InvalidOperationException("Heji cardless Slash lost its own available independent round quota.");
        var paid = _coreRoundOnceCardlessBasicOperations.ConsumeRoundOnceCardlessBasic(source, false);
        var r = f.WuJing!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuJing = r with { Stage = WuJingStage.IssuingUse, RoundOncePayment = paid, Selection = new(CardKind.Slash, false, false) { Conversions = [source] }, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
        var id = _coreNativeCardUseOperations.BeginCardUse(new(0, CardKind.Slash, Suit.None, 0), actor.Seat, [target.Seat], CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, CardKind.Slash), physicalCardIds: [], conversionSource: source);
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
