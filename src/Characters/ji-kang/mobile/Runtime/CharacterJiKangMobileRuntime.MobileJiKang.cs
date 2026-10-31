#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiKangMobileRuntime
{
    internal static CardConversionSource MobileJiKangSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal const string MobileJiKangDiscard = "skill-program.mobile-ji-kang.discard";
    SkillProgramStepOutcome IMobileJiKangHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginMobileJiKang(f, op);
    internal IReadOnlyList<PromptChoice> MobileJiKangCosts(long frame, int owner) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame, owner, owner, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Where(c => c.Cards.Count == 1).ToArray();
    internal bool MobileJiKangHasCost(CharacterState owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).Any(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner.Seat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner.Seat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard));
    internal bool MobileJiKangTargetAllowed(CardConversionSource source, int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && seat != source.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && (source.SkillId != "mobile:canyun" || !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileJiKangTargetIssuedEvent>().Any(e => e.Source == source && e.TargetSeat == seat));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 80900, SkillProgramEffectOp.ResolveMobileJiKangQingxian)]
    internal bool CanActivateMobileJiKang(ProgramActivationLegalityContext gate) => gate.Owner is { IsAlive: true, HasHpValue: true, Hp: > 0 } && MobileJiKangHasCost(gate.Owner) && _coreNationalSkillsOperations.GetSkillBindingShard(gate.Owner).ActiveGrants.Where(g => g.SkillId == gate.SkillId).Any(g => _coreMatchParticipantsPlayersAccess._players.Any(p => MobileJiKangTargetAllowed(new(g.SkillId, gate.Activation.Id, gate.OwnerSeat, g.SkillInstanceId), p.Seat)));
    internal SkillProgramStepOutcome BeginMobileJiKang(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MobileJiKang is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Mobile Ji Kang requires his original single issuing instruction.");
        int? killer = null;
        if (op == SkillProgramEffectOp.ResolveMobileJiKangJuexiang)
        {
            if (f.SkillId != "mobile:juexiang" || f.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied, SourceSeat: var source } || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                throw new InvalidOperationException("Juexiang requires the actual original owner-death window.");
            killer = source is { } seat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive ? seat : null;
        }
        else if (f.SkillId is not ("mobile:qingxian" or "mobile:canyun") || f.ActivationId != "equipment-comparison" || f.WindowContext is not null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].HasHpValue || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp <= 0 || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != f.OwnerSeat || MobileJiKangCosts(f.Id, f.OwnerSeat).Count == 0 || !_coreMatchParticipantsPlayersAccess._players.Any(p => MobileJiKangTargetAllowed(MobileJiKangSource(f), p.Seat)))
            throw new InvalidOperationException("Qingxian requires its exact current Play activation.");
        var r = new MobileJiKangReceipt
        {
            Source = MobileJiKangSource(f),
            Hash = f.GameplayHash,
            InitialHp = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp,
            Killer = killer,
            Stage = op == SkillProgramEffectOp.ResolveMobileJiKangJuexiang ? MobileJiKangStage.KillerDiscardChildren : MobileJiKangStage.Targets
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiKang = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiKangIssuedEvent(f.Id, r.Source, r.Hash, r.InitialHp, killer));
        if (op == SkillProgramEffectOp.ResolveMobileJiKangJuexiang && killer is { } actual)
        {
            var cards = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[actual]).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actual, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).Select(c => c.Id).ToArray();
            PayMobileJiKang(f, cards, actual, actual, MobileJiKangStage.KillerDiscardChildren);
        }

        ContinueMobileJiKang(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void PayMobileJiKang(ProgramSkillFrame f, IReadOnlyList<int> cards, int actor, int subject, MobileJiKangStage after)
    {
        var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiKang = f.MobileJiKang!with { Stage = after, Cards = cards, Before = before, After = before, Paid = cards.Count == 0 } });
        if (cards.Count == 0)
            return;
        var logical = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(actor, subject, cards, new(MobileJiKangDiscard), f.Id);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards, CardLocation.DiscardPile, new(MobileJiKangDiscard), (_, moves) =>
        {
            var live = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var actual = moves.Count(m => m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame);
            _coreRuntimeOperations.ReplaceRuntimeTop(live with { MobileJiKang = live.MobileJiKang!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence, Paid = actual == cards.Count } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(f.Id, "discard", live.MobileJiKang!.Cursor, subject, actual, actor, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
        });
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logical);
    }

    internal bool DrainMobileJiKang(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void StepMobileJiKang(ProgramSkillFrame f, MobileJiKangReceipt r)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileJiKang = r });
    }

    internal void FinishMobileJiKang(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(f.Id, "complete", f.MobileJiKang!.Cursor, f.MobileJiKang.Subject, 0, 0));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileJiKang = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void ContinueMobileJiKang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileJiKang: { } r } f || f.Id != id)
            return;
        if (f.NativeDraw is not null)
            return;
        if (r.Stage is MobileJiKangStage.Targets or MobileJiKangStage.Cards or MobileJiKangStage.Recipient or MobileJiKangStage.Club)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishMobileJiKang(f);
            return;
        }

        if (DrainMobileJiKang(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MobileJiKang!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMobileJiKang(f);
            return;
        }

        switch (r.Stage)
        {
            case MobileJiKangStage.CostChildren:
                if (!r.Paid)
                    throw new InvalidOperationException("Qingxian did not discard its exact number of selected targets.");
                foreach (var(seat, index)in r.Targets.Select((seat, index) => (seat, index)))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiKangTargetIssuedEvent(id, r.Source, index, seat));
                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Compare });
                ContinueMobileJiKang(id);
                return;
            case MobileJiKangStage.Compare:
                if (r.Cursor >= r.Targets.Count)
                {
                    StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Reward });
                    ContinueMobileJiKang(id);
                    return;
                }

                var target = r.Targets[r.Cursor];
                if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                {
                    StepMobileJiKang(f, r with { Cursor = r.Cursor + 1 });
                    ContinueMobileJiKang(id);
                    return;
                }

                var own = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
                var other = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).Count;
                var comparison = other.CompareTo(own);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(id, "compare", r.Cursor, target, own, other));
                if (comparison == 0)
                {
                    StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Drawing, Subject = target, Comparison = comparison });
                    _coreProgramNativeDrawOperations.BeginOperationProgramDraw(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), target, 1, new("skill-program.mobile-ji-kang.equal-draw"));
                    return;
                }

                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Health, Subject = target, Comparison = comparison, Paid = false });
                ContinueMobileJiKang(id);
                return;
            case MobileJiKangStage.Health:
                if (!_coreMatchParticipantsPlayersAccess._players[r.Subject].IsAlive)
                {
                    StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Compare, Cursor = r.Cursor + 1 });
                    ContinueMobileJiKang(id);
                    return;
                }

                if (r.Comparison < 0)
                {
                    StepMobileJiKang(f, r with { Stage = MobileJiKangStage.HealthChildren, Paid = true });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(id, "recover", r.Cursor, r.Subject, 1, 0));
                    _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(id, f.OwnerSeat, r.Subject, 1, null, null);
                    ContinueMobileJiKang(id);
                    return;
                }

                LoseMobileJiKangHp(f, r.Subject, MobileJiKangStage.HealthChildren);
                return;
            case MobileJiKangStage.HealthChildren:
                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Compare, Cursor = r.Cursor + 1 });
                ContinueMobileJiKang(id);
                return;
            case MobileJiKangStage.Reward:
                if (r.Targets.Count == r.InitialHp && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    StepMobileJiKang(f, r with { Stage = MobileJiKangStage.RewardDrawing, Subject = f.OwnerSeat });
                    _coreProgramNativeDrawOperations.BeginOperationProgramDraw(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), f.OwnerSeat, 1, new("skill-program.mobile-ji-kang.full-count-draw"));
                    return;
                }

                FinishMobileJiKang(f);
                return;
            case MobileJiKangStage.KillerDiscardChildren:
                if (r.Killer is { } killer && _coreMatchParticipantsPlayersAccess._players[killer].IsAlive)
                {
                    StepMobileJiKang(f, r with { Stage = MobileJiKangStage.KillerHealth, Subject = killer, Paid = false });
                    ContinueMobileJiKang(id);
                    return;
                }

                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Recipient });
                ContinueMobileJiKang(id);
                return;
            case MobileJiKangStage.KillerHealth:
                if (_coreMatchParticipantsPlayersAccess._players[r.Subject].IsAlive)
                {
                    LoseMobileJiKangHp(f, r.Subject, MobileJiKangStage.KillerHealthChildren);
                    return;
                }

                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Recipient });
                ContinueMobileJiKang(id);
                return;
            case MobileJiKangStage.KillerHealthChildren:
                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Recipient });
                ContinueMobileJiKang(id);
                return;
            case MobileJiKangStage.GrantChildren:
                if (!_coreMatchParticipantsPlayersAccess._players[r.Subject].IsAlive)
                {
                    FinishMobileJiKang(f);
                    return;
                }

                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Club });
                ContinueMobileJiKang(id);
                return;
            case MobileJiKangStage.ClubChildren:
                if (r.Paid && _coreMatchParticipantsPlayersAccess._players[r.Subject].IsAlive)
                {
                    StepMobileJiKang(f, r with { Stage = MobileJiKangStage.FinalGrantChildren });
                    _coreProgramSkillChangesOperations.GrantProgramSkills2(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), r.Subject, ["mobile:juexiang"]);
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(id, "inherit-juexiang", r.Cursor, r.Subject, 1, 0));
                    ContinueMobileJiKang(id);
                    return;
                }

                FinishMobileJiKang(f);
                return;
            case MobileJiKangStage.FinalGrantChildren:
                FinishMobileJiKang(f);
                return;
            default:
                throw new InvalidOperationException("Mobile Ji Kang lost its exact typed native return stage.");
        }
    }

    internal void LoseMobileJiKangHp(ProgramSkillFrame f, int target, MobileJiKangStage after)
    {
        if (_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(target) && _characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoProgramHealthChoice(f, target, HpChangeKind.Loss, 1))
            return;
        var outcome = _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, target, 1, (before, next, actual) =>
        {
            var live = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            StepMobileJiKang(live, live.MobileJiKang!with { Stage = after, Paid = true });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(f.Id, "lose-hp", live.MobileJiKang!.Cursor, target, actual, before - next));
            if (_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(target))
                _characterDaQiaoXiaoQiaoHappyGodOperations.CompleteHappyQiaoHealthPayment(f.Id, target, HpChangeKind.Loss, before, next);
        });
        if (outcome == SkillProgramStepOutcome.Continue)
            ContinueMobileJiKang(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileJiKangQingxian)]
    internal void MobileJiKangDrawReturned(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.MobileJiKang ?? throw new InvalidOperationException("Ji Kang draw lost its original receipt.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(f.Id, "draw-return", r.Cursor, r.Subject, outcome.ActualDrawIds.Count, 1, outcome.SequenceBefore, outcome.SequenceAfter));
        if (r.Stage == MobileJiKangStage.RewardDrawing)
        {
            FinishMobileJiKang(f);
            return;
        }

        StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Compare, Cursor = r.Cursor + 1 });
        ContinueMobileJiKang(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 80900)]
    internal bool ResumeMobileJiKang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileJiKang: not null } f || f.Id != id)
            return false;
        ContinueMobileJiKang(id);
        return true;
    }
}
