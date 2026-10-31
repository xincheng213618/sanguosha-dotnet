#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuYuShengOlRuntime
{
    internal const string LuYuShengDrawReason = "skill-program.lu-yu-sheng.draw", LuYuShengBottomDiscardReason = "skill-program.lu-yu-sheng.cangxin.discard", LuYuShengRunweiDiscardReason = "skill-program.lu-yu-sheng.runwei.discard";
    internal CardConversionSource LuYuShengSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome ILuYuShengHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginLuYuSheng(f, op);
    internal bool LuYuShengParent(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c)
    {
        if (c.OwnerSeat != candidate.OwnerSeat)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == c.ParentFrameId);
        if (parent is BeforeDamageProgramWindowFrame before)
            return c.Window == SkillProgramTriggerWindow.BeforeDamageApplied && before.TargetSeat == candidate.OwnerSeat && c.TargetSeat == before.TargetSeat && before.CandidateIndex >= 0 && before.CandidateIndex < before.Candidates.Count && before.Candidates[before.CandidateIndex].Candidate == candidate;
        return parent is ProgramLifecycleTriggerWindowFrame w && c.Window == w.Window && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && w.Candidates[w.CandidateIndex] == candidate && (c.Window == SkillProgramTriggerWindow.DrawPhaseStarting ? w.OwnerSeat == candidate.OwnerSeat && w.Continuation == ProgramLifecycleContinuation.CompleteDrawPhase : c.Window == SkillProgramTriggerWindow.DiscardPhaseStarting && w.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && w.Continuation == ProgramLifecycleContinuation.CompleteDiscardPhase) && c.SourceSeat == w.OwnerSeat;
    }

    internal bool LuYuShengFirstDamage(int seat) => !_corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).OfType<DamageAppliedEvent>().Any(e => e.TargetSeat == seat && e.Amount > 0) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<LuYuShengStartedEvent>().Any(e => e.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.Source.OwnerSeat == seat && e.Operation == SkillProgramEffectOp.DiscardBottomHeartsForDamageReduction);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26900)]
    internal bool CanOfferLuYuSheng(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects is not [var e] || !LuYuShengComposition.IsOperation(e.Op))
            return true;
        if (!LuYuShengParent(c, context))
            return false;
        if (e.Op == SkillProgramEffectOp.RevealBottomHeartsAndDraw)
            return _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Draw;
        if (e.Op == SkillProgramEffectOp.DiscardBottomHeartsForDamageReduction)
            return context.Amount > 0 && LuYuShengFirstDamage(c.OwnerSeat) && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { DamageAmountFinalized: true } attack && attack.TargetSeat == c.OwnerSeat;
        return _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Discard && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Discard } p && p.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) && _coreMatchParticipantsPlayersAccess._players[p.ActorSeat].IsAlive && _coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[p.ActorSeat]);
    }

    internal SkillProgramStepOutcome BeginLuYuSheng(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = f.WindowContext;
        if (f.LuYuSheng is not null || f.InstructionIndex != 1 || f.TriggerId is null || c is null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !CanOfferLuYuSheng(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("Lu Yu Sheng requires its exact unpaid actual native candidate.");
        var bottom = op == SkillProgramEffectOp.RunweiDrawOrDiscardForActualPhase ? [] : _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Take(3).Select(card => card.Id).ToArray();
        var attack = op == SkillProgramEffectOp.DiscardBottomHeartsForDamageReduction ? _coreProgramAttackAttemptOperations.CurrentDamageAttempt : null;
        var r = new LuYuShengReceipt(LuYuShengSource(f), f.GameplayHash, c.ParentFrameId, c.Window, op, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, op == SkillProgramEffectOp.RunweiDrawOrDiscardForActualPhase ? LuYuShengStage.RunweiChoice : attack is null ? LuYuShengStage.DrawChildren : LuYuShengStage.BottomChoice, op == SkillProgramEffectOp.RunweiDrawOrDiscardForActualPhase ? _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat : f.OwnerSeat, _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase(), attack?.ResolutionId, c.Amount, attack is null ? 0 : LuYuShengChainCursor(attack), attack?.IsChainPropagation ?? false, c.SourceSeat)
        {
            BottomCards = bottom
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LuYuSheng = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LuYuShengStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, op, r.Turn, r.TurnOwner, r.Payer, r.AttackId, r.OriginalDamage));
        for (var i = 0; i < bottom.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LuYuShengBottomRevealedEvent(f.Id, i, bottom[i], _coreAdvancedLifecycleOperations.GetAdvancedCard(bottom[i]).Suit));
        if (bottom.Length > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, Array.AsReadOnly(bottom.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray())));
        if (op == SkillProgramEffectOp.RevealBottomHeartsAndDraw)
        {
            IssueLuYuShengDraw(f, bottom.Count(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Suit == Suit.Heart));
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishLuYuSheng(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int LuYuShengChooser(ProgramSkillFrame f) => f.LuYuSheng!.Stage == LuYuShengStage.RunweiDiscardChoice ? f.LuYuSheng.Payer : f.OwnerSeat;
    internal IReadOnlyList<PromptChoice> LuYuShengChoices(ProgramSkillFrame f)
    {
        var r = f.LuYuSheng!;
        PromptChoice C(string option, string label, int? card = null) => new(new($"lu-yu-sheng.{(f.Id)}.{(r.Stage)}.{(option)}.{(card)}.{(r.Selected.Count)}"), label, card is { } id ? [id] : [], [], new Dictionary<string, string> { { "program-action", "lu-yu-sheng" }, { "frame-id", f.Id.ToString(CultureInfo.InvariantCulture) }, { "option", option } });
        if (r.Stage == LuYuShengStage.RunweiChoice)
            return[C("draw", "令其摸一张牌，此阶段手牌上限-1"), C("discard", "令其弃置一张牌，此阶段手牌上限+1")];
        if (r.Stage == LuYuShengStage.BottomChoice)
        {
            var choices = r.BottomCards.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DrawPile && !r.Selected.Contains(id)).Select(id => C("pick", $"弃置【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】", id)).ToList();
            choices.AddRange(r.Selected.Select(id => C("unpick", $"撤回【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】", id)));
            choices.Add(C("finish", $"完成（弃置{(r.Selected.Count)}张）"));
            return Array.AsReadOnly(choices.ToArray());
        }

        if (r.Stage != LuYuShengStage.RunweiDiscardChoice || !_coreMatchParticipantsPlayersAccess._players[r.Payer].IsAlive)
            return[];
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, r.Payer, r.Payer, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (zone, card) => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(r.Payer, card, new(zone, r.Payer), OwnedCardMoveIntent.Discard)).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "lu-yu-sheng", ["option"] = "pay" } }).ToArray());
    }

    internal void PublishLuYuSheng(ProgramSkillFrame f)
    {
        var choices = LuYuShengChoices(f);
        if (choices.Count == 0)
        {
            IssueLuYuShengRunwei(f, 1);
            FinishLuYuSheng(f);
            return;
        }

        var chooser = LuYuShengChooser(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, chooser, choices, f.LuYuSheng!.Stage == LuYuShengStage.BottomChoice ? "藏心：选择弃置公开牌堆底中的任意张牌。" : "润微：结算本次实际弃牌阶段。", f.LuYuSheng.Payer);
    }

    [ProgramAnswer("lu-yu-sheng")]
    internal void AnswerLuYuSheng(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LuYuSheng: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != LuYuShengChooser(f) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], LuYuShengChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Lu Yu Sheng lost its actual private chooser or original exposed entities.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var option = choice.Parameters["option"];
        if (r.Stage == LuYuShengStage.RunweiChoice)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LuYuShengRunweiChosenEvent(f.Id, r.Payer, option));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LuYuSheng = r with { Branch = option } });
            if (option == "draw")
            {
                IssueLuYuShengDraw(f, 1);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LuYuSheng = f.LuYuSheng!with { Stage = LuYuShengStage.RunweiDiscardChoice } });
            PublishLuYuSheng(f);
            return;
        }

        if (r.Stage == LuYuShengStage.BottomChoice)
        {
            if (option != "finish")
            {
                var id = choice.Cards.Single();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LuYuSheng = r with { Selected = option == "pick" ? r.Selected.Append(id).ToArray() : r.Selected.Where(x => x != id).ToArray() } });
                PublishLuYuSheng(f);
                return;
            }

            if (r.Selected.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.DrawPile))
                throw new InvalidOperationException("Cangxin cannot discard a replaced or moved bottom entity.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LuYuSheng = r with { Stage = LuYuShengStage.BottomDiscardChildren, Count = r.Selected.Count(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Suit == Suit.Heart), Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(r.Payer, 0, null) });
            if (r.Selected.Count > 0)
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Selected, CardLocation.DiscardPile, new(LuYuShengBottomDiscardReason));
            RecordLuYuShengMovement(f.Id);
            ContinueLuYuSheng(f.Id);
            return;
        }

        if (!Enum.TryParse<CardZoneKind>(choice.Parameters.GetValueOrDefault("source-zone"), out var zone) || zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !int.TryParse(choice.Parameters.GetValueOrDefault("slot-index"), out var slot))
            throw new InvalidOperationException("Runwei lost its real self-discard slot.");
        var from = new CardLocation(zone, r.Payer);
        var cards = _cardZonesOperations._cardZones.CardsAt(from);
        if (slot < 0 || slot >= cards.Count)
            throw new InvalidOperationException("Runwei payer slot is absent.");
        var card = cards[slot];
        var grain = from.Zone == CardZoneKind.Equipment && card.Kind == CardKind.WoodenOx ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[r.Payer]).Select(c => c.Id).ToArray() : [];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LuYuSheng = r with { Stage = LuYuShengStage.RunweiDiscardChildren, CardId = card.Id, From = from, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, WoodenOxGrainIds = grain }, PendingMovementContinuation = new(r.Payer, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Payer, [card.Id], new(LuYuShengRunweiDiscardReason));
        RecordLuYuShengMovement(f.Id);
        IssueLuYuShengRunwei(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), 1);
        ContinueLuYuSheng(f.Id);
    }

    [ProgramViewContributor(ProgramViewContribution.PublicCardsAfterResolution, 30)]
    internal IReadOnlyList<CardSnapshot> CurrentLuYuShengRevealedCards() => Array.AsReadOnly(_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.LuYuSheng is not null).SelectMany(f => f.LuYuSheng!.BottomCards).Distinct().Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray());
    internal void IssueLuYuShengDraw(ProgramSkillFrame f, int count)
    {
        var r = f.LuYuSheng!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LuYuSheng = r with { Stage = LuYuShengStage.DrawChildren, Count = count, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(r.Payer, 0, null) });
        if (count > 0 && _coreMatchParticipantsPlayersAccess._players[r.Payer].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, r.Payer, count, null, null, SkillProgramCardSetVisibility.Private, new(LuYuShengDrawReason));
        RecordLuYuShengMovement(f.Id);
        if (r.Operation == SkillProgramEffectOp.RunweiDrawOrDiscardForActualPhase)
            IssueLuYuShengRunwei(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), -1);
        ContinueLuYuSheng(f.Id);
    }

    internal void RecordLuYuShengMovement(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.LuYuSheng!;
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var shuffles = r.Stage == LuYuShengStage.DrawChildren ? _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == id && b.ParentBatchId is null && b.AwaitingProgramFrameId == id && b.TurnNumber == r.Turn && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.Movements.Count > 0 && b.Movements.All(m => m.Sequence > r.Before && m.Sequence <= after && m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)) && b.SourceCounts is [{ Location: var source, CountBefore: var discarded, CountAfter: 0 }] && source == CardLocation.DiscardPile && discarded == b.Movements.Count && b.DestinationCounts is [{ Location: var destination, CountBefore: 0, CountAfter: var replenished }] && destination == CardLocation.DrawPile && replenished == b.Movements.Count).Select(b => new LuYuShengReshuffle(b.Id, id, b.Movements[0].Sequence - 1, b.Movements[^1].Sequence) { Movements = b.Movements }).ToArray() : [];
        var count = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= after && (r.Stage != LuYuShengStage.DrawChildren || m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Payer) && m.Reason.Value == LuYuShengDrawReason));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { LuYuSheng = r with { After = after, ActualCount = count, Reshuffles = shuffles } });
        foreach (var s in shuffles)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LuYuShengReshuffleCapturedEvent(id, s.BatchId, s.Before, s.After, s.Movements.Count));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LuYuShengMovementIssuedEvent(id, r.Stage, r.Payer, r.Before, after, count));
    }

    internal bool DrainLuYuSheng(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueLuYuSheng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LuYuSheng: { } r } f || f.Id != id)
            return;
        if (r.Stage is LuYuShengStage.BottomChoice or LuYuShengStage.RunweiChoice or LuYuShengStage.RunweiDiscardChoice)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishLuYuSheng(f);
            return;
        }

        if (DrainLuYuSheng(f))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == LuYuShengStage.BottomDiscardChildren)
            ApplyLuYuShengReduction(f);
        FinishLuYuSheng(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void FinishLuYuSheng(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LuYuShengCompletedEvent(f.Id, true));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LuYuSheng = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26900)]
    internal bool ResumeLuYuSheng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LuYuSheng: not null } f || f.Id != id)
            return false;
        ContinueLuYuSheng(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 26900)]
    internal bool ReturnLuYuSheng(ProgramSkillFrame f)
    {
        if (f.LuYuSheng is null)
            return false;
        ContinueLuYuSheng(f.Id);
        return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26900)]
    internal bool ContinuePaidLuYuSheng(ProgramSkillFrame f)
    {
        if (f.LuYuSheng is null)
            return false;
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.RevealBottomHeartsAndDraw, SkillProgramEffectOp.DiscardBottomHeartsForDamageReduction, SkillProgramEffectOp.RunweiDrawOrDiscardForActualPhase)]
    internal PromptChoice AiLuYuSheng(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option") == "pick" && _coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()).Suit == Suit.Heart) ?? p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option") == "finish") ?? p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option") == "draw") ?? p.Choices.First();
}
