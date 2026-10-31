#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhongHuiAnniversaryGodRuntime
{
    internal static bool LishiOptionMatchesPhase(int option, ActualDiscardRecoveryPhaseKind kind) => option switch
    {
        1 => kind is ActualDiscardRecoveryPhaseKind.Preparation or ActualDiscardRecoveryPhaseKind.Ending,
        2 => kind == ActualDiscardRecoveryPhaseKind.Judgment,
        3 => kind == ActualDiscardRecoveryPhaseKind.Draw,
        4 => kind == ActualDiscardRecoveryPhaseKind.Play,
        5 => kind == ActualDiscardRecoveryPhaseKind.Discard,
        _ => false
    };
    internal IEnumerable<LishiPunishmentIssuedEvent> LishiActiveEffects(ActualDiscardRecoveryPhaseKey phase)
    {
        if (!HasLishiCapability || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ActualPhaseEndedEvent>().Any(e => e.Phase == phase))
            return[];
        var applied = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LishiPhaseAppliedEvent>().Where(e => e.Phase == phase).ToArray();
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LishiPunishmentIssuedEvent>().Where(e => e.TargetSeat == phase.ActorSeat && applied.Any(a => a.IssueFrameId == e.FrameId && a.TargetSeat == e.TargetSeat && a.Option == e.Option));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 39300)]
    internal void ObserveLishiPhase(IGameEvent payload)
    {
        if (!HasLishiCapability)
            return;
        if (payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { Action: { Type: CardActionType.Use } action } use && use.SourceSeat == declared.SourceSeat && use.CardId == declared.CardId && use.CardKind == declared.CardKind && action.ActorSeat == use.SourceSeat && action.EffectiveKind == use.CardKind)
            RecordLishiHandUse(action);
        if (payload is CardActionAcceptedEvent { Action.Type: CardActionType.Response } accepted)
            RecordLishiHandUse(accepted.Action);
        if (payload is ActualDiscardRecoveryPhaseStartedEvent started)
        {
            var phase = started.Phase;
            foreach (var issue in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LishiPunishmentIssuedEvent>().Where(e => e.TargetSeat == phase.ActorSeat && e.PhaseToken < phase.Token && LishiOptionMatchesPhase(e.Option, phase.Kind)).ToArray())
            {
                if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<LishiPhaseAppliedEvent>().Any(a => a.IssueFrameId == issue.FrameId && a.TargetSeat == issue.TargetSeat && a.Option == issue.Option && a.Phase.Kind == phase.Kind))
                    continue;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LishiPhaseAppliedEvent(issue.FrameId, issue.TargetSeat, issue.Option, phase));
                if (issue.Option == 1)
                    _coreProgramSkillChangesOperations.IssueSkillState(new(issue.FrameId, (int)phase.Kind,
                        new(issue.Source.SkillId, issue.Source.BindingId, issue.Source.OwnerSeat, issue.Source.SkillInstanceId)),
                        issue.TargetSeat, new(SkillStateAction.Suppress, SkillStateMatch.NonLocked,
                            [new(SkillStateBoundaryEvent.PhaseEnded, SkillStateSubject.Target, SkillStateBoundaryTiming.After, Phase: phase.Kind)],
                            new("inIssuedPhase", ProgramExpressionType.Bool, [new("selectedTarget", ProgramExpressionType.Participant, [], name: "selectedTarget")])));
            }
        }
    }

    internal static string LishiCategory(CardKind kind) => CardUseCategoryCatalog.Get(kind) switch
    {
        CardUseCategories.Basic => "basic",
        CardUseCategories.Equipment => "equipment",
        _ => "trick"
    };
    internal bool HasHappyLishiActualUseParticipant(int actor) => HasLishiCapability && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == actor && LishiActiveEffects(phase).Any(e => e.Option == 4 && e.Source.SkillId == ShenZhongHuiComposition.HappyLishi);
    internal bool LishiHandTypeSpent(int actor, CardKind kind, IReadOnlyList<int> ids, long? existing = null)
    {
        if (!HasLishiCapability || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != actor)
            return false;
        var hasHand = ids.Any(id => id > 0 && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(actor));
        if (!LishiActiveEffects(phase).Any(e => e.Option == 4 && (hasHand || e.Source.SkillId == ShenZhongHuiComposition.HappyLishi)))
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LishiHandTypeUsedEvent>().Any(e => e.Phase == phase && e.ActorSeat == actor && e.Category == LishiCategory(kind) && e.ActionId != existing);
    }

    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 42100)]
    internal bool HappyLishiAnyUseForbidden(ProgramCardUseQuery q) => q.ActionType == CardActionType.Use && HasHappyLishiActualUseParticipant(q.ActorSeat) && LishiHandTypeSpent(q.ActorSeat, q.Kind, [], q.ExistingUseFrameId is { } id ? _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.Action?.ActionId : null);
    [ProgramCardLegality(ProgramCardLegalityStage.SubmittedUseValidation, 39300)]
    internal void LishiSubmittedHandForbidden(ProgramSubmittedUseQuery q)
    {
        if (LishiHandTypeSpent(q.ActorSeat, q.Kind, q.CardIds))
            throw new InvalidOperationException("Lishi permits only one actual hand-card use of each type in the issued Play phase.");
    }

    [ProgramCardLegality(ProgramCardLegalityStage.AfterLegalActionKind, 39300)]
    internal bool LishiProjectedHandForbidden(ProgramLegalActionQuery q) => q.Action.CardId is { } id && q.EffectiveKind is { } kind && LishiHandTypeSpent(q.Actor.Seat, kind, [id]);
    [ProgramCardLegality(ProgramCardLegalityStage.MaterialUseBan, 39300)]
    internal bool LishiMaterialForbidden(ProgramCardMaterialQuery q) => !q.IsResponse && LishiHandTypeSpent(q.ActorSeat, q.Kind, q.CardIds.ToArray());
    internal bool IsLishiResponseUse(int actor, CardKind kind) => kind == CardKind.Nullification || kind == CardKind.Dodge && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.TargetSeat == actor && attack.EffectiveCardKind is { } incoming && CoreNativeCardClassificationsRuntime.IsSlashCard(incoming);
    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 39300)]
    internal bool LishiResponseHandForbidden(ProgramNativeResponseQuery q) => IsLishiResponseUse(q.Provider.Seat, q.Kind) && LishiHandTypeSpent(q.Provider.Seat, q.Kind, q.CardIds);
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.AcceptedConversionCost, 39301)]
    internal void RecordLishiHandUse(CardActionContext action)
    {
        if (!HasLishiCapability || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != action.ActorSeat || action.Type != CardActionType.Use && !(IsLishiResponseUse(action.ActorSeat, action.EffectiveKind) && _coreResponseUseCompletionOperations.IsCompletedResponseUse(action, action.EffectiveKind == CardKind.Nullification ? ProgramCardContinuation.NullificationResponse : ProgramCardContinuation.Dodge)) || !LishiActiveEffects(phase).Any(e => e.Option == 4 && (e.Source.SkillId == ShenZhongHuiComposition.HappyLishi || action.PhysicalCards.Any(c => c.From == CardLocation.Hand(action.ActorSeat)))) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LishiHandTypeUsedEvent>().Any(e => e.ActionId == action.ActionId))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LishiHandTypeUsedEvent(action.ActionId, action.ActorSeat, LishiCategory(action.EffectiveKind), phase));
    }

    internal CardLocation? LishiDiscardOrigin(int cardId, CardLocation from, CardMoveReason reason, long before)
    {
        if (CoreGameEngineProgramDiscardOriginsRuntime.IsOwnedDiscardSource(from))
            return CoreGameEngineProgramDiscardOriginsRuntime.IsProgramDiscardOriginReason(reason) ? from : null;
        if (from != CardLocation.Processing)
            return null;
        var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cardId && m.Sequence <= before);
        return entry is not null && entry.To == CardLocation.Processing && CoreGameEngineProgramDiscardOriginsRuntime.IsOwnedDiscardSource(entry.From) && CoreGameEngineProgramDiscardOriginsRuntime.IsProgramDiscardOriginReason(entry.Reason) && (reason == entry.Reason || reason.Value == entry.Reason.Value + "-finished" || reason.Value == entry.Reason.Value + "-discard") ? entry.From : null;
    }

    internal LishiPunishmentIssuedEvent? LishiDiscardClaim(int actor) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Discard } phase && phase.ActorSeat == actor ? LishiActiveEffects(phase).Where(e => e.Option == 5 && _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat].IsAlive).OrderBy(e => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, e.Source.OwnerSeat)).ThenBy(e => e.FrameId).FirstOrDefault() : null;
    internal CardLocation NormalizeLishiDiscardDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason)
    {
        if (to != CardLocation.DiscardPile || !HasLishiCapability || LishiDiscardOrigin(card.Id, from, reason, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)is not { OwnerSeat: { } owner } || LishiDiscardClaim(owner)is not { } claim)
            return to;
        return CardLocation.Hand(claim.Source.OwnerSeat);
    }

    internal void RecordLishiDiscardRedirection(CardMovementRecord move)
    {
        if (!HasLishiCapability || move.To.Zone != CardZoneKind.Hand || LishiDiscardOrigin(move.CardId, move.From, move.Reason, move.Sequence - 1)is not { OwnerSeat: { } target } original || LishiDiscardClaim(target)is not { } claim || move.To != CardLocation.Hand(claim.Source.OwnerSeat) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LishiDiscardRedirectedEvent>().Any(e => e.MovementSequence == move.Sequence))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LishiDiscardRedirectedEvent(move.Sequence, claim.FrameId, claim.Source.OwnerSeat, target, move.CardId, original, move.From, move.Reason, phase));
    }

    internal bool IsLishiPaidDiscardMovement(CardMovementRecord move, CardLocation expectedSource, CardMoveReason expectedReason) => HasLishiCapability && move.Reason == expectedReason && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LishiDiscardRedirectedEvent>().Any(e => e.MovementSequence == move.Sequence && e.CardId == move.CardId && e.OriginalFrom == expectedSource && e.ActualFrom == move.From && e.Reason == expectedReason && move.To == CardLocation.Hand(e.SourceSeat) && e.TargetSeat == e.Phase.ActorSeat && e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Discard && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(e.Phase) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LishiPunishmentIssuedEvent>().Any(i => i.FrameId == e.IssueFrameId && i.Source.OwnerSeat == e.SourceSeat && i.TargetSeat == e.TargetSeat && i.Option == 5));
    internal void SettleLishiDrawPhase(ProgramSkillFrame f)
    {
        var r = f.ShenZhongHui!;
        var phase = r.Phase!;
        var invoices = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeDrawInvocationRecordedEvent>().Where(e => e.OwnerSeat == f.OwnerSeat && e.Phase == phase).ToArray();
        var drawn = invoices.SelectMany(d => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > d.SequenceBefore && m.Sequence <= d.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason == d.Reason)).Select(m => m.CardId).Distinct().Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray();
        var allowed = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard).SelectMany(c => c.Cards).ToHashSet();
        var discard = drawn.Length > 0 && drawn.Select(c => CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c))).Distinct().Count() == 1 ? drawn.Where(c => _cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.Hand(f.OwnerSeat) && allowed.Contains(c.Id) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, c, CardLocation.Hand(f.OwnerSeat), OwnedCardMoveIntent.Discard)).ToArray() : [];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhongHui = r with { Before = before, After = before, Stage = ShenZhongHuiStage.Complete }, PendingMovementContinuation = discard.Length > 0 ? new(f.OwnerSeat, 0, null) : null });
        if (discard.Length > 0)
        {
            _corePhysicalMovementFacadesOperations.MoveCards(discard, CardLocation.Hand(f.OwnerSeat), CardLocation.DiscardPile, new("skill-program.anniversary:lishi.draw-discard"));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShenZhongHui = f.ShenZhongHui!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            foreach (var c in discard)
                if (_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == c.Id && m.Sequence > before)is { } paid)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LishiDrawDiscardedEvent(f.Id, c.Id, phase, paid.Sequence));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LishiDrawPhaseSettledEvent(f.Id, phase, drawn.Length, discard.Length));
    }

    internal string LishiJudgmentReason(ShenZhongHuiReceipt r) => CardCatalog.Get(r.Judgments[r.JudgmentIndex]).DisplayName;
    internal bool CanBeginLishiJudgment(ProgramSkillFrame f, int targetSeat, string reason, string bind, SkillProgramCardSetVisibility visibility) => f.SkillId is (ShenZhongHuiComposition.Curse or ShenZhongHuiComposition.HappyCurse) && f.TriggerId == "judge-debt" && f.ShenZhongHui is { Operation: SkillProgramEffectOp.ResolveLishiPhase, Stage: ShenZhongHuiStage.PhaseJudging, JudgmentId: > 0, Phase: { Kind: ActualDiscardRecoveryPhaseKind.Judgment } phase } r && r.Judgments.Count == 2 && r.JudgmentIndex < 2 && targetSeat == f.OwnerSeat && phase.ActorSeat == targetSeat && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase() == phase && reason == LishiJudgmentReason(r) && bind == "lishi-judgment" && visibility == SkillProgramCardSetVisibility.Public && LishiActiveEffects(phase).Any(e => e.Option == 2);
    internal bool IsLishiJudgment(JudgmentFrame j, ProgramSkillFrame f) => j.Continuation == JudgmentContinuationKind.ProgramSkill && j.ParentFrameId == f.Id && f.ShenZhongHui!.JudgmentId == j.Id && j.SourceSeat == f.OwnerSeat && CanBeginLishiJudgment(f, j.TargetSeat, j.Reason, j.ProgramResultBind ?? "", j.ProgramResultVisibility ?? SkillProgramCardSetVisibility.Private);
    internal void BeginLishiJudgment(ProgramSkillFrame f)
    {
        var r = f.ShenZhongHui!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhongHui = r = r with { Stage = ShenZhongHuiStage.PhaseJudging, JudgmentId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1, ResultId = null, ResultSuit = Suit.None, ResultRank = 0 } });
        _ = _coreNativeJudgmentOperations.BeginJudgment(null, f.OwnerSeat, LishiJudgmentReason(r), f.Id, null, JudgmentContinuationKind.ProgramSkill, sourceSeat: f.OwnerSeat, programResultBind: "lishi-judgment", programResultVisibility: SkillProgramCardSetVisibility.Public);
    }

    internal bool TryReturnLishiJudgment(JudgmentFrame j)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenZhongHui: { Stage: ShenZhongHuiStage.PhaseJudging } r } f || f.Id != j.ParentFrameId)
            return false;
        if (!IsLishiJudgment(j, f) || _resolutionStackOperations._resolutionStack.Any(x => x.Id == j.Id))
            throw new InvalidOperationException("Lishi's native judgment lost its exact original phase return.");
        var results = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<JudgmentResolvedEvent>().Where(e => e.ResolutionId == j.Id && e.ParentResolutionId == f.Id && e.TargetSeat == f.OwnerSeat && e.Reason == LishiJudgmentReason(r)).ToArray();
        if (results is not [var result])
            throw new InvalidOperationException("Lishi needs one real final judgment outcome.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenZhongHui = r with { Stage = ShenZhongHuiStage.PhaseResult, ResultId = result.CardId, ResultSuit = result.Suit ?? Suit.None, ResultRank = result.Rank ?? 0 } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LishiJudgmentReturnedEvent(f.Id, j.Id, r.JudgmentIndex, result.CardId, result.Suit ?? Suit.None, result.Rank ?? 0));
        ContinueShenZh(f.Id);
        return true;
    }

    internal void ContinueLishiJudgments(ProgramSkillFrame f)
    {
        var r = f.ShenZhongHui!;
        if (r.Stage == ShenZhongHuiStage.PhaseResult && r.ResultId is { } id && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing)
        {
            if (_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == id)is not { From.Zone: CardZoneKind.Judgment, To.Zone: CardZoneKind.Processing } paid || paid.Reason.Value != "skill-program.judgment.result")
                throw new InvalidOperationException("Lishi cannot discard a judgment entity without the native final-result transfer.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhongHui = r with { Stage = ShenZhongHuiStage.PhaseCleanup, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Processing, CardLocation.DiscardPile, new("skill-program.anniversary:lishi.judgment-finish"));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShenZhongHui = f.ShenZhongHui!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            ContinueShenZh(f.Id);
            return;
        }

        if (r.Stage is ShenZhongHuiStage.PhaseResult or ShenZhongHuiStage.PhaseCleanup)
        {
            var kind = r.Judgments[r.JudgmentIndex];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhongHui = r with { Stage = ShenZhongHuiStage.PhaseNext }, PendingMovementContinuation = null });
            if (r.ResultId is not null)
            {
                if (kind == CardKind.Indulgence && r.ResultSuit != Suit.Heart)
                    _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects |= GameEngine.DelayedTurnEffects.SkipPlayPhase;
                if (kind == CardKind.SupplyShortage && r.ResultSuit != Suit.Club)
                    _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects |= GameEngine.DelayedTurnEffects.SkipDrawPhase;
                if (kind == CardKind.Lightning && r.ResultSuit == Suit.Spade && r.ResultRank is >= 2 and <= 9)
                {
                    _coreSkillProgramsOperations.BeginProgramSkillDamage(f, f.OwnerSeat, 3, nature: DamageNature.Thunder, sourceLess: true);
                    return;
                }
            }
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.ShenZhongHui!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || r.JudgmentIndex + 1 >= r.Judgments.Count)
        {
            FinishShenZh(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhongHui = r with { JudgmentIndex = r.JudgmentIndex + 1 } });
        BeginLishiJudgment(f);
    }
}
